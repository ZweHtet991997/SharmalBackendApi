namespace SharmalRealEstateSystem.Api.Features.Admin.Middlewares;

public class CustomAdsMiddleware
{
    private readonly AdsValidator _adsValidator;
    private readonly UpdateAdsValidator _updateAdsValidator;

    public CustomAdsMiddleware(AdsValidator adsValidator, UpdateAdsValidator updateAdsValidator)
    {
        _adsValidator = adsValidator;
        _updateAdsValidator = updateAdsValidator;
    }

    #region Handle Ads

    public async Task HandleAds(RequestDelegate next, HttpContext context)
    {
        var formDataDictionary = await context.ReadFormDataAsync();
        var requestModel = GetAdsRequestModel(formDataDictionary);

        ValidationResult validationResult = await _adsValidator.ValidateAsync(requestModel);
        if (!validationResult.IsValid)
        {
            string errors = string.Join(" ", validationResult.Errors.Select(e => e.ErrorMessage));
            var responseModel = Result<AdsResponseModel>.FailureResult(errors);

            await context.Response.WriteAsync(responseModel.SerializeObject());
            return;
        }

        await next(context);
    }

    #endregion

    #region Handle Update Ads

    public async Task HandleUpdateAds(RequestDelegate next, HttpContext context)
    {
        var formDataDictionary = await context.ReadFormDataAsync();
        var requestModel = GetUpdateAdsRequestModel(formDataDictionary);

        ValidationResult validationResult = await _updateAdsValidator.ValidateAsync(requestModel);
        if (!validationResult.IsValid)
        {
            string errors = string.Join(" ", validationResult.Errors.Select(e => e.ErrorMessage));
            var responseModel = Result<AdsResponseModel>.FailureResult(errors);

            await context.Response.WriteAsync(responseModel.SerializeObject());
            return;
        }

        await next(context);
    }

    #endregion

    #region Get Ads Request Model

    private AdsRequestModel GetAdsRequestModel(Dictionary<string, string> formDataDictionary)
    {
        return new AdsRequestModel
        {
            Title = formDataDictionary.TryGetValue("Title", out var title) ? title : string.Empty,
            TargetUrl = formDataDictionary.TryGetValue("TargetUrl", out var targetUrl)
                ? targetUrl
                : string.Empty,
            AdsLayout = formDataDictionary.TryGetValue("AdsLayout", out var adsLayout)
                ? adsLayout
                : string.Empty,
            StartDate = formDataDictionary.TryGetValue("StartDate", out var startDate)
                ? startDate
                : string.Empty,
            EndDate = formDataDictionary.TryGetValue("EndDate", out var endDate)
                ? endDate
                : string.Empty,
            CreatedBy = formDataDictionary.TryGetValue("CreatedBy", out var createdBy)
                ? createdBy
                : string.Empty,
        };
    }

    #endregion

    #region Get Update Ads Request Model

    private UpdateAdsRequestModel GetUpdateAdsRequestModel(
        Dictionary<string, string> formDataDictionary
    )
    {
        return new UpdateAdsRequestModel
        {
            Title = formDataDictionary.TryGetValue("Title", out var title) ? title : string.Empty,
            TargetUrl = formDataDictionary.TryGetValue("TargetUrl", out var targetUrl)
                ? targetUrl
                : string.Empty,
            AdsLayout = formDataDictionary.TryGetValue("AdsLayout", out var adsLayout)
                ? adsLayout
                : string.Empty,
            StartDate = formDataDictionary.TryGetValue("StartDate", out var startDate)
                ? startDate
                : string.Empty,
            EndDate = formDataDictionary.TryGetValue("EndDate", out var endDate)
                ? endDate
                : string.Empty,
            UpdatedBy = formDataDictionary.TryGetValue("UpdatedBy", out var updatedBy)
                ? updatedBy
                : string.Empty,
        };
    }

    #endregion
}
