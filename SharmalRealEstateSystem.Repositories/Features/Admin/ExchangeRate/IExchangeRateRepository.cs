namespace SharmalRealEstateSystem.Repositories.Features.Admin.ExchangeRate;

public interface IExchangeRateRepository
{
    Task<Result<ExchangeRateListResponseModel>> GetExchangeRateList();
    Task<Result<ExchangeRateResponseModel>> UpdateExchangeRate(
        UpdateExchangeRateRequestModel requestModel
    );
    Task<Result<ExchangeRateResponseModel>> DeleteExchangeRate(int id);
}
