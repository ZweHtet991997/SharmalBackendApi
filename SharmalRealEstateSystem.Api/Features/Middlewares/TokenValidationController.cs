namespace SharmalRealEstateSystem.Api.Features.Middlewares;

[Route("api/v1/jwt")]
[ApiController]
public class TokenValidationController : BaseController
{
    private readonly IConfiguration _configuration;
    private readonly TokenValidationService _tokenValidationService;

    public TokenValidationController(
        IConfiguration configuration,
        TokenValidationService tokenValidationService
    )
    {
        _configuration = configuration;
        _tokenValidationService = tokenValidationService;
    }

    [HttpPost]
    public IActionResult ValidateToken([FromBody] JwtRequestModel requestModel)
    {
        Result<string> responseModel;
        try
        {
            ClaimsPrincipal principal = _tokenValidationService.ValidateToken(requestModel.Token!);

            if (principal is not null)
            {
                responseModel = Result<string>.SuccessResult();
                goto result;
            }

            responseModel = Result<string>.FailureResult(
                MessageResource.Unauthorized,
                EnumStatusCode.UnAuthorized
            );
        }
        catch (Exception ex)
        {
            responseModel = Result<string>.FailureResult(
                MessageResource.Unauthorized,
                EnumStatusCode.UnAuthorized
            );
        }

    result:
        return Content(responseModel);
    }
}
