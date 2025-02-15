namespace SharmalRealEstateSystem.Repositories.Features.Admin.Ads;

public class AdsRepository : IAdsRepository
{
    private readonly AppDbContext _context;
    private readonly FtpService _ftpService;
    private readonly AesService _aesService;
    private readonly DapperService _dapperService;

    public AdsRepository(
        AppDbContext context,
        FtpService ftpService,
        AesService aesService,
        DapperService dapperService
    )
    {
        _context = context;
        _ftpService = ftpService;
        _aesService = aesService;
        _dapperService = dapperService;
    }

    #region Get Ads List

    public async Task<Result<AdsListResponseModel>> GetAdsList(
        FilterAdsRequestModel filterAdsRequestModel
    )
    {
        Result<AdsListResponseModel> responseModel;
        try
        {
            var parameters = new
            {
                Status = filterAdsRequestModel.Status!.IsNullOrEmpty()
                    ? null
                    : filterAdsRequestModel.Status,
                Pages = filterAdsRequestModel.Pages!.IsNullOrEmpty()
                    ? null
                    : filterAdsRequestModel.Pages,
                Layout = filterAdsRequestModel.Layout!.IsNullOrEmpty()
                    ? null
                    : filterAdsRequestModel.Layout,
                filterAdsRequestModel.PageNo,
                filterAdsRequestModel.PageSize
            };
            var lst = await _dapperService.QueryAsync<AdsModel>(
                CommonQuery.GetFilterAds,
                parameters,
                CommandType.StoredProcedure
            );
            var totalCount = await _dapperService.GetTotalCountAsync(
                CommonQuery.AdsCountResult,
                new
                {
                    filterAdsRequestModel.Pages,
                    filterAdsRequestModel.Status,
                    filterAdsRequestModel.Layout
                }
            );
            var pageCount = totalCount / filterAdsRequestModel.PageSize;

            if (totalCount % filterAdsRequestModel.PageSize > 0)
            {
                pageCount++;
            }

            List<AdsDataModel> adsDataModels = new();
            foreach (var item in lst)
            {
                var imgLstByAds = await _context
                    .TblImages.Where(x => x.AdsId == item.AdsId)
                    .ToListAsync();
                var adsPagePlacementLstByAds = await _context
                    .TblAdsPagePlacements.Where(x => x.AdsId == item.AdsId)
                    .ToListAsync();

                adsDataModels.Add(GetAdsDataModel(item, imgLstByAds, adsPagePlacementLstByAds));
            }

            var pageSettingModel = GetPageSettingModel(
                filterAdsRequestModel.PageNo,
                filterAdsRequestModel.PageSize,
                pageCount,
                totalCount
            );
            var model = new AdsListResponseModel(adsDataModels, pageSettingModel);

            responseModel = Result<AdsListResponseModel>.SuccessResult(model);
        }
        catch (Exception ex)
        {
            responseModel = Result<AdsListResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    public async Task<Result<AdsListResponseModel>> GetAdsListByUserId(
        string userId,
        int pageNo,
        int pageSize
    )
    {
        Result<AdsListResponseModel> responseModel;
        try
        {
            var user = await _context.TblUsers.FindAsync(userId);
            if (user is null)
            {
                responseModel = Result<AdsListResponseModel>.FailureResult(
                    "User Not Found.",
                    EnumStatusCode.NotFound
                );
                goto result;
            }

            var query = _context
                .TblAds.OrderByDescending(x => x.AdsId)
                .Where(x => x.CreatedBy == userId || x.UpdatedBy == userId && !x.IsDeleted);
            var totalCount = await query.CountAsync();
            var pageCount = totalCount / pageSize;
            if (totalCount % pageSize > 0)
            {
                pageCount++;
            }
            var adsLst = await query.Pagination(pageNo, pageSize).ToListAsync();

            var dataLst = new List<AdsDataModel>();
            foreach (var item in adsLst)
            {
                var adsPagePlacementLst = await GetAdsPagePlacementsByAdsId(item.AdsId);
                var imgLst = await GetImagesByAdsId(item.AdsId);

                dataLst.Add(
                    new AdsDataModel()
                    {
                        Ads = item.Map(),
                        AdsPagePlacements = adsPagePlacementLst.Select(x => x.Map()).ToList(),
                        Images = imgLst
                            .Select(x => new AdsImageModel()
                            {
                                AdsId = x.AdsId!,
                                ImageId = x.ImageId,
                                ImageName = x.ImageName
                            })
                            .ToList()
                    }
                );
            }

            var pageSettingModel = new PageSettingModel(pageNo, pageSize, pageCount, totalCount);
            var model = new AdsListResponseModel(dataLst, pageSettingModel);

            responseModel = Result<AdsListResponseModel>.SuccessResult(model);
        }
        catch (Exception ex)
        {
            responseModel = Result<AdsListResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #region Create Ads

    public async Task<Result<AdsResponseModel>> CreateAds(AdsRequestModel requestModel)
    {
        Result<AdsResponseModel> responseModel;
        var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            #region Is Admin

            if (!await IsAdmin(requestModel.CreatedBy))
            {
                responseModel = GetUserNotFoundResult();
                goto result;
            }

            #endregion

            #region Save Ads

            var adsModel = requestModel.Map();
            await _context.TblAds.AddAsync(adsModel);

            #endregion

            #region Ads Page Placement

            foreach (var placement in requestModel.AdsPagePlacements!)
            {
                bool isValid = await _context.TblAdsPages.AnyAsync(x =>
                    x.AdsPageId == placement.AdsPageId && !x.IsDeleted
                );
                if (!isValid)
                {
                    responseModel = Result<AdsResponseModel>.FailureResult("Page is invalid.");
                    goto result;
                }

                await _context.TblAdsPagePlacements.AddAsync(placement.Map(adsModel.AdsId));
            }

            #endregion

            #region Check / Create Directory

            string directoryName = GetAdsDirectory(requestModel.CreatedBy);

            if (!await _ftpService.CheckDirectoryExistsAsync(directoryName))
            {
                if (!await _ftpService.CreateDirectoryAsync(directoryName))
                {
                    responseModel = GetCreateDirectoryFailResult();
                    goto result;
                }
            }

            #endregion

            #region Save to TblImage & Upload to Ftp

            var uploadFilesSaveTblImageResult = await UploadFilesSaveTblImageResult(
                requestModel.Files!,
                directoryName,
                adsModel.AdsId
            );
            if (!uploadFilesSaveTblImageResult.IsSuccess)
            {
                responseModel = uploadFilesSaveTblImageResult;
                await transaction.RollbackAsync();
                goto result;
            }

            #endregion

            await transaction.CommitAsync();
            responseModel = Result<AdsResponseModel>.SuccessResult();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            responseModel = Result<AdsResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Create Ads V1

    public async Task<Result<AdsResponseModel>> CreateAdsV1(AdsRequestModel requestModel)
    {
        Result<AdsResponseModel> responseModel;
        var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            #region Is Admin

            //bool isAdmin = await IsAdmin(requestModel.CreatedBy);

            //if (!isAdmin)
            //{
            //    responseModel = GetUserNotFoundResult();
            //    goto result;
            //}

            #endregion

            #region Save Ads

            var adsModel = requestModel.Map();
            await _context.TblAds.AddAsync(adsModel);

            #endregion

            #region Ads Page Placement

            foreach (var placement in requestModel.AdsPagePlacements!)
            {
                bool isValid = await IsAdsPageValid(placement.AdsPageId);
                if (!isValid)
                {
                    responseModel = Result<AdsResponseModel>.FailureResult("Page is invalid.");
                    goto result;
                }

                await _context.TblAdsPagePlacements.AddAsync(placement.Map(adsModel.AdsId));
            }

            #endregion

            #region Check / Create Directory

            string directoryName = GetAdsDirectory(requestModel.CreatedBy);

            bool directoryExists = await _ftpService.CheckDirectoryExistsAsync(directoryName);
            if (!directoryExists)
            {
                bool directoryCreateSuccess = await _ftpService.CreateDirectoryAsync(directoryName);
                if (!directoryCreateSuccess)
                {
                    responseModel = GetCreateDirectoryFailResult();
                    goto result;
                }
            }

            #endregion

            #region Save to TblImage & Upload to Ftp

            var uploadFilesSaveTblImageResult = await UploadFilesSaveTblImageResult(
                requestModel.Files!,
                directoryName,
                adsModel.AdsId
            );
            if (!uploadFilesSaveTblImageResult.IsSuccess)
            {
                responseModel = uploadFilesSaveTblImageResult;
                await transaction.RollbackAsync();
                goto result;
            }

            #endregion

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            responseModel = Result<AdsResponseModel>.SuccessResult();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            responseModel = Result<AdsResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Update Ads

    public async Task<Result<AdsResponseModel>> UpdateAds(
        UpdateAdsRequestModel requestModel,
        string adsId
    )
    {
        Result<AdsResponseModel> responseModel;
        try
        {
            #region Check Admin

            if (!await IsAdmin(requestModel.UpdatedBy))
            {
                responseModel = GetUserNotFoundResult();
                goto result;
            }

            #endregion

            #region Get Active Ads By Id

            var item = await GetActiveAdsById(adsId);
            if (item is null)
            {
                responseModel = GetAdsNotFoundResult();
                goto result;
            }

            #endregion

            #region Check Created By & Updated By

            if (!item.CreatedBy.Equals(requestModel.UpdatedBy))
            {
                responseModel = GetUserNotFoundResult();
                goto result;
            }

            #endregion

            #region Ads Update

            item.Title = requestModel.Title;
            item.TargetUrl = requestModel.TargetUrl;
            item.StartDate = requestModel.StartDate;
            item.EndDate = requestModel.EndDate;
            item.UpdatedBy = requestModel.UpdatedBy;

            _context.TblAds.Update(item);

            #endregion

            #region Old Ads Page Placement Remove Range

            var adsPagePlacementLst = await _context
                .TblAdsPagePlacements.Where(x => x.AdsId == adsId)
                .ToListAsync();
            if (adsPagePlacementLst is not null)
            {
                _context.TblAdsPagePlacements.RemoveRange(adsPagePlacementLst);
            }

            #endregion

            #region Check Ads Page Valid

            foreach (var adsPagePlacement in requestModel.AdsPagePlacements!)
            {
                bool isValid = await _context.TblAdsPages.AnyAsync(x =>
                    x.AdsPageId == adsPagePlacement.AdsPageId && !x.IsDeleted
                );
                if (!isValid)
                {
                    responseModel = Result<AdsResponseModel>.FailureResult("Page is invalid.");
                    goto result;
                }
            }

            #endregion

            #region Ads Page Placement Add Range

            var adsPagePlacementLstToInsert = requestModel
                .AdsPagePlacements.Select(x => x.Map(item.AdsId))
                .ToList();
            await _context.TblAdsPagePlacements.AddRangeAsync(adsPagePlacementLstToInsert);

            #endregion

            #region Remove File form Db & Ftp

            var directoryName = GetAdsDirectory(requestModel.UpdatedBy);

            if (requestModel.RemoveFiles is not null && requestModel.RemoveFiles.Count > 0)
            {
                foreach (var file in requestModel.RemoveFiles!)
                {
                    var image = await _context.TblImages.FirstOrDefaultAsync(x =>
                        x.ImageId == file.ImageId
                    );
                    if (image is null)
                    {
                        responseModel = Result<AdsResponseModel>.FailureResult(
                            "Image Not Found.",
                            EnumStatusCode.NotFound
                        );
                        goto result;
                    }

                    var filePath = directoryName + $"/{image.ImageName}";
                    await _ftpService.DeleteFileAsync(filePath);

                    _context.TblImages.Remove(image);
                }
            }

            #endregion

            #region Upload Files & Insert into Db

            if (requestModel.Files is not null && requestModel.Files.Count > 0)
            {
                foreach (var file in requestModel.Files!)
                {
                    var fileName = file.GetFileNameV1();
                    var tblImageModel = GetTblImageModel(item.AdsId, fileName);

                    await _context.TblImages.AddAsync(tblImageModel);
                    await _ftpService.UploadFileAsync(file, directoryName, fileName);
                }
            }

            #endregion

            await _context.SaveChangesAsync();
            responseModel = Result<AdsResponseModel>.SuccessResult();
        }
        catch (Exception ex)
        {
            responseModel = Result<AdsResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Update Ads V1

    public async Task<Result<AdsResponseModel>> UpdateAdsV1(
        UpdateAdsRequestModel requestModel,
        string adsId
    )
    {
        Result<AdsResponseModel> responseModel;
        try
        {
            #region Check Admin

            //bool isAdmin = await IsAdmin(requestModel.UpdatedBy);
            //if (!isAdmin)
            //{
            //    responseModel = GetUserNotFoundResult();
            //    goto result;
            //}

            #endregion

            #region Get Active Ads By Id

            var item = await GetActiveAdsById(adsId);
            if (item is null)
            {
                responseModel = GetAdsNotFoundResult();
                goto result;
            }

            #endregion

            #region Check Created By & Updated By

            //if (!item.CreatedBy.Equals(requestModel.UpdatedBy))
            //{
            //    responseModel = GetUserNotFoundResult();
            //    goto result;
            //}

            #endregion

            #region Ads Update

            item.Title = requestModel.Title;
            item.TargetUrl = requestModel.TargetUrl;
            item.StartDate = requestModel.StartDate;
            item.EndDate = requestModel.EndDate;
            item.UpdatedBy = requestModel.UpdatedBy;

            _context.TblAds.Update(item);

            #endregion

            #region Old Ads Page Placement Remove Range

            var adsPagePlacementLst = await GetAdsPagePlacementsByAdsId(adsId);
            if (adsPagePlacementLst is not null)
            {
                _context.TblAdsPagePlacements.RemoveRange(adsPagePlacementLst);
            }

            #endregion

            #region Check Ads Page Valid

            foreach (var adsPagePlacement in requestModel.AdsPagePlacements!)
            {
                bool isValid = await AdsPageValid(adsPagePlacement.AdsPageId);
                if (!isValid)
                {
                    responseModel = Result<AdsResponseModel>.FailureResult("Page is invalid.");
                    goto result;
                }
            }

            #endregion

            #region Ads Page Placement Add Range

            var adsPagePlacementLstToInsert = requestModel
                .AdsPagePlacements.Select(x => x.Map(item.AdsId))
                .ToList();

            await _context.TblAdsPagePlacements.AddRangeAsync(adsPagePlacementLstToInsert);

            #endregion

            #region Remove File form Db & Ftp

            var directoryName = GetAdsDirectory(requestModel.UpdatedBy);

            if (requestModel.RemoveFiles is not null && requestModel.RemoveFiles.Count > 0)
            {
                foreach (var file in requestModel.RemoveFiles!)
                {
                    var image = await GetImageById(file.ImageId);
                    if (image is null)
                    {
                        responseModel = GetImageNotFoundResult();
                        goto result;
                    }

                    var filePath = directoryName + $"/{image.ImageName}";
                    await _ftpService.DeleteFileAsync(filePath);

                    _context.TblImages.Remove(image);
                }
            }

            #endregion

            #region Upload Files & Insert into Db

            if (requestModel.Files is not null && requestModel.Files.Count > 0)
            {
                foreach (var file in requestModel.Files!)
                {
                    var fileName = file.GetFileNameV1();
                    var tblImageModel = GetTblImageModel(item.AdsId, fileName);
                    bool isDirectoryExist = await _ftpService.CheckDirectoryExistsAsync(
                        directoryName
                    );
                    if (!isDirectoryExist)
                    {
                        bool isCreateSuccess = await _ftpService.CreateDirectoryAsync(
                            directoryName
                        );
                        if (!isCreateSuccess)
                        {
                            responseModel = Result<AdsResponseModel>.FailureResult();
                            goto result;
                        }
                    }

                    await _context.TblImages.AddAsync(tblImageModel);
                    await _ftpService.UploadFileAsync(file, directoryName, fileName);
                }
            }

            #endregion

            await _context.SaveChangesAsync();
            responseModel = Result<AdsResponseModel>.SuccessResult();
        }
        catch (Exception ex)
        {
            responseModel = Result<AdsResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Patch Ads

    //public async Task<Result<AdsResponseModel>> PatchAds(string adsId)
    //{
    //    Result<AdsResponseModel> responseModel;
    //    try
    //    {
    //        var item = await GetActiveAdsById(adsId);
    //        if (item is null)
    //        {
    //            responseModel = GetAdsNotFoundResult();
    //            goto result;
    //        }

    //        item.IsActive = !item.IsActive;
    //        _context.Update(item);
    //        await _context.SaveChangesAsync();

    //        responseModel = Result<AdsResponseModel>.SuccessResult();
    //    }
    //    catch (Exception ex)
    //    {
    //        responseModel = Result<AdsResponseModel>.FailureResult(ex);
    //    }

    //result:
    //    return responseModel;
    //}

    #endregion

    #region Delete Ads

    public async Task<Result<AdsResponseModel>> DeleteAds(string adsId)
    {
        Result<AdsResponseModel> responseModel;
        try
        {
            #region Ads Not Found Case, Delete Ads

            var item = await GetActiveAdsById(adsId);
            if (item is null)
            {
                responseModel = GetAdsNotFoundResult();
                goto result;
            }

            item.IsDeleted = true;
            _context.TblAds.Update(item);

            #endregion

            #region Get Img List By Ads Id

            var imgLst = await _context.TblImages.Where(x => x.AdsId == adsId).ToListAsync();

            #endregion

            #region Delete Ads Page Placement List By Ads Id

            var pagePlacementLst = await _context
                .TblAdsPagePlacements.Where(x => x.AdsId == adsId)
                .ToListAsync();

            _context.TblAdsPagePlacements.RemoveRange(pagePlacementLst);

            #endregion

            #region Delete Image From Db & Ftp

            foreach (var image in imgLst)
            {
                var filePath = GetAdsDirectory(item.CreatedBy) + $"/{image.ImageName}";
                await _ftpService.DeleteFileAsync(filePath);

                _context.TblImages.Remove(image);
            }

            #endregion

            await _context.SaveChangesAsync();

            responseModel = Result<AdsResponseModel>.SuccessResult();
        }
        catch (Exception ex)
        {
            responseModel = Result<AdsResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    private string GetAdsDirectory(string userId)
    {
        return $"{userId}/{Convert.ToString(EnumFtpFolderName.Ads)}";
    }

    private Result<AdsResponseModel> GetCreateDirectoryFailResult() =>
        Result<AdsResponseModel>.FailureResult(MessageResource.CreateDirectoryFail);

    private async Task<Result<AdsResponseModel>> UploadFilesSaveTblImageResult(
        List<IFormFile> files,
        string directoryName,
        string adsId
    )
    {
        Result<AdsResponseModel> responseModel;
        try
        {
            foreach (var file in files)
            {
                var fileName = file.GetFileNameV1();
                await _ftpService.UploadFileAsync(file, directoryName, fileName);

                var tblImgModel = GetTblImageModel(adsId, fileName);
                await _context.TblImages.AddAsync(tblImgModel);
            }

            await _context.SaveChangesAsync();

            responseModel = Result<AdsResponseModel>.SuccessResult();
        }
        catch (Exception ex)
        {
            responseModel = Result<AdsResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    private TblImage GetTblImageModel(string adsId, string fileName)
    {
        return new TblImage
        {
            ImageId = Ulid.NewUlid().ToString(),
            AdsId = adsId,
            ImageName = fileName
        };
    }

    private async Task<bool> IsAdmin(string userId)
    {
        return await _context.TblUsers.AnyAsync(x =>
            x.UserId == userId && !x.IsDeleted && x.UserRole == Convert.ToString(EnumUserRole.Admin)
        );
    }

    private async Task<bool> IsAdsPageValid(string AdsPageId) =>
        await _context.TblAdsPages.AnyAsync(x => x.AdsPageId == AdsPageId && !x.IsDeleted);

    private async Task<List<TblAdsPagePlacement>> GetAdsPagePlacementsByAdsId(string adsId) =>
        await _context.TblAdsPagePlacements.Where(x => x.AdsId == adsId).ToListAsync();

    private async Task<bool> AdsPageValid(string AdsPageId) =>
        await _context.TblAdsPages.AnyAsync(x => x.AdsPageId == AdsPageId && !x.IsDeleted);

    private async Task<TblImage?> GetImageById(string imageId) =>
        await _context.TblImages.FirstOrDefaultAsync(x => x.ImageId == imageId);

    private async Task<TblAd?> GetActiveAdsById(string adsId) =>
        await _context.TblAds.FirstOrDefaultAsync(x => x.AdsId == adsId && !x.IsDeleted);

    private Result<AdsResponseModel> GetAdsNotFoundResult() =>
        Result<AdsResponseModel>.FailureResult(MessageResource.NotFound, EnumStatusCode.NotFound);

    private Result<AdsResponseModel> GetUserNotFoundResult() =>
        Result<AdsResponseModel>.FailureResult("User Not Found.", EnumStatusCode.NotFound);

    private Result<AdsResponseModel> GetImageNotFoundResult() =>
        Result<AdsResponseModel>.FailureResult("Image Not Found.", EnumStatusCode.NotFound);

    private List<AdsImageModel> GetAdsImageModels(List<TblImage> imgLstByAds) =>
        imgLstByAds
            .Select(x => new AdsImageModel
            {
                AdsId = x.AdsId!,
                ImageId = x.ImageId,
                ImageName = x.ImageName
            })
            .ToList();

    private List<AdsPagePlacementModel> GetAdsPagePlacementModels(
        List<TblAdsPagePlacement> tblAdsPagePlacements
    ) => tblAdsPagePlacements.Select(x => x.Map()).ToList();

    private AdsDataModel GetAdsDataModel(
        AdsModel item,
        List<TblImage> imgLstByAds,
        List<TblAdsPagePlacement> tblAdsPagePlacements
    ) =>
        new()
        {
            Ads = item,
            Images = GetAdsImageModels(imgLstByAds),
            AdsPagePlacements = GetAdsPagePlacementModels(tblAdsPagePlacements)
        };

    private PageSettingModel GetPageSettingModel(
        int pageNo,
        int pageSize,
        int pageCount,
        int totalCount
    ) => new(pageNo, pageSize, pageCount, totalCount);

    private async Task<List<TblImage>> GetImagesByAdsId(string adsId)
    {
        return await _context.TblImages.Where(x => x.AdsId == adsId).ToListAsync();
    }
}
