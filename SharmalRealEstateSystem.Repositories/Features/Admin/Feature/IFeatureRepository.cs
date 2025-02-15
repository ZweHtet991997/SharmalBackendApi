namespace SharmalRealEstateSystem.Repositories.Features.Admin.Feature;

public interface IFeatureRepository
{
    Task<Result<FeatureListResponseModel>> GetFeatureList(int? isDeleted);
    Task<Result<FeatureResponseModel>> CreateFeature(FeatureRequestModel requestModel);
    Task<Result<FeatureResponseModel>> UpdateFeature(
        FeatureRequestModel requestModel,
        string featureId
    );
    Task<Result<FeatureResponseModel>> UpdateFeatureV1(
        FeatureRequestModel requestModel,
        string featureId
    );
    Task<Result<FeatureResponseModel>> DeleteFeature(string featureId);
    Task<Result<FeatureResponseModel>> ReactivateFeature(string featureId);
}
