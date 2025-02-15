namespace SharmalRealEstateSystem.Repositories.Features.Admin.Auth;

public class AuthRepository : IAuthRepository
{
    private readonly AppDbContext _context;
    private readonly AesService _aesService;

    public AuthRepository(AppDbContext context, AesService aesService)
    {
        _context = context;
        _aesService = aesService;
    }

    #region Get User List

    public async Task<Result<UserListResponseModel>> GetUserList(int pageNo, int pageSize)
    {
        Result<UserListResponseModel> responseModel;
        try
        {
            var query = _context.TblUsers.Where(x => !x.IsDeleted).OrderByDescending(x => x.UserId);

            var lst = await query.Pagination(pageNo, pageSize).ToListAsync();

            var totalCount = await query.CountAsync();
            var pageCount = totalCount / pageSize;

            if (totalCount % pageSize > 0)
            {
                pageCount++;
            }

            var pageSettingModel = new PageSettingModel(pageNo, pageSize, pageCount, totalCount);
            var model = new UserListResponseModel(
                lst.Select(x => x.Map()).ToList(),
                pageSettingModel
            );

            responseModel = Result<UserListResponseModel>.SuccessResult(model);
        }
        catch (Exception ex)
        {
            responseModel = Result<UserListResponseModel>.FailureResult(ex);
        }

        return responseModel;
    }

    #endregion

    #region Login

