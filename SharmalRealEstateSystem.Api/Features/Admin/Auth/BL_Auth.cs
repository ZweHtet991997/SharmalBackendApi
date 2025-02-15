namespace SharmalRealEstateSystem.Api.Features.Admin.Auth;

public class BL_Auth
{
    #region Initializations

    private readonly IAdminUnitOfWork _adminUnitOfWork;
    private readonly JWTAuth _jWTAuth;
    private readonly RegisterValidator _registerValidator;
    private readonly AesService _aesService;
    private readonly UpdateAuthValidator _updateAuthValidator;
    private readonly TokenValidationService _tokenValidationService;
    private readonly UpdateProfileValidator _updateProfileValidator;

    public BL_Auth(
        JWTAuth jWTAuth,
        RegisterValidator registerValidator,
        IAdminUnitOfWork adminUnitOfWork,
        AesService aesService,
        UpdateAuthValidator updateAuthValidator,
        TokenValidationService tokenValidationService,
        UpdateProfileValidator updateProfileValidator
    )
    {
        _jWTAuth = jWTAuth;
        _registerValidator = registerValidator;
        _adminUnitOfWork = adminUnitOfWork;
        _aesService = aesService;
        _updateAuthValidator = updateAuthValidator;
        _tokenValidationService = tokenValidationService;
        _updateProfileValidator = updateProfileValidator;
    }

    #endregion

