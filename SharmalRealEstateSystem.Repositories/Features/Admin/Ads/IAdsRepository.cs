namespace SharmalRealEstateSystem.Repositories.Features.Admin.Ads;

public interface IAdsRepository
{
    Task<Result<AdsListResponseModel>> GetAdsList(FilterAdsRequestModel filterAdsRequestModel);
    Task<Result<AdsListResponseModel>> GetAdsListByUserId(string userId, int pageNo, int pageSize);
    Task<Result<AdsResponseModel>> CreateAds(AdsRequestModel requestModel);
    Task<Result<AdsResponseModel>> CreateAdsV1(AdsRequestModel requestModel);
    Task<Result<AdsResponseModel>> UpdateAds(UpdateAdsRequestModel requestModel, string adsId);

    //Task<Result<AdsResponseModel>> PatchAds(string adsId);
    Task<Result<AdsResponseModel>> DeleteAds(string adsId);
}