    public async Task<Result<JwtResponseModel>> Login(LoginRequestModel requestModel)
    {
        Result<JwtResponseModel> responseModel;
        try
        {
            #region Check Email Exist

            var user = await _context.TblUsers.FirstOrDefaultAsync(x =>
                x.Email == requestModel.Email
                && !x.IsDeleted
                && x.UserRole == EnumUserRole.Admin.ToString()
            );
            if (user is null)
            {
                responseModel = Result<JwtResponseModel>.FailureResult(
                    MessageResource.NotFound,
                    EnumStatusCode.NotFound
                );
                goto result;
            }

            #endregion

            #region Check attempts

            if (user.FailedCount >= 5)
            {
                responseModel = Result<JwtResponseModel>.FailureResult(
                    "Your Account is locked.",
                    EnumStatusCode.Locked
                );
                goto result;
            }

            #endregion

            if (!user.Password.Equals(requestModel.Password))
            {
                user.FailedCount++;
                responseModel = Result<JwtResponseModel>.FailureResult(
                    "Password is incorrect.",
                    EnumStatusCode.NotFound
                );
            }
            else
            {
                user.FailedCount = 0;
                JwtResponseModel jwtResponseModel =
                    new(
                        _aesService.EncryptString(user.UserId),
                        _aesService.EncryptString(user.UserName),
                        _aesService.EncryptString(user.Email),
                        _aesService.EncryptString(user.UserRole)
                    );
                responseModel = Result<JwtResponseModel>.SuccessResult(jwtResponseModel);
            }

            _context.Update(user);
            int result = await _context.SaveChangesAsync();

            if (result <= 0)
            {
                responseModel = Result<JwtResponseModel>.FailureResult(
                    "Error while updating the attempts.",
                    EnumStatusCode.InternalServerError
                );
                goto result;
            }
        }
        catch (Exception ex)
        {
            responseModel = Result<JwtResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Login V1

    public async Task<Result<JwtResponseModel>> LoginV1(LoginRequestModel requestModel)
    {
        Result<JwtResponseModel> responseModel;
        try
        {
            var user = await GetUserByEmailV1(requestModel.Email!);

            #region User Not Found

            if (user is null)
            {
                responseModel = Result<JwtResponseModel>.FailureResult(
                    "User Not Found.",
                    EnumStatusCode.NotFound
                );
                goto result;
            }

            #endregion

            var lockedAccountResult = GetLockedAccountResult(user);

            #region Check Locked Account

            if (lockedAccountResult is not null)
            {
                responseModel = lockedAccountResult;
                goto result;
            }

            #endregion

            if (!IsPasswordCorrect(user, requestModel.Password!))
                responseModel = IncrementFailCountResult(user);
            else
            {
                ResetFailCount(user);
                var jwtResponseModel = GetJwtResponseModel(user);
                responseModel = Result<JwtResponseModel>.SuccessResult(jwtResponseModel);
            }

            #region Save Changes

            _context.Update(user);
            int result = await _context.SaveChangesAsync();

            #endregion

            #region Save Changes Fail

            if (result <= 0)
            {
                responseModel = Result<JwtResponseModel>.FailureResult(
                    "Error while updating the attempts.",
                    EnumStatusCode.InternalServerError
                );
                goto result;
            }

            #endregion
        }
        catch (Exception ex)
        {
            responseModel = Result<JwtResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Register

    public async Task<Result<AuthResponseModel>> Register(RegisterRequestModel requestModel)
    {
        Result<AuthResponseModel> responseModel;
        try
        {
            #region Check Admin

            bool isAdmin = await IsAdmin(requestModel.CreatedBy!);
            if (!isAdmin)
            {
                responseModel = Result<AuthResponseModel>.FailureResult(
                    "Admin Not Found.",
                    EnumStatusCode.NotFound
                );
                goto result;
            }

            #endregion

            #region Check Email Duplicate

            var user = await GetUserByEmailV1(requestModel.Email);
            if (user is not null)
            {
                responseModel = Result<AuthResponseModel>.FailureResult(
                    MessageResource.EmailDuplicate,
                    EnumStatusCode.Conflict
                );
                goto result;
            }

            #endregion

            await _context.TblUsers.AddAsync(requestModel.Map());
            int result = await _context.SaveChangesAsync();

            responseModel = Result<AuthResponseModel>.ExecuteResult(result);
        }
        catch (Exception ex)
        {
            responseModel = Result<AuthResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Reset Fail Count

    public async Task<Result<AuthResponseModel>> ResetFailCount(string userId)
    {
        Result<AuthResponseModel> responseModel;
        try
        {
            var item = await _context.TblUsers.FirstOrDefaultAsync(x =>
                x.UserId == userId && !x.IsDeleted
            );
            if (item is null)
            {
                responseModel = GetUserNotFoundResult();
                goto result;
            }

            item.FailedCount = 0;
            _context.TblUsers.Update(item);
            await _context.SaveChangesAsync();

            responseModel = GetUserUpdateSuccessResult();
        }
        catch (Exception ex)
        {
            responseModel = Result<AuthResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    public async Task<Result<AuthResponseModel>> UpdateUser(
        UpdateAuthRequestModel requestModel,
        string userId,
        string decryptedActionMakerId
    )
    {
        Result<AuthResponseModel> responseModel;
        try
        {
            var item = await _context.TblUsers.FindAsync(userId);
            if (item is null)
            {
                responseModel = GetUserNotFoundResult();
                goto result;
            }

            var actionMaker = await _context.TblUsers.FirstOrDefaultAsync(x =>
                x.UserId == decryptedActionMakerId && !x.IsDeleted
            );
            if (actionMaker is null)
            {
                responseModel = GetUserNotFoundResult();
                goto result;
            }

            #region Only Admin can change User Role

            if (
                actionMaker.UserRole != Convert.ToString(EnumUserRole.Admin)
                && !requestModel.UserRole!.IsNullOrEmpty()
            )
            {
                responseModel = Result<AuthResponseModel>.FailureResult(
                    "Only Admin can change User Role."
                );
                goto result;
            }

            #endregion

            #region Cannot change own User Role

            if (decryptedActionMakerId == userId && !requestModel.UserRole!.IsNullOrEmpty())
            {
                responseModel = Result<AuthResponseModel>.FailureResult(
                    "You cannot change your User Role."
                );
                goto result;
            }

            #endregion

            #region Email Duplicate

            bool emailDuplicate = await _context.TblUsers.AnyAsync(x =>
                x.Email == requestModel.Email! && !x.IsDeleted && x.UserId != userId
            );
            if (emailDuplicate)
            {
                responseModel = Result<AuthResponseModel>.FailureResult(
                    MessageResource.EmailDuplicate,
                    EnumStatusCode.Conflict
                );
                goto result;
            }

            #endregion

            #region Action Maker is Admin

            if (actionMaker.UserRole == Convert.ToString(EnumUserRole.Admin))
            {
                if (requestModel.UserName is null || requestModel.UserName.IsNullOrEmpty())
                {
                    responseModel = Result<AuthResponseModel>.FailureResult(
                        "User name cannot be empty."
                    );
                    goto result;
                }

                if (requestModel.Email is null || requestModel.Email.IsNullOrEmpty())
                {
                    responseModel = Result<AuthResponseModel>.FailureResult(
                        "Email cannot be empty."
                    );
                    goto result;
                }

                if (requestModel.OldPassword is not null && requestModel.NewPassword is not null)
                {
                    var decryptedOldPassword = _aesService.DecryptString(requestModel.OldPassword!);
                    var decryptedDbPassword = _aesService.DecryptString(item.Password);

                    #region Old Password incorrect case

                    if (!decryptedOldPassword.Equals(decryptedDbPassword))
                    {
                        var lockedAccountResult = GetUpdateLockedAccountResult(item);
                        if (lockedAccountResult is not null)
                        {
                            responseModel = lockedAccountResult;
                            goto result;
                        }

                        item.FailedCount += 1;
                        _context.TblUsers.Update(item);
                        await _context.SaveChangesAsync();

                        responseModel = Result<AuthResponseModel>.FailureResult(
                            "Old Password is incorrect."
                        );
                        goto result;
                    }

                    #endregion
                }

                item.UserName = requestModel.UserName!;
                item.Email = requestModel.Email!;
                item.UserRole = requestModel.UserRole!;
                item.UpdatedBy = decryptedActionMakerId;
                if (
                    requestModel.NewPassword is not null
                    && !requestModel.NewPassword.IsNullOrEmpty()
                )
                {
                    item.Password = requestModel.NewPassword;
                }
                item.FailedCount = 0;

                _context.TblUsers.Update(item);
                await _context.SaveChangesAsync();

                responseModel = Result<AuthResponseModel>.SuccessResult(
                    message: MessageResource.UpdateSuccess
                );
                goto result;
            }

            #endregion


            #region Action Maker is Staff

            if (actionMaker.UserRole == Convert.ToString(EnumUserRole.Staff))
            {
                #region Staff cannot update other records

                if (decryptedActionMakerId != userId)
                {
                    actionMaker.FailedCount += 1;
                    _context.TblUsers.Update(actionMaker);
                    await _context.SaveChangesAsync();

                    responseModel = Result<AuthResponseModel>.FailureResult(
                        "You cannot edit this record."
                    );
                    goto result;
                }

                #endregion

                var decryptedOldPassword = _aesService.DecryptString(requestModel.OldPassword!);
                var decryptedDbPassword = _aesService.DecryptString(item.Password);

                #region Old Password incorrect case

                if (!decryptedOldPassword.Equals(decryptedDbPassword))
                {
                    var lockedAccountResult = GetUpdateLockedAccountResult(item);
                    if (lockedAccountResult is not null)
                    {
                        responseModel = lockedAccountResult;
                        goto result;
                    }

                    item.FailedCount += 1;
                    _context.TblUsers.Update(item);
                    await _context.SaveChangesAsync();

                    responseModel = Result<AuthResponseModel>.FailureResult(
                        "Old Password is incorrect."
                    );
                    goto result;
                }

                #endregion

                item.Password = requestModel.NewPassword!;
                _context.TblUsers.Update(item);
                await _context.SaveChangesAsync();

                responseModel = Result<AuthResponseModel>.SuccessResult(
                    message: MessageResource.UpdateSuccess
                );
                goto result;
            }

            #endregion

            responseModel = Result<AuthResponseModel>.FailureResult();
        }
        catch (Exception ex)
        {
            responseModel = Result<AuthResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    public async Task<Result<AuthResponseModel>> UpdateProfile(
        UpdateProfileRequestModel requestModel,
        string userId,
        string decryptedActionTakerId
    )
    {
        Result<AuthResponseModel> responseModel;
        try
        {
            var item = await _context.TblUsers.FindAsync(userId);
            if (item is null)
            {
                responseModel = Result<AuthResponseModel>.FailureResult(
                    MessageResource.NotFound,
                    EnumStatusCode.NotFound
                );
                goto result;
            }

            bool emailDuplicate = await _context.TblUsers.AnyAsync(x =>
                x.Email == requestModel.Email && !x.IsDeleted && x.UserId != userId
            );
            if (emailDuplicate)
            {
                responseModel = Result<AuthResponseModel>.FailureResult(
                    MessageResource.EmailDuplicate,
                    EnumStatusCode.Conflict
                );
                goto result;
            }

            item.UserName = requestModel.UserName;
            item.Email = requestModel.Email;
            if (userId != decryptedActionTakerId)
            {
                item.UserRole = requestModel.UserRole!;
            }
            item.UpdatedBy = decryptedActionTakerId;

            _context.TblUsers.Update(item);
            await _context.SaveChangesAsync();

            responseModel = Result<AuthResponseModel>.SuccessResult(
                message: MessageResource.UpdateSuccess
            );
        }
        catch (Exception ex)
        {
            responseModel = Result<AuthResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    public async Task<Result<AuthResponseModel>> UpdatePassword(
        UpdatePasswordRequestModel requestModel,
        string userId,
        string decryptedActionTakerId
    )
    {
        Result<AuthResponseModel> responseModel;
        try
        {
            var item = await _context.TblUsers.FindAsync(userId);
            if (item is null)
            {
                responseModel = Result<AuthResponseModel>.FailureResult(
                    "User Not Found.",
                    EnumStatusCode.NotFound
                );
                goto result;
            }

            var decryptedOldDbPassword = _aesService.DecryptString(item.Password);
            var decryptedRequestPassword = _aesService.DecryptString(requestModel.OldPassword!);

            var actionTaker = await _context.TblUsers.FindAsync(decryptedActionTakerId);
            if (actionTaker is null)
            {
                responseModel = Result<AuthResponseModel>.FailureResult(
                    "User Not Found.",
                    EnumStatusCode.NotFound
                );
                goto result;
            }

            if (actionTaker.UserRole == Convert.ToString(EnumUserRole.Admin))
            {
                if (decryptedOldDbPassword != decryptedRequestPassword)
                {
                    responseModel = Result<AuthResponseModel>.FailureResult("Old Password is invalid.");
                    goto result;
                }

                item.Password = requestModel.NewPassword!;
                _context.TblUsers.Update(item);
                await _context.SaveChangesAsync();

                responseModel = Result<AuthResponseModel>.SuccessResult(
                    message: MessageResource.UpdateSuccess
                );
                goto result;
            }

            if (actionTaker.UserRole == Convert.ToString(EnumUserRole.Staff))
            {
                if (userId != decryptedActionTakerId)
                {
                    var lockedAccountResult = GetUpdateLockedAccountResult(actionTaker);
                    if (lockedAccountResult is not null)
                    {
                        responseModel = lockedAccountResult;
                        goto result;
                    }

                    actionTaker.FailedCount += 1;
                    _context.TblUsers.Update(actionTaker);
                    await _context.SaveChangesAsync();

                    responseModel = Result<AuthResponseModel>.FailureResult(
                        "You cannot edit this record."
                    );
                    goto result;
                }

                if (decryptedOldDbPassword != decryptedRequestPassword)
                {
                    var lockedAccountResult = GetUpdateLockedAccountResult(item);
                    if (lockedAccountResult is not null)
                    {
                        responseModel = lockedAccountResult;
                        goto result;
                    }

                    item.FailedCount += 1;
                    _context.TblUsers.Update(item);
                    await _context.SaveChangesAsync();

                    responseModel = Result<AuthResponseModel>.FailureResult(
                        "Old Password is incorrect."
                    );
                    goto result;
                }

                item.Password = requestModel.NewPassword!;
                item.FailedCount = 0;

                _context.TblUsers.Update(item);
                await _context.SaveChangesAsync();

                responseModel = Result<AuthResponseModel>.SuccessResult(
                    message: MessageResource.UpdateSuccess
                );
                goto result;
            }

            responseModel = Result<AuthResponseModel>.FailureResult();
        }
        catch (Exception ex)
        {
            responseModel = Result<AuthResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    public async Task<Result<AuthResponseModel>> DeleteUser(string userId)
    {
        Result<AuthResponseModel> responseModel;
        try
        {
            var item = await _context.TblUsers.FindAsync(userId);
            if (item is null)
            {
                responseModel = Result<AuthResponseModel>.FailureResult(
                    "User Not Found.",
                    EnumStatusCode.NotFound
                );
                goto result;
            }

            item.IsDeleted = true;
            _context.TblUsers.Update(item);
            await _context.SaveChangesAsync();

            responseModel = Result<AuthResponseModel>.SuccessResult(
                message: MessageResource.DeleteSuccess
            );
        }
        catch (Exception ex)
        {
            responseModel = Result<AuthResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    private async Task<TblUser?> GetUserByEmail(string email)
    {
        return await _context.TblUsers.FirstOrDefaultAsync(x =>
            x.Email == email && !x.IsDeleted && x.UserRole == EnumUserRole.Admin.ToString()
        );
    }

    private async Task<TblUser?> GetUserByEmailV1(string email)
    {
        return await _context.TblUsers.FirstOrDefaultAsync(x => x.Email == email && !x.IsDeleted);
    }

    private async Task<bool> IsAdmin(string userId)
    {
        return await _context.TblUsers.AnyAsync(x =>
            x.UserId == userId && !x.IsDeleted && x.UserRole == Convert.ToString(EnumUserRole.Admin)
        );
    }

    private Result<JwtResponseModel> GetLockedAccountResult(TblUser user)
    {
        if (user.FailedCount >= 5)
        {
            return Result<JwtResponseModel>.FailureResult(
                "Your Account is locked.",
                EnumStatusCode.Locked
            );
        }

        return null!;
    }

    private Result<AuthResponseModel> GetUpdateLockedAccountResult(TblUser user)
    {
        if (user.FailedCount >= 5)
        {
            return Result<AuthResponseModel>.FailureResult(
                "Your Account is locked.",
                EnumStatusCode.Locked
            );
        }

        return null!;
    }

    private bool IsPasswordCorrect(TblUser user, string password)
    {
        return _aesService.DecryptString(user.Password).Equals(_aesService.DecryptString(password));
    }

    private JwtResponseModel GetJwtResponseModel(TblUser user)
    {
        return new JwtResponseModel(
            _aesService.EncryptString(user.UserId),
            _aesService.EncryptString(user.UserName),
            _aesService.EncryptString(user.Email),
            _aesService.EncryptString(user.UserRole)
        );
    }

    private Result<JwtResponseModel> IncrementFailCountResult(TblUser user)
    {
        user.FailedCount++;

        return Result<JwtResponseModel>.FailureResult(
            MessageResource.LoginFail,
            EnumStatusCode.NotFound
        );
    }

    private void ResetFailCount(TblUser user)
    {
        user.FailedCount = 0;
    }

    private Result<AuthResponseModel> GetUserNotFoundResult() =>
        Result<AuthResponseModel>.FailureResult("User Not Found.", EnumStatusCode.NotFound);

    private Result<AuthResponseModel> GetUserUpdateSuccessResult() =>
        Result<AuthResponseModel>.SuccessResult(
            MessageResource.UpdateSuccess,
            EnumStatusCode.Success
        );
}