    public async Task<Result<UserListResponseModel>> GetUserList(int pageNo, int pageSize)
    {
        Result<UserListResponseModel> responseModel;
        try
        {
            if (pageNo <= 0)
            {
                responseModel = GetInvalidPageNoResult();
                goto result;
            }

            if (pageSize <= 0)
            {
                responseModel = GetInvalidPageSizeResult();
                goto result;
            }

            responseModel = await _adminUnitOfWork.AuthRepository.GetUserList(pageNo, pageSize);
        }
        catch (Exception ex)
        {
            responseModel = Result<UserListResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #region Login

    public async Task<Result<JwtResponseModel>> Login(LoginRequestModel requestModel)
    {
        Result<JwtResponseModel> responseModel;
        try
        {
            responseModel = await _adminUnitOfWork.AuthRepository.LoginV1(requestModel);
            if (responseModel.IsSuccess)
            {
                responseModel.Token = _jWTAuth.GetJWTToken(responseModel.Data);
                responseModel.Data = null!; // no need to return JWT response model
            }
        }
        catch (Exception ex)
        {
            responseModel = Result<JwtResponseModel>.FailureResult(ex);
        }

        return responseModel;
    }

    #endregion

    #region Register

    public async Task<Result<AuthResponseModel>> Register(RegisterRequestModel requestModel)
    {
        Result<AuthResponseModel> responseModel;
        try
        {
            #region Enum Validation

            if (!Enum.IsDefined(typeof(EnumUserRole), requestModel.UserRole))
            {
                responseModel = GetInvalidUserRoleResult();
                goto result;
            }

            #endregion

            #region Password Policy

            var passwordPolicy = PasswordPolicyChecker.ValidatePassword(
                _aesService.DecryptString(requestModel.Password)
            );
            if (!string.IsNullOrEmpty(passwordPolicy))
            {
                responseModel = Result<AuthResponseModel>.FailureResult(
                    passwordPolicy,
                    EnumStatusCode.BadRequest
                );
                goto result;
            }

            #endregion

            responseModel = await _adminUnitOfWork.AuthRepository.Register(requestModel);
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
            if (userId.IsNullOrEmpty())
            {
                responseModel = GetUserIdInvalidResult();
                goto result;
            }

            responseModel = await _adminUnitOfWork.AuthRepository.ResetFailCount(userId);
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
        HttpContext context
    )
    {
        Result<AuthResponseModel> responseModel;
        try
        {
            #region User Id Checking

            if (userId.IsNullOrEmpty())
            {
                responseModel = GetUserIdInvalidResult();
                goto result;
            }

            #endregion

            #region User Role Checking

            if (requestModel.UserRole is not null && !requestModel.UserRole.IsNullOrEmpty())
            {
                if (!Enum.IsDefined(typeof(EnumUserRole), requestModel.UserRole!))
                {
                    responseModel = GetInvalidUserRoleResult();
                    goto result;
                }
            }

            #endregion

            #region Admin Validation

            //if (requestModel.UserRole!.Equals(EnumUserRole.Admin.ToString()))
            //{
            //    ValidationResult validationResult = await _updateAuthValidator.ValidateAsync(requestModel);
            //    if (!validationResult.IsValid)
            //    {
            //        string errors = string.Join(" ", validationResult.Errors.Select(x => x.ErrorMessage));
            //        responseModel = Result<AuthResponseModel>.FailureResult(errors);
            //        goto result;
            //    }
            //}

            #endregion

            #region Old Password

            if (requestModel.OldPassword is not null && !requestModel.OldPassword.IsNullOrEmpty())
            {
                #region Password Policy

                var passwordPolicy = PasswordPolicyChecker.ValidatePassword(
                    _aesService.DecryptString(requestModel.OldPassword)
                );
                if (!string.IsNullOrEmpty(passwordPolicy))
                {
                    responseModel = Result<AuthResponseModel>.FailureResult(
                        passwordPolicy,
                        EnumStatusCode.BadRequest
                    );
                    goto result;
                }

                #endregion

                if (requestModel.NewPassword is null || requestModel.NewPassword.IsNullOrEmpty())
                {
                    responseModel = Result<AuthResponseModel>.FailureResult(
                        "New Password is required."
                    );
                    goto result;
                }
            }
            #endregion

            #region New Password

            else if (
                requestModel.NewPassword is not null
                && !requestModel.NewPassword.IsNullOrEmpty()
            )
            {
                #region Password Policy

                var passwordPolicy = PasswordPolicyChecker.ValidatePassword(
                    _aesService.DecryptString(requestModel.NewPassword)
                );
                if (!string.IsNullOrEmpty(passwordPolicy))
                {
                    responseModel = Result<AuthResponseModel>.FailureResult(
                        passwordPolicy,
                        EnumStatusCode.BadRequest
                    );
                    goto result;
                }

                #endregion

                if (requestModel.OldPassword is null || requestModel.OldPassword.IsNullOrEmpty())
                {
                    responseModel = Result<AuthResponseModel>.FailureResult(
                        "Old Password is required."
                    );
                    goto result;
                }
            }

            #endregion

            string authHeader = context.Request.Headers["Authorization"]!;
            string[] header_and_token = authHeader.Split(' ');
            string token = header_and_token[1];
            var principal = _tokenValidationService.ValidateToken(token);
            var decryptedActionMakerId = _aesService.DecryptString(
                principal.FindFirst("UserId")!.Value
            );

            responseModel = await _adminUnitOfWork.AuthRepository.UpdateUser(
                requestModel,
                userId,
                decryptedActionMakerId
            );
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
        HttpContext context
    )
    {
        Result<AuthResponseModel> responseModel;
        try
        {
            if (userId.IsNullOrEmpty())
            {
                responseModel = GetUserIdInvalidResult();
                goto result;
            }

            string authHeader = context.Request.Headers["Authorization"]!;
            string[] header_and_token = authHeader.Split(' ');
            string token = header_and_token[1];
            var principal = _tokenValidationService.ValidateToken(token);
            var decryptedActionTakerId = _aesService.DecryptString(
                principal.FindFirst("UserId")!.Value
            );

            responseModel = await _adminUnitOfWork.AuthRepository.UpdateProfile(
                requestModel,
                userId,
                decryptedActionTakerId
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
        HttpContext context
    )
    {
        Result<AuthResponseModel> responseModel;
        try
        {
            if (userId.IsNullOrEmpty())
            {
                responseModel = GetUserIdInvalidResult();
                goto result;
            }

            if (requestModel.NewPassword is null || requestModel.NewPassword.IsNullOrEmpty())
            {
                responseModel = Result<AuthResponseModel>.FailureResult(
                    "New Password cannot be empty."
                );
                goto result;
            }

            #region Password Policy

            var passwordPolicy = PasswordPolicyChecker.ValidatePassword(
                _aesService.DecryptString(requestModel.NewPassword)
            );
            if (!string.IsNullOrEmpty(passwordPolicy))
            {
                responseModel = Result<AuthResponseModel>.FailureResult(
                    passwordPolicy,
                    EnumStatusCode.BadRequest
                );
                goto result;
            }

            #endregion

            string authHeader = context.Request.Headers["Authorization"]!;
            string[] header_and_token = authHeader.Split(' ');
            string token = header_and_token[1];
            var principal = _tokenValidationService.ValidateToken(token);
            var decryptedActionMakerId = _aesService.DecryptString(
                principal.FindFirst("UserId")!.Value
            );
            var decryptedRole = _aesService.DecryptString(principal.FindFirstValue("UserRole")!);

            if (decryptedRole == Convert.ToString(EnumUserRole.Staff))
            {
                if (requestModel.OldPassword is null || requestModel.OldPassword.IsNullOrEmpty())
                {
                    responseModel = Result<AuthResponseModel>.FailureResult(
                        "Old Password cannot be empty."
                    );
                    goto result;
                }
            }

            responseModel = await _adminUnitOfWork.AuthRepository.UpdatePassword(
                requestModel,
                userId,
                decryptedActionMakerId
            );
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

        if (userId.IsNullOrEmpty())
        {
            responseModel = Result<AuthResponseModel>.FailureResult(MessageResource.InvalidId);
            goto result;
        }

        responseModel = await _adminUnitOfWork.AuthRepository.DeleteUser(userId);

    result:
        return responseModel;
    }

    private Result<AuthResponseModel> GetUserIdInvalidResult() =>
        Result<AuthResponseModel>.FailureResult(MessageResource.InvalidId);

    private Result<UserListResponseModel> GetInvalidPageNoResult() =>
        Result<UserListResponseModel>.FailureResult(MessageResource.InvalidPageNo);

    private Result<UserListResponseModel> GetInvalidPageSizeResult() =>
        Result<UserListResponseModel>.FailureResult(MessageResource.InvalidPageSize);

    private Result<AuthResponseModel> GetInvalidUserRoleResult() =>
        Result<AuthResponseModel>.FailureResult("User Role is invalid.");
}
