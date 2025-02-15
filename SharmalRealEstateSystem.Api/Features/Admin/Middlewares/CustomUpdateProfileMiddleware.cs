namespace SharmalRealEstateSystem.Api.Features.Admin.Middlewares;

public class CustomUpdateProfileMiddleware
{
    private readonly UpdateProfileValidator _updateProfileValidator;

    public CustomUpdateProfileMiddleware(UpdateProfileValidator updateProfileValidator)
    {
        _updateProfileValidator = updateProfileValidator;
    }

    #region Handle Update Profile

    public async Task HandleUpdateProfile(RequestDelegate next, HttpContext context)
    {
        string jsonStr = await context.ReadRequestBodyAsync();
        var requestModel = jsonStr.DeserializeObject<UpdateProfileRequestModel>();

        ValidationResult validationResult = await _updateProfileValidator.ValidateAsync(
            requestModel
        );
        if (!validationResult.IsValid)
        {
            string errors = string.Join(" ", validationResult.Errors.Select(x => x.ErrorMessage));
            var responseModel = Result<AuthResponseModel>.FailureResult(errors);

            await context.Response.WriteAsync(responseModel.SerializeObject());
            return;
        }

        await next(context);
    }

    #endregion
}
