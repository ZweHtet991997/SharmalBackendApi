namespace SharmalRealEstateSystem.Repositories.Features.Admin.AdsPage;

public interface IAdsPageRepository
{
    Task<Result<AdsPageListResponseModel>> GetAdsPageList(int? isDeleted);
    Task<Result<AdsPageResponseModel>> CreateAdsPage(AdsPageRequestModel requestModel);
    Task<Result<AdsPageResponseModel>> UpdateAdsPage(
        AdsPageRequestModel requestModel,
        string adsPageId
    );
    Task<Result<AdsPageResponseModel>> DeleteAdsPage(string adsPageId);
    Task<Result<AdsPageResponseModel>> ReactivateAdsPage(string adsPageId);
}
