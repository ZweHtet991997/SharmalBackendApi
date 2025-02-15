namespace SharmalRealEstateSystem.Api.Features.Admin.Middlewares;

public class CustomAdsPageMiddleware
{
    private readonly AdsPageValidator _adsPageValidator;

    public CustomAdsPageMiddleware(AdsPageValidator adsPageValidator)
    {
        _adsPageValidator = adsPageValidator;
    }

    #region Handle Ads Page

    public async Task HandleAdsPage(RequestDelegate next, HttpContext context)
    {
        string jsonStr = await context.ReadRequestBodyAsync();
        var requestModel = jsonStr.DeserializeObject<AdsPageRequestModel>();

        ValidationResult validationResult = await _adsPageValidator.ValidateAsync(requestModel);
        if (!validationResult.IsValid)
        {
            string errors = string.Join(" ", validationResult.Errors.Select(x => x.ErrorMessage));
            var responseModel = Result<AdsPageResponseModel>.FailureResult(errors);

            await context.Response.WriteAsync(responseModel.SerializeObject());
            return;
        }

        await next(context);
    }

    #endregion
}
