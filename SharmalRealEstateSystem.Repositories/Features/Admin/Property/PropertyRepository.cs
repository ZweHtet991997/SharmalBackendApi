global using SharmalRealEstateSystem.Models.Features.Admin.PropertyFeature;

namespace SharmalRealEstateSystem.Repositories.Features.Admin.Property;

public class PropertyRepository : IPropertyRepository
{
    private readonly AppDbContext _context;
    private readonly FtpService _ftpService;
    private readonly DapperService _dapperService;
    private readonly AesService _aesService;

    public PropertyRepository(
        AppDbContext context,
        FtpService ftpService,
        DapperService dapperService,
        AesService aesService
    )
    {
        _context = context;
        _ftpService = ftpService;
        _dapperService = dapperService;
        _aesService = aesService;
    }

    #region Get List

    public async Task<Result<PropertyListResponseModel>> GetList(
        GetPropertyListRequestModel requestModel
    )
    {
        Result<PropertyListResponseModel> responseModel;
        try
        {
            var parameters = new
            {
                Location = requestModel.Location!.IsNullOrEmpty() ? null : requestModel.Location,
                City = requestModel.City!.IsNullOrEmpty() ? null : requestModel.City,
                Furnished = requestModel.Furnished!.IsNullOrEmpty() ? null : requestModel.Furnished,
                Type = requestModel.Type!.IsNullOrEmpty() ? null : requestModel.Type,
                Code = requestModel.Code!.IsNullOrEmpty() ? null : requestModel.Code,
                PaymentOption = requestModel.PaymentOption!.IsNullOrEmpty()
                    ? null
                    : requestModel.PaymentOption,
                Status = requestModel.Status!.IsNullOrEmpty() ? null : requestModel.Status,
                MinPrice = requestModel.MinPrice is null || requestModel.MinPrice <= 0
                    ? null
                    : requestModel.MinPrice,
                MaxPrice = requestModel.MaxPrice is null || requestModel.MaxPrice <= 0
                    ? null
                    : requestModel.MaxPrice,
                MinBedRooms = requestModel.MinBedRooms is null || requestModel.MinBedRooms <= 0
                    ? null
                    : requestModel.MinBedRooms,
                MaxBedRooms = requestModel.MaxBedRooms is null || requestModel.MaxBedRooms <= 0
                    ? null
                    : requestModel.MaxBedRooms,
                MinBathRooms = requestModel.MinBathRooms is null || requestModel.MinBathRooms <= 0
                ? null
                : requestModel.MinBathRooms,
                MaxBathRooms = requestModel.MaxBathRooms is null || requestModel.MinBathRooms <= 0
                ? null
                : requestModel.MinBathRooms,
                IsPopular = requestModel.IsPopular is null ? null : requestModel.IsPopular,
                IsHotDeal = requestModel.IsHotDeal is null ? null : requestModel.IsHotDeal,
                requestModel.PageNo,
                requestModel.PageSize
            };
            List<PropertyModel> lst = await _dapperService.QueryAsync<PropertyModel>(
                CommonQuery.GetFilterProperty,
                parameters,
                commandType: CommandType.StoredProcedure
            );

            var totalCountParams = new
            {
                Location = requestModel.Location!.IsNullOrEmpty() ? null : requestModel.Location,
                City = requestModel.City!.IsNullOrEmpty() ? null : requestModel.City,
                Furnished = requestModel.Furnished!.IsNullOrEmpty() ? null : requestModel.Furnished,
                Type = requestModel.Type!.IsNullOrEmpty() ? null : requestModel.Type,
                Code= requestModel.Code!.IsNullOrEmpty() ? null : requestModel.Code,
                PaymentOption = requestModel.PaymentOption!.IsNullOrEmpty()
                    ? null
                    : requestModel.PaymentOption,
                Status = requestModel.Status!.IsNullOrEmpty() ? null : requestModel.Status,
                MinPrice = requestModel.MinPrice is null || requestModel.MinPrice <= 0
                    ? null
                    : requestModel.MinPrice,
                MaxPrice = requestModel.MaxPrice is null || requestModel.MaxPrice <= 0
                    ? null
                    : requestModel.MaxPrice,
                MinBedRooms = requestModel.MinBedRooms is null || requestModel.MinBedRooms <= 0
                    ? null
                    : requestModel.MinBedRooms,
                MaxBedRooms = requestModel.MaxBedRooms is null || requestModel.MaxBedRooms <= 0
                    ? null
                    : requestModel.MaxBedRooms,
                MinBathRooms = requestModel.MinBathRooms is null || requestModel.MinBathRooms <= 0
                ? null
                : requestModel.MinBathRooms,
                MaxBathRooms = requestModel.MaxBathRooms is null || requestModel.MinBathRooms <= 0
                ? null
                : requestModel.MinBathRooms,
                IsPopular = requestModel.IsPopular is null ? null : requestModel.IsPopular,
                IsHotDeal = requestModel.IsHotDeal is null ? null : requestModel.IsHotDeal,
            };
            var totalCount = await _dapperService.GetTotalCountAsync(
                CommonQuery.PropertyCountResult,
                totalCountParams,
                CommandType.StoredProcedure
            );
            var pageCount = totalCount / requestModel.PageSize;
            if (totalCount % requestModel.PageSize > 0)
            {
                pageCount++;
            }

            List<PropertyDataModel> dataLst = new();

            foreach (var item in lst)
            {
                string fetchPropertyFeatureLstQuery = CommonQuery.GetPropertyFeatureListByPropertyId;
                var featureLstByProperty = await _dapperService.QueryAsync<PropertyFeatureModel>(fetchPropertyFeatureLstQuery, new { PropertyId = item.PropertyId });
                var imageListByProperty = await GetImageListByProperty(item.PropertyId);

                PropertyDataModel propertyDataModel =
                    new(
                        item,
                        featureLstByProperty,
                        imageListByProperty.Select(x => x.Map(item.CreatedBy)).ToList()
                    );
                dataLst.Add(propertyDataModel);
            }

            var pageSettingModel = new PageSettingModel(
                requestModel.PageNo,
                requestModel.PageSize,
                pageCount,
                totalCount
            );
            var model = new PropertyListResponseModel(dataLst, pageSettingModel);
            responseModel = Result<PropertyListResponseModel>.SuccessResult(model);

            goto result;
        }
        catch (Exception ex)
        {
            responseModel = Result<PropertyListResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    public async Task<Result<PropertyDataModel>> GetPropertyByPropertyId(string id)
    {
        Result<PropertyDataModel> responseModel;
        try
        {
            var property = await _context.TblProperties.FindAsync(id);
            if (property is null)
            {
                responseModel = Result<PropertyDataModel>.FailureResult(MessageResource.NotFound, EnumStatusCode.NotFound);
                goto result;
            }

            string query = CommonQuery.GetPropertyFeatureListByPropertyId;
            var parameters = new
            {
                PropertyId = property.PropertyId
            };
            var featureLstByProperty = await _dapperService.QueryAsync<PropertyFeatureModel>(query, parameters);
            var imgLstByProperty = await _context.TblImages.Where(x => x.PropertyId == property.PropertyId).ToListAsync();

            var model = new PropertyDataModel(property.Map(),
                featureLstByProperty,
                imgLstByProperty.Select(x => x.Map(property.CreatedBy)).ToList());

            responseModel = Result<PropertyDataModel>.SuccessResult(model);
        }
        catch (Exception ex)
        {
            responseModel = Result<PropertyDataModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    public async Task<Result<PropertyListResponseModel>> GetListByUserId(
        string userId,
        int pageNo,
        int pageSize
    )
    {
        Result<PropertyListResponseModel> responseModel;
        try
        {
            var user = await _context.TblUsers.FindAsync(userId);
            if (user is null)
            {
                responseModel = Result<PropertyListResponseModel>.FailureResult(
                    "User Not Found.",
                    EnumStatusCode.NotFound
                );
                goto result;
            }

            var query = _context
                .TblProperties.OrderByDescending(x => x.PropertyId)
                .Where(x => x.CreatedBy == userId || x.UpdatedBy == userId && !x.IsDeleted);
            var propertyLstByUser = await query.Pagination(pageNo, pageSize).ToListAsync();

            var totalCount = await query.CountAsync();
            var pageCount = totalCount / pageSize;
            if (totalCount % pageSize > 0)
            {
                pageCount++;
            }

            var dataLst = new List<PropertyDataModel>();
            foreach (var item in propertyLstByUser)
            {
                var featureLstByProperty = await GetFeatureListByProperty(item.PropertyId);
                var imgLstByProperty = await GetImageListByProperty(item.PropertyId);

                dataLst.Add(
                    new PropertyDataModel(
                        item.Map(),
                        featureLstByProperty.Select(x => x.Map()).ToList(),
                        imgLstByProperty.Select(x => x.Map(item.CreatedBy)).ToList()
                    )
                );
            }

            var pageSettingModel = new PageSettingModel(pageNo, pageSize, pageCount, totalCount);
            var model = new PropertyListResponseModel(dataLst, pageSettingModel);

            responseModel = Result<PropertyListResponseModel>.SuccessResult(model);
        }
        catch (Exception ex)
        {
            responseModel = Result<PropertyListResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #region Create Property

    public async Task<Result<PropertyResponseModel>> CreateProperty(
        PropertyRequestModel requestModel
    )
    {
        Result<PropertyResponseModel> responseModel;
        var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            #region Check Admin Role

            //bool isAdmin = await IsAdmin(requestModel.CreatedBy);
            //if (!isAdmin)
            //{
            //    responseModel = Result<PropertyResponseModel>.FailureResult(
            //        MessageResource.NotFound,
            //        EnumStatusCode.NotFound
            //    );
            //    goto result;
            //}

            #endregion

            #region Directory Create / Check

            string directoryName = GetPropertyDirectory(requestModel.CreatedBy);
            bool directoryExist = await _ftpService.CheckDirectoryExistsAsync(directoryName);

            if (!directoryExist)
            {
                bool directoryCreateSuccess = await _ftpService.CreateDirectoryAsync(directoryName);
                if (!directoryCreateSuccess)
                {
                    responseModel = Result<PropertyResponseModel>.FailureResult(
                        "Creating Directory Fail.",
                        EnumStatusCode.BadRequest
                    );
                    goto result;
                }
            }

            #endregion

            #region Save Property

            var propertyModel = requestModel.Map();
            await _context.TblProperties.AddAsync(propertyModel);
            int result = await _context.SaveChangesAsync();

            #endregion

            #region Upload Files to Ftp and to Db

            foreach (var file in requestModel.Files!)
            {
                string fileName = file.GetFileNameV1();
                await _ftpService.UploadFileAsync(file, directoryName, fileName);

                var imageModel = new TblImage
                {
                    ImageId = Ulid.NewUlid().ToString(),
                    ImageName = fileName,
                    PropertyId = propertyModel.PropertyId
                };
                await _context.TblImages.AddAsync(imageModel);
            }
            int imageSaveChanges = await _context.SaveChangesAsync();

            #endregion

            #region Save Features

            if (requestModel.Features is not null && requestModel.Features.Count > 0)
            {
                var features = requestModel
                    .Features!.Select(x => x.Map(propertyModel.PropertyId))
                    .ToList();

                #region Validate valid features

                foreach (var feature in features)
                {
                    bool isFeatureValid = await _context.TblFeatures.AnyAsync(x =>
                        !x.IsDeleted && x.FeatureId == feature.FeatureId
                    );

                    #region if feature invalid

                    if (!isFeatureValid)
                    {
                        responseModel = Result<PropertyResponseModel>.FailureResult(
                            "Features is already deleted."
                        );
                        await transaction.RollbackAsync();
                        goto result;
                    }

                    #endregion

                    bool propertyFeatureDuplicate = await _context.TblPropertyFeatures.AnyAsync(x =>
                        x.FeatureId == feature.FeatureId && x.PropertyId == propertyModel.PropertyId
                    );

                    #region If Duplicate Feature

                    if (propertyFeatureDuplicate)
                    {
                        responseModel = Result<PropertyResponseModel>.FailureResult(
                            "Feature Duplicate.",
                            EnumStatusCode.Conflict
                        );
                        await transaction.RollbackAsync();
                        goto result;
                    }

                    #endregion

                    await _context.TblPropertyFeatures.AddAsync(feature);
                    int featureSaveChanges = await _context.SaveChangesAsync();

                    if (featureSaveChanges <= 0)
                    {
                        responseModel = Result<PropertyResponseModel>.FailureResult(
                            MessageResource.SaveFail,
                            EnumStatusCode.BadRequest
                        );
                        await transaction.RollbackAsync();
                        goto result;
                    }
                }

                #endregion
            }

            #endregion

            if (result > 0 && imageSaveChanges == requestModel.Files.Count)
            {
                await transaction.CommitAsync();
                responseModel = Result<PropertyResponseModel>.SuccessResult();
                goto result;
            }

            await transaction.RollbackAsync();
            responseModel = Result<PropertyResponseModel>.FailureResult(
                MessageResource.SaveFail,
                EnumStatusCode.BadRequest
            );
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            responseModel = Result<PropertyResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Create Property V1

    public async Task<Result<PropertyResponseModel>> CreatePropertyV1(
        PropertyRequestModel requestModel
    )
    {
        Result<PropertyResponseModel> responseModel;
        var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            #region Check Admin Role

            //if (!await IsAdmin(requestModel.CreatedBy))
            //{
            //    responseModel = GetUserNotFoundResult();
            //    goto result;
            //}

            #endregion

            #region Directory Create / Check

            string directoryName = GetPropertyDirectory(requestModel.CreatedBy);

            if (!await _ftpService.CheckDirectoryExistsAsync(directoryName))
            {
                if (!await _ftpService.CreateDirectoryAsync(directoryName))
                {
                    responseModel = GetCreateDirectoryFailResult();
                    goto result;
                }
            }

            #endregion

            #region Save Property

            var propertyModel = requestModel.Map();
            await _context.TblProperties.AddAsync(propertyModel);
            await _context.SaveChangesAsync();

            #endregion

            #region Upload Files to Ftp and to Db

            foreach (var file in requestModel.Files!)
            {
                string fileName = file.GetFileNameV1();
                await _ftpService.UploadFileAsync(file, directoryName, fileName);

                var imageModel = GetTblImageModel(fileName, propertyModel.PropertyId);
                await _context.TblImages.AddAsync(imageModel);
            }

            #endregion

            #region Save Features

            if (requestModel.Features is not null && requestModel.Features.Count > 0)
            {
                var features = requestModel
                    .Features!.Select(x => x.Map(propertyModel.PropertyId))
                    .ToList();

                if (!requestModel.Features.Any(x => x.FeatureId!.IsNullOrEmpty()))
                {
                    #region Validate valid features

                    foreach (var feature in features)
                    {
                        #region if feature invalid

                        if (!await FeatureValid(feature.FeatureId))
                        {
                            responseModel = GetFeatureInvalidResult();
                            await transaction.RollbackAsync();
                            goto result;
                        }

                        #endregion

                        #region If Duplicate Feature

                        bool propertyFeatureDuplicate = await PropertyFeatureDuplicate(
                            feature.FeatureId,
                            propertyModel.PropertyId
                        );

                        if (propertyFeatureDuplicate)
                        {
                            responseModel = GetPropertyFeatureDuplicateResult();
                            await transaction.RollbackAsync();
                            goto result;
                        }
                        await _context.TblPropertyFeatures.AddAsync(feature);

                        #endregion
                    }
                }

                #endregion
            }

            #endregion

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            responseModel = Result<PropertyResponseModel>.SuccessResult(
                MessageResource.SaveSuccess,
                EnumStatusCode.Success
            );
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            responseModel = Result<PropertyResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Update Property

    public async Task<Result<PropertyResponseModel>> UpdateProperty(
        UpdatePropertyRequestModel requestModel,
        string propertyId
    )
    {
        Result<PropertyResponseModel> responseModel;
        var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            #region Check Admin Exist

            //bool isAdmin = await IsAdmin(requestModel.UpdatedBy!);
            //if (!isAdmin)
            //{
            //    responseModel = Result<PropertyResponseModel>.FailureResult(
            //        MessageResource.NotFound,
            //        EnumStatusCode.NotFound
            //    );
            //    goto result;
            //}

            #endregion

            #region Verify User

            bool isUserVerified = await VerifyUser(requestModel.CreatedBy!);
            if (!isUserVerified)
            {
                responseModel = Result<PropertyResponseModel>.FailureResult(
                    "User Not Found.",
                    EnumStatusCode.NotFound
                );
                goto result;
            }

            #endregion

            #region Check Property Exist

            var property = await GetPropertyById(propertyId);
            if (property is null)
            {
                responseModel = Result<PropertyResponseModel>.FailureResult(
                    MessageResource.NotFound,
                    EnumStatusCode.NotFound
                );
                goto result;
            }

            #endregion

            #region Check Created By vs Updated By

            //if (!property.CreatedBy.Equals(requestModel.UpdatedBy))
            //{
            //    responseModel = Result<PropertyResponseModel>.FailureResult(
            //        "User is invalid.",
            //        EnumStatusCode.NotFound
            //    );
            //    goto result;
            //}

            #endregion

            #region Update Property

            property.Title = requestModel.Title;
            property.Code = requestModel.Code;
            property.Status = requestModel.Status;
            property.Type = requestModel.Type;
            property.Price = requestModel.Price;
            property.PaymentOption = requestModel.PaymentOption;
            property.Location = requestModel.Location;
            property.City = requestModel.City;
            property.NumberOfViewers = requestModel.NumberOfViewers;
            property.Bedrooms = requestModel.Bedrooms;
            property.Area = requestModel.Area;
            property.Condition = requestModel.Condition;
            property.Floor = requestModel.Floor;
            property.Description = requestModel.Description;
            property.Furnished = requestModel.Furnished;
            property.MapUrl = requestModel.MapUrl;
            property.SellerName = requestModel.SellerName;
            property.PrimaryPhoneNumber = requestModel.PrimaryPhoneNumber;
            property.SecondaryPhoneNumber = requestModel.SecondaryPhoneNumber;
            property.Email = requestModel.Email;
            property.Address = requestModel.Address;
            property.UpdatedBy = requestModel.UpdatedBy;
            property.UpdatedDate = DevCode.GetCurrentMyanmarDateTime();
            property.IsPopular = requestModel.IsPopular;
            property.IsHotDeal = requestModel.IsHotDeal;

            _context.Update(property);

            #endregion

            var featureLst = await GetFeatureListByProperty(propertyId);
            var imageLst = await GetImageListByProperty(propertyId);

            if (featureLst is not null && featureLst.Count > 0)
            {
                _context.TblPropertyFeatures.RemoveRange(featureLst);
            }

            #region Check Features, Delete Property Features, Upload New Property Features

            if (requestModel.Features is not null && requestModel.Features.Count > 0)
            {
                #region Check Valid Features

                foreach (var feature in requestModel.Features!)
                {
                    //var item = await _context.TblFeatures.FindAsync(feature.FeatureId);
                    bool isFeatureValid = await _context.TblFeatures.AnyAsync(x =>
                        x.FeatureId == feature.FeatureId && !x.IsDeleted
                    );

                    if (!isFeatureValid)
                    {
                        responseModel = Result<PropertyResponseModel>.FailureResult(
                            "Feature is already deleted.",
                            EnumStatusCode.NotFound
                        );
                        goto result;
                    }
                }

                #endregion

                await _context.TblPropertyFeatures.AddRangeAsync(
                    requestModel.Features.Select(x => x.Map(propertyId))
                );
            }

            #endregion

            #region Remove Images List

            if (requestModel.RemoveImages is not null && requestModel.RemoveImages.Count > 0)
            {
                var imageLstToRemove = imageLst
                    .Where(image =>
                        requestModel.RemoveImages.Any(removeImage =>
                            removeImage.ImageId == image.ImageId
                        )
                    )
                    .ToList();

                foreach (var removeImage in imageLstToRemove)
                {
                    _context.TblImages.Remove(removeImage);
                    string filePath =
                        $"{GetPropertyDirectory(property.CreatedBy)}/{removeImage.ImageName}";

                    await _ftpService.DeleteFileAsync(filePath);
                }
            }

            #endregion

            if (requestModel.Files is not null && requestModel.Files.Count > 0)
            {
                #region Insert into Db & Upload to Ftp

                foreach (var file in requestModel.Files)
                {
                    var fileName = file.GetFileNameV1();
                    var imageModel = GetTblImageModel(fileName, property.PropertyId);
                    await _context.TblImages.AddAsync(imageModel);

                    string directory = GetPropertyDirectory(property.CreatedBy);
                    bool isDirectoryExists = await _ftpService.CheckDirectoryExistsAsync(directory);
                    if (!isDirectoryExists)
                    {
                        bool isCreateSuccess = await _ftpService.CreateDirectoryAsync(directory);
                        if (!isCreateSuccess)
                        {
                            responseModel = GetCreateDirectoryFailResult();
                            goto result;
                        }
                    }

                    await _ftpService.UploadFileAsync(file, directory, fileName);
                }

                #endregion
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            responseModel = Result<PropertyResponseModel>.SuccessResult(
                MessageResource.UpdateSuccess,
                EnumStatusCode.Success
            );
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            responseModel = Result<PropertyResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Delete Property

    public async Task<Result<PropertyResponseModel>> DeleteProperty(string propertyId)
    {
        Result<PropertyResponseModel> responseModel;
        try
        {
            var property = await GetPropertyById(propertyId);

            if (propertyId is null)
            {
                responseModel = Result<PropertyResponseModel>.FailureResult(
                    MessageResource.NotFound,
                    EnumStatusCode.NotFound
                );
                goto result;
            }
            var featureLst = await GetFeatureListByProperty(property!.PropertyId);
            var imageLst = await GetImageListByProperty(property!.PropertyId);

            foreach (var image in imageLst)
            {
                var filePath = GetPropertyDirectory(property.CreatedBy) + $"/{image.ImageName}";
                await _ftpService.DeleteFileAsync(filePath);
            }

            property.IsDeleted = true;
            _context.TblPropertyFeatures.RemoveRange(featureLst);
            _context.TblImages.RemoveRange(imageLst);

            _context.TblProperties.Update(property);
            int result = await _context.SaveChangesAsync();

            responseModel = Result<PropertyResponseModel>.ExecuteResult(result);
        }
        catch (Exception ex)
        {
            responseModel = Result<PropertyResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Delete Property V1

    public async Task<Result<PropertyResponseModel>> DeletePropertyV1(string propertyId)
    {
        Result<PropertyResponseModel> responseModel;
        try
        {
            var property = await GetPropertyById(propertyId);

            if (propertyId is null)
            {
                responseModel = GetPropertyNotFoundResult();
                goto result;
            }
            var featureLst = await GetFeatureListByProperty(property!.PropertyId);
            var imageLst = await GetImageListByProperty(property!.PropertyId);

            foreach (var image in imageLst)
            {
                var filePath = GetPropertyDirectory(property.CreatedBy) + $"/{image.ImageName}";
                await _ftpService.DeleteFileAsync(filePath);
            }

            property.IsDeleted = true;
            if (featureLst is not null && featureLst.Count > 0)
            {
                _context.TblPropertyFeatures.RemoveRange(featureLst);
            }
            _context.TblImages.RemoveRange(imageLst);

            _context.TblProperties.Update(property);
            await _context.SaveChangesAsync();

            responseModel = Result<PropertyResponseModel>.SuccessResult(
                MessageResource.DeleteSuccess,
                EnumStatusCode.Success
            );
        }
        catch (Exception ex)
        {
            responseModel = Result<PropertyResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    private async Task<List<TblProperty>> GetPropertyListByDesc(
        IQueryable<TblProperty> query,
        int pageNo,
        int pageSize
    )
    {
        return await query.Where(x => !x.IsDeleted).Pagination(pageNo, pageSize).ToListAsync();
    }

    private async Task<List<TblPropertyFeature>> GetFeatureListByProperty(string propertyId)
    {
        return await _context
            .TblPropertyFeatures.Where(x => x.PropertyId == propertyId)
            .ToListAsync();
    }

    private async Task<List<TblImage>> GetImageListByProperty(string propertyId)
    {
        return await _context.TblImages.Where(x => x.PropertyId == propertyId).ToListAsync();
    }

    private async Task<bool> IsAdmin(string userId)
    {
        return await _context.TblUsers.AnyAsync(x =>
            x.UserId == userId && !x.IsDeleted && x.UserRole == EnumUserRole.Admin.ToString()
        );
    }

    private async Task<bool> VerifyUser(string userId)
    {
        return await _context.TblUsers.AnyAsync(x => x.UserId == userId && !x.IsDeleted);
    }

    private string GetPropertyDirectory(string userId)
    {
        return $"{userId}/{EnumFtpFolderName.Property.ToString()}";
    }

    private async Task<TblProperty> GetPropertyById(string propertyId)
    {
        var item = await _context.TblProperties.FirstOrDefaultAsync(x =>
            x.PropertyId == propertyId && !x.IsDeleted
        );

        return item!;
    }

    private Result<PropertyResponseModel> GetUserNotFoundResult() =>
        Result<PropertyResponseModel>.FailureResult("User Not Found.", EnumStatusCode.NotFound);

    private Result<PropertyResponseModel> GetCreateDirectoryFailResult() =>
        Result<PropertyResponseModel>.FailureResult(
            MessageResource.CreateDirectoryFail,
            EnumStatusCode.BadRequest
        );

    private Result<PropertyResponseModel> GetImageSaveFailResult() =>
        Result<PropertyResponseModel>.FailureResult(MessageResource.SaveFail);

    private Result<PropertyResponseModel> GetFeatureInvalidResult() =>
        Result<PropertyResponseModel>.FailureResult("Features is already deleted.");

    private Result<PropertyResponseModel> GetPropertyFeatureDuplicateResult() =>
        Result<PropertyResponseModel>.FailureResult("Feature Duplicate.", EnumStatusCode.Conflict);

    private Result<PropertyResponseModel> GetFeatureSaveFailResult() =>
        Result<PropertyResponseModel>.FailureResult(
            MessageResource.SaveFail,
            EnumStatusCode.BadRequest
        );

    private Result<PropertyResponseModel> GetPropertyNotFoundResult() =>
        Result<PropertyResponseModel>.FailureResult("Property Not Found.", EnumStatusCode.NotFound);

    private TblImage GetTblImageModel(string fileName, string propertyId)
    {
        return new TblImage
        {
            ImageId = Ulid.NewUlid().ToString(),
            ImageName = fileName,
            PropertyId = propertyId
        };
    }

    private async Task<int> UploadFilesToFtpSaveChangesToDb(
        List<IFormFile> files,
        string directoryName,
        string propertyId
    )
    {
        foreach (var file in files)
        {
            string fileName = file.GetFileNameV1();
            await _ftpService.UploadFileAsync(file, directoryName, fileName);

            var imageModel = GetTblImageModel(fileName, propertyId);
            await _context.TblImages.AddAsync(imageModel);
        }
        return await _context.SaveChangesAsync();
    }

    private async Task<bool> FeatureValid(string featureId)
    {
        return await _context.TblFeatures.AnyAsync(x => !x.IsDeleted && x.FeatureId == featureId);
    }

    private async Task<bool> PropertyFeatureDuplicate(string featureId, string propertyId)
    {
        return await _context.TblPropertyFeatures.AnyAsync(x =>
            x.FeatureId == featureId && x.PropertyId == propertyId
        );
    }
}
