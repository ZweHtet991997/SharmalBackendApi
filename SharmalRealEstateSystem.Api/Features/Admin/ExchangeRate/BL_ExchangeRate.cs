namespace SharmalRealEstateSystem.Api.Features.Admin.ExchangeRate;

public class BL_ExchangeRate
{
    #region Initializations

    private readonly IAdminUnitOfWork _adminUnitOfWork;

    public BL_ExchangeRate(IAdminUnitOfWork adminUnitOfWork)
    {
        _adminUnitOfWork = adminUnitOfWork;
    }

    #endregion

    #region Get Exchange Rate List

    public async Task<Result<ExchangeRateListResponseModel>> GetExchangeRateList()
    {
        return await _adminUnitOfWork.ExchangeRateRepository.GetExchangeRateList();
    }

    #endregion

    #region Update Exchange Rate

    public async Task<Result<ExchangeRateResponseModel>> UpdateExchangeRate(
        UpdateExchangeRateRequestModel requestModel
    )
    {
        Result<ExchangeRateResponseModel> responseModel;
        try
        {
            if (IsUpdateExchangeRateInvalid(requestModel))
            {
                responseModel = Result<ExchangeRateResponseModel>.FailureResult(
                    "Invalid Exchange Rate List."
                );
                goto result;
            }

            responseModel = await _adminUnitOfWork.ExchangeRateRepository.UpdateExchangeRate(
                requestModel
            );
        }
        catch (Exception ex)
        {
            responseModel = Result<ExchangeRateResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Delete Exchange Rate

    public async Task<Result<ExchangeRateResponseModel>> DeleteExchangeRate(int id)
    {
        Result<ExchangeRateResponseModel> responseModel;
        try
        {
            if (id <= 0)
            {
                responseModel = GetInvalidIdResult();
                goto result;
            }

            responseModel = await _adminUnitOfWork.ExchangeRateRepository.DeleteExchangeRate(id);
        }
        catch (Exception ex)
        {
            responseModel = Result<ExchangeRateResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    private bool IsUpdateExchangeRateInvalid(UpdateExchangeRateRequestModel requestModel) =>
        requestModel is null
        || requestModel.ExchangeRates is null
        || requestModel.ExchangeRates!.Count <= 0;

    private Result<ExchangeRateResponseModel> GetInvalidIdResult() =>
        Result<ExchangeRateResponseModel>.FailureResult(MessageResource.InvalidId);
}
