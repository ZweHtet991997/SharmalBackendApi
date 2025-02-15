namespace SharmalRealEstateSystem.Api.Features.Admin.Middlewares;

public class CustomInquiryMiddleware
{
    private readonly InquiryValidator _inquiryValidator;

    public CustomInquiryMiddleware(InquiryValidator inquiryValidator)
    {
        _inquiryValidator = inquiryValidator;
    }

    #region Handle Inquiry

    public async Task HandleInquiry(RequestDelegate next, HttpContext context)
    {
        string jsonStr = await context.ReadRequestBodyAsync();
        var requestModel = jsonStr.DeserializeObject<InquiryRequestModel>();

        ValidationResult validationResult = await _inquiryValidator.ValidateAsync(requestModel);
        if (!validationResult.IsValid)
        {
            string errors = string.Join(" ", validationResult.Errors.Select(x => x.ErrorMessage));
            var responseModel = Result<InquiryResponseModel>.FailureResult(errors);

            await context.Response.WriteAsync(responseModel.SerializeObject());
            return;
        }

        await next(context);
    }

    #endregion
}
