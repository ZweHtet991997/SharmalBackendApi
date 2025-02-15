namespace SharmalRealEstateSystem.Api.Features.Middlewares;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private readonly TokenValidationService _tokenValidationService;
    private readonly AesService _aesService;

    public AuthenticationMiddleware(
        RequestDelegate next,
        IConfiguration configuration,
        TokenValidationService tokenValidationService,
        AesService aesService
    )
    {
        _next = next;
        _configuration = configuration;
        _tokenValidationService = tokenValidationService;
        _aesService = aesService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Result<string> responseModel;
        try
        {
            string? authHeader = context.Request.Headers["Authorization"];
            string requestPath = context.Request.Path;

            #region Should Pass Case

            if (ShouldPass(requestPath, context.Request.Method))
            {
                await _next.Invoke(context);
            }

            #endregion

            else
            {
                if (authHeader != null && authHeader.StartsWith("Bearer"))
                {
                    string[] header_and_token = authHeader.Split(' ');
                    string header = header_and_token[0];
                    string token = header_and_token[1];
                    ClaimsPrincipal principal = _tokenValidationService.ValidateToken(token);

                    if (principal is not null)
                    {
                        var userRoleClaim = principal.FindFirst(ClaimTypes.Role)!.Value;
                        string decryptedUserRole = _aesService.DecryptString(userRoleClaim);

                        #region Admin Access Control

                        if (decryptedUserRole.Equals(Convert.ToString(EnumUserRole.Admin)))
                        {
                            var adminEndpoints = AdminEndpoints.GetAdminEndpoints();
                            if (!adminEndpoints.Any(endpoint => endpoint.Equals(requestPath)))
                            {
                                responseModel = GetUnAuthorizedResult();
                                await context.Response.WriteAsync(responseModel.SerializeObject());
                                return;
                            }

                            await _next.Invoke(context);
                            return;
                        }

                        #endregion

                        #region User Access Control

                        if (decryptedUserRole.Equals(Convert.ToString(EnumUserRole.User)))
                        {
                            var userEndpoints = GetUserEndpoints();
                            if (!userEndpoints.Any(endpoint => endpoint.Equals(requestPath)))
                            {
                                responseModel = GetUnAuthorizedResult();
                                await context.Response.WriteAsync(responseModel.SerializeObject());
                                return;
                            }

                            await _next.Invoke(context);
                            return;
                        }

                        #endregion

                        #region Staff Access Control

                        if (decryptedUserRole.Equals(EnumUserRole.Staff.ToString()))
                        {
                            var staffEndpoints = StaffEndpoints.GetStaffEndpoints();
                            if (!staffEndpoints.Any(endpoint => endpoint.Equals(requestPath)))
                            {
                                responseModel = GetUnAuthorizedResult();
                                await context.Response.WriteAsync(responseModel.SerializeObject());
                                return;
                            }

                            await _next.Invoke(context);
                            return;
                        }

                        #endregion

                        responseModel = GetUnAuthorizedResult();
                        await context.Response.WriteAsync(responseModel.SerializeObject());
                        return;
                    }
                    #region Invalid Principal

                    else
                    {
                        responseModel = GetUnAuthorizedResult();
                        await context.Response.WriteAsync(responseModel.SerializeObject());
                        return;
                    }

                    #endregion
                }
                #region AuthHeader null or not Bearer

                else
                {
                    responseModel = GetUnAuthorizedResult();
                    await context.Response.WriteAsync(responseModel.SerializeObject());
                    return;
                }

                #endregion
            }
        }
        catch (Exception ex)
        {
            responseModel = GetUnAuthorizedResult();
            await context.Response.WriteAsync(responseModel.SerializeObject());
        }
    }

    private Result<string> GetUnAuthorizedResult() =>
        Result<string>.FailureResult(MessageResource.Unauthorized, EnumStatusCode.UnAuthorized);

    private bool ShouldPass(string requestPath, string httpMethod)
    {
        return requestPath == "/api/Aes/Encrypt"
            || requestPath == "/api/Aes/Decrypt"
            || requestPath == "/api/v1/feature-property/filter"
            || requestPath == "/api/v1/feature-property/property-id"
            || requestPath == "/api/v1/feature-car/filter"
            || requestPath == "/api/v1/feature-car/car-id"
            || HasToPassExchangeRateGetList(requestPath, httpMethod)
            || HasToPassCreateInquiry(requestPath, httpMethod)
            //|| requestPath == AdminEndpoints.Login
            || requestPath == AdminEndpoints.Register
            || requestPath == AdminEndpoints.ResetFailCount
            || requestPath == "/api/v1/feature-account/login"
            || requestPath == "/weatherforecast"
            || requestPath == "/favicon.ico"
            || requestPath.Contains(".png")
            || requestPath.Contains(".jpeg")
            || requestPath.Contains(".jpg");
    }

    private bool HasToPassExchangeRateGetList(string requestPath, string httpMethod)
    {
        return requestPath == "/api/v1/feature-exchange-rate" && httpMethod == "GET";
    }

    private bool HasToPassCreateInquiry(string requestPath, string httpMethod)
    {
        return requestPath == "/api/v1/feature-inquiry" && httpMethod == EnumHttpMethod.POST.ToString();
    }

    private List<string> GetAdminEndpoints() =>
        new()
        {
            AdminEndpoints.UserList.ToLower(), // same as delete
            AdminEndpoints.Register.ToLower(),
            AdminEndpoints.UpdateProfile.ToLower(),
            AdminEndpoints.UpdatePassword.ToLower(),
            //AdminEndpoints.Login.ToLower(),
            AdminEndpoints.ExchangeRate.ToLower(),
            AdminEndpoints.Property.ToLower(),
            AdminEndpoints.FilterProperty.ToLower(),
            AdminEndpoints.Feature.ToLower(),
            AdminEndpoints.Inquiry.ToLower(),
            AdminEndpoints.FilterInquiry.ToLower(),
            AdminEndpoints.AdsPage.ToLower(),
            AdminEndpoints.Ads.ToLower(),
            AdminEndpoints.Car.ToLower(),
            AdminEndpoints.FilterCar.ToLower(),
        };

    private List<string> GetUserEndpoints() =>
        new() { UserEndpoints.Property.ToLower(), UserEndpoints.Car.ToLower() };

    private List<string> GetStaffEndpoints() =>
        new() { StaffEndpoints.UpdateUser, StaffEndpoints.UpdatePassword };
}
