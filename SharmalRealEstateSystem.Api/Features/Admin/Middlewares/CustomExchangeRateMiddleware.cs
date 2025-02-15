namespace SharmalRealEstateSystem.Api.Features.Admin.Middlewares;

public class CustomExchangeRateMiddleware
{
    private readonly ExchangeRateValidator _exchangeRateValidator;

    public CustomExchangeRateMiddleware(ExchangeRateValidator exchangeRateValidator)
    {
        _exchangeRateValidator = exchangeRateValidator;
    }

    #region Handle Exchange Rate

    public async Task HandleExchangeRate(HttpContext context, RequestDelegate next)
    {
        string jsonStr = await context.ReadRequestBodyAsync();
        var requestModel = jsonStr.DeserializeObject<ExchangeRateRequestModel>();

        ValidationResult validationResult = await _exchangeRateValidator.ValidateAsync(
            requestModel
        );
        if (!validationResult.IsValid)
        {
            string errors = string.Join(" ", validationResult.Errors.Select(x => x.ErrorMessage));
            var responseModel = Result<ExchangeRateResponseModel>.FailureResult(errors);

            await context.Response.WriteAsync(responseModel.SerializeObject());
            return;
        }

        await next(context);
    }

    #endregion
}
