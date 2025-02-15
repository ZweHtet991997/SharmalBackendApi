namespace SharmalRealEstateSystem.Api.Features.Admin.AdsPage;

public class BL_AdsPage
{
    #region Initializations

    private readonly IAdminUnitOfWork _adminUnitOfWork;

    public BL_AdsPage(IAdminUnitOfWork adminUnitOfWork)
    {
        _adminUnitOfWork = adminUnitOfWork;
    }

    #endregion

    #region Get Ads Page List

    public async Task<Result<AdsPageListResponseModel>> GetAdsPageList(int? isDeleted)
    {
        return await _adminUnitOfWork.AdsPageRepository.GetAdsPageList(isDeleted);
    }

    #endregion

    #region Create Ads Page

    public async Task<Result<AdsPageResponseModel>> CreateAdsPage(AdsPageRequestModel requestModel)
    {
        return await _adminUnitOfWork.AdsPageRepository.CreateAdsPage(requestModel);
    }

    #endregion

    #region Update Ads Page

    public async Task<Result<AdsPageResponseModel>> UpdateAdsPage(
        AdsPageRequestModel requestModel,
        string adsPageId
    )
    {
        Result<AdsPageResponseModel> responseModel;

        if (adsPageId.IsNullOrEmpty())
        {
            responseModel = GetInvalidIdResult();
            goto result;
        }

        responseModel = await _adminUnitOfWork.AdsPageRepository.UpdateAdsPage(
            requestModel,
            adsPageId
        );

    result:
        return responseModel;
    }

    #endregion

    #region Reactivate Ads Page

    public async Task<Result<AdsPageResponseModel>> ReactivateAdsPage(string adsPageId)
    {
        Result<AdsPageResponseModel> responseModel;

        if (adsPageId.IsNullOrEmpty())
        {
            responseModel = GetInvalidIdResult();
            goto result;
        }

        responseModel = await _adminUnitOfWork.AdsPageRepository.ReactivateAdsPage(adsPageId);

    result:
        return responseModel;
    }

    #endregion

    #region Delete Ads Page

    public async Task<Result<AdsPageResponseModel>> DeleteAdsPage(string adsPageId)
    {
        Result<AdsPageResponseModel> responseModel;

        if (adsPageId.IsNullOrEmpty())
        {
            responseModel = GetInvalidIdResult();
            goto result;
        }

        responseModel = await _adminUnitOfWork.AdsPageRepository.DeleteAdsPage(adsPageId);

    result:
        return responseModel;
    }

    #endregion

    private Result<AdsPageResponseModel> GetInvalidIdResult() =>
        Result<AdsPageResponseModel>.FailureResult(MessageResource.InvalidId);
}
