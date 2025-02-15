namespace SharmalRealEstateSystem.Api.Features.Admin.Feature;

public class BL_Feature
{
    private readonly IAdminUnitOfWork _adminUnitOfWork;

    public BL_Feature(IAdminUnitOfWork adminUnitOfWork)
    {
        _adminUnitOfWork = adminUnitOfWork;
    }

    #region Get Feature List

    public async Task<Result<FeatureListResponseModel>> GetFeatureList(int? isDeleted)
    {
        return await _adminUnitOfWork.FeatureRepository.GetFeatureList(isDeleted);
    }

    #endregion

    #region Create Feature

    public async Task<Result<FeatureResponseModel>> CreateFeature(FeatureRequestModel requestModel)
    {
        return await _adminUnitOfWork.FeatureRepository.CreateFeature(requestModel);
    }

    #endregion

    #region Update Feature

    public async Task<Result<FeatureResponseModel>> UpdateFeature(
        FeatureRequestModel requestModel,
        string featureId
    )
    {
        Result<FeatureResponseModel> responseModel;

        if (featureId.IsNullOrEmpty())
        {
            responseModel = GetInvalidIdResult();
            goto result;
        }

        responseModel = await _adminUnitOfWork.FeatureRepository.UpdateFeatureV1(
            requestModel,
            featureId
        );

    result:
        return responseModel;
    }

    #endregion

    #region Delete Feature

    public async Task<Result<FeatureResponseModel>> DeleteFeature(string featureId)
    {
        Result<FeatureResponseModel> responseModel;

        if (featureId.IsNullOrEmpty())
        {
            responseModel = GetInvalidIdResult();
            goto result;
        }

        responseModel = await _adminUnitOfWork.FeatureRepository.DeleteFeature(featureId);

    result:
        return responseModel;
    }

    #endregion

    #region Reactivate Feature

    public async Task<Result<FeatureResponseModel>> ReactivateFeature(string featureId)
    {
        Result<FeatureResponseModel> responseModel;
        try
        {
            if (featureId.IsNullOrEmpty())
            {
                responseModel = GetInvalidIdResult();
                goto result;
            }

            responseModel = await _adminUnitOfWork.FeatureRepository.ReactivateFeature(featureId);
        }
        catch (Exception ex)
        {
            responseModel = Result<FeatureResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    private Result<FeatureResponseModel> GetInvalidIdResult() =>
        Result<FeatureResponseModel>.FailureResult(MessageResource.InvalidId);
}
