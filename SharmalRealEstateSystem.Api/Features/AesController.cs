namespace SharmalRealEstateSystem.Api.Features;

[Route("api/[controller]")]
[ApiController]
public class AesController : BaseController
{
    private readonly AesService _aesService;

    public AesController(AesService aesService)
    {
        _aesService = aesService;
    }

    [HttpGet("Encrypt")]
    public IActionResult Encrypt(string plainText)
    {
        Result<string> responseModel;
        try
        {
            string encrypted = _aesService.EncryptString(plainText);
            responseModel = Result<string>.SuccessResult(data: encrypted);
        }
        catch (Exception ex)
        {
            responseModel = Result<string>.FailureResult(ex);
        }

        return Content(responseModel);
    }

    [HttpGet("Decrypt")]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Decrypt(string encryptedText)
    {
        Result<string> responseModel;
        try
        {
            string decrypted = _aesService.DecryptString(encryptedText);
            responseModel = Result<string>.SuccessResult(data: decrypted);
        }
        catch (Exception ex)
        {
            responseModel = Result<string>.FailureResult(ex);
        }

        return Content(responseModel);
    }
}
