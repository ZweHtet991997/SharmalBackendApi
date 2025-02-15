namespace SharmalRealEstateSystem.Api.Features.Admin.Ads;

public class BL_Ads
{
    #region Initializations

    private readonly IAdminUnitOfWork _adminUnitOfWork;

    public BL_Ads(IAdminUnitOfWork adminUnitOfWork)
    {
        _adminUnitOfWork = adminUnitOfWork;
    }

    #endregion

    #region Get Ads List

    public async Task<Result<AdsListResponseModel>> GetAdsList(
        FilterAdsRequestModel filterAdsRequestModel
    )
    {
        Result<AdsListResponseModel> responseModel;
        try
        {
            if (filterAdsRequestModel.PageNo <= 0)
            {
                responseModel = GetInvalidPageNoResult();
                goto result;
            }

            if (filterAdsRequestModel.PageSize <= 0)
            {
                responseModel = GetInvalidPageSizeResult();
                goto result;
            }

            if (!filterAdsRequestModel.Status!.IsNullOrEmpty())
            {
                if (!Enum.IsDefined(typeof(EnumAdsFilterStatus), filterAdsRequestModel.Status!))
                {
                    responseModel = Result<AdsListResponseModel>.FailureResult("Invalid Status.");
                    goto result;
                }
            }

            responseModel = await _adminUnitOfWork.AdsRepository.GetAdsList(filterAdsRequestModel);
        }
        catch (Exception ex)
        {
            responseModel = Result<AdsListResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Get Ads Lis By User Id

    public async Task<Result<AdsListResponseModel>> GetAdsListByUserId(string userId, int pageNo, int pageSize)
    {
        Result<AdsListResponseModel> responseModel;
        try
        {
            if (userId.IsNullOrEmpty())
            {
                responseModel = Result<AdsListResponseModel>.FailureResult(MessageResource.InvalidId);
                goto result;
            }

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

            responseModel = await _adminUnitOfWork.AdsRepository.GetAdsListByUserId(userId, pageNo, pageSize);
        }
        catch (Exception ex)
        {
            responseModel = Result<AdsListResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Create Ads

    public async Task<Result<AdsResponseModel>> CreateAds(AdsRequestModel requestModel)
    {
        Result<AdsResponseModel> responseModel;
        try
        {
            if (requestModel.Files is null || requestModel.Files.Count == 0)
            {
                responseModel = GetInvalidFileResult();
                goto result;
            }

            if (requestModel.Files is null || requestModel.Files.Count > 5)
            {
                responseModel = GetMaximumFilesExceedResult();
                goto result;
            }

            if (requestModel.AdsPagePlacements is null || requestModel.AdsPagePlacements.Count <= 0)
            {
                responseModel = GetInvalidAdsPagePlacementsResult();
                goto result;
            }

            if (!AdsLayout.GetAdsLayout().Any(x => x.Equals(requestModel.AdsLayout)))
            {
                responseModel = GetInvalidAdsLayoutResult();
                goto result;
            }

            responseModel = await _adminUnitOfWork.AdsRepository.CreateAdsV1(requestModel);
        }
        catch (Exception ex)
        {
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
            if (requestModel.AdsPagePlacements is null || requestModel.AdsPagePlacements.Count <= 0)
            {
                responseModel = GetInvalidAdsPagePlacementsResult();
                goto result;
            }

            if (adsId.IsNullOrEmpty())
            {
                responseModel = GetInvalidIdResult();
                goto result;
            }

            responseModel = await _adminUnitOfWork.AdsRepository.UpdateAds(requestModel, adsId);
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
    //        if (adsId.IsNullOrEmpty())
    //        {
    //            responseModel = GetInvalidIdResult();
    //            goto result;
    //        }

    //        responseModel = await _adminUnitOfWork.AdsRepository.PatchAds(adsId);
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
            if (adsId.IsNullOrEmpty())
            {
                responseModel = GetInvalidIdResult();
                goto result;
            }

            responseModel = await _adminUnitOfWork.AdsRepository.DeleteAds(adsId);
        }
        catch (Exception ex)
        {
            responseModel = Result<AdsResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    private Result<AdsResponseModel> GetMaximumFilesExceedResult() =>
        Result<AdsResponseModel>.FailureResult("Upload Files must be 5 images maximum.");

    private Result<AdsResponseModel> GetInvalidFileResult() =>
        Result<AdsResponseModel>.FailureResult("Invalid Files");

    private Result<AdsResponseModel> GetInvalidAdsPagePlacementsResult() =>
        Result<AdsResponseModel>.FailureResult("Invalid Placement.");

    private Result<AdsResponseModel> GetInvalidIdResult() =>
        Result<AdsResponseModel>.FailureResult(MessageResource.InvalidId);

    private Result<AdsResponseModel> GetInvalidAdsLayoutResult() =>
        Result<AdsResponseModel>.FailureResult("Invalid Ads Layout");

    private Result<AdsListResponseModel> GetInvalidPageNoResult() =>
        Result<AdsListResponseModel>.FailureResult("Page No is invalid.");

    private Result<AdsListResponseModel> GetInvalidPageSizeResult() =>
        Result<AdsListResponseModel>.FailureResult("Page Size invalid.");
}
