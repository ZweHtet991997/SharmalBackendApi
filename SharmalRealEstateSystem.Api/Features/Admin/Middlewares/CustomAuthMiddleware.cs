namespace SharmalRealEstateSystem.Api.Features.Admin.Middlewares;

public class CustomAuthMiddleware
{
    private readonly LoginValidator _loginValidator;
    private readonly RegisterValidator _registerValidator;
    private readonly AesService _aesService;

    public CustomAuthMiddleware(
        LoginValidator loginValidator,
        RegisterValidator registerValidator,
        AesService aesService
    )
    {
        _loginValidator = loginValidator;
        _registerValidator = registerValidator;
        _aesService = aesService;
    }

    #region Handle Login

    public async Task HandleLogin(HttpContext context, RequestDelegate next)
    {
        Result<AuthResponseModel> responseModel;
        try
        {
            string jsonStr = await context.ReadRequestBodyAsync();
            var requestModel = jsonStr.DeserializeObject<LoginRequestModel>();

            ValidationResult validationResult = await _loginValidator.ValidateAsync(requestModel);
            if (!validationResult.IsValid)
            {
                string errors = string.Join(
                    " ",
                    validationResult.Errors.Select(e => e.ErrorMessage)
                );
                responseModel = Result<AuthResponseModel>.FailureResult(errors);

                await context.Response.WriteAsync(responseModel.SerializeObject());
                return;
            }
            _aesService.DecryptString(requestModel.Password);

            await next(context);
        }
        catch (CryptographicException ex)
        {
            responseModel = Result<AuthResponseModel>.FailureResult(
                MessageResource.LoginFail,
                EnumStatusCode.NotFound
            );
            await context.Response.WriteAsync(responseModel.SerializeObject());
            return;
        }
    }

    #endregion

    #region Handle Register

    public async Task HandleRegister(HttpContext context, RequestDelegate next)
    {
        Result<AuthResponseModel> responseModel;
        try
        {
            string jsonStr = await context.ReadRequestBodyAsync();
            var requestModel = jsonStr.DeserializeObject<RegisterRequestModel>();

            ValidationResult validationResult = await _registerValidator.ValidateAsync(
                requestModel
            );
            if (!validationResult.IsValid)
            {
                string errors = string.Join(
                    " ",
                    validationResult.Errors.Select(e => e.ErrorMessage)
                );
                responseModel = Result<AuthResponseModel>.FailureResult(errors);

                await context.Response.WriteAsync(responseModel.SerializeObject());
                return;
            }
            _aesService.DecryptString(requestModel.Password);

            await next(context);
        }
        catch (CryptographicException ex)
        {
            responseModel = Result<AuthResponseModel>.FailureResult(
                MessageResource.InvalidEncryptionKey,
                EnumStatusCode.MethodNotAllowed
            );
            await context.Response.WriteAsync(responseModel.SerializeObject());
            return;
        }
    }

    #endregion
}
