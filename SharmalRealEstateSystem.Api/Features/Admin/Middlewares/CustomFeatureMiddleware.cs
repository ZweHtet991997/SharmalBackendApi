namespace SharmalRealEstateSystem.Api.Features.Admin.Middlewares;

public class CustomFeatureMiddleware
{
    private readonly FeatureValidator _featureValidator;

    public CustomFeatureMiddleware(FeatureValidator featureValidator)
    {
        _featureValidator = featureValidator;
    }

    #region Handle Feature

    public async Task HandleFeature(RequestDelegate next, HttpContext context)
    {
        string jsonStr = await context.ReadRequestBodyAsync();
        var requestModel = jsonStr.DeserializeObject<FeatureRequestModel>();

        ValidationResult validationResult = await _featureValidator.ValidateAsync(requestModel);
        if (!validationResult.IsValid)
        {
            string errors = string.Join(" ", validationResult.Errors.Select(x => x.ErrorMessage));
            var responseModel = Result<FeatureResponseModel>.FailureResult(errors);

            await context.Response.WriteAsync(responseModel.SerializeObject());
            return;
        }

        await next(context);
    }

    #endregion
}
