namespace SharmalRealEstateSystem.Api.Features.Admin.ExchangeRate;

[Route("api/v1/feature-exchange-rate")]
[ApiController]
public class ExchangeRateController : BaseController
{
    #region Initializations

    private readonly BL_ExchangeRate _bL_ExchangeRate;

    public ExchangeRateController(BL_ExchangeRate bL_ExchangeRate)
    {
        _bL_ExchangeRate = bL_ExchangeRate;
    }

    #endregion

    #region Get List

    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var result = await _bL_ExchangeRate.GetExchangeRateList();
        return Content(result);
    }

    #endregion

    #region Update Exchange Rate

    [HttpPut]
    public async Task<IActionResult> UpdateExchangeRate(
        [FromBody] UpdateExchangeRateRequestModel requestModel
    )
    {
        var result = await _bL_ExchangeRate.UpdateExchangeRate(requestModel);
        return Content(result);
    }

    #endregion

    #region Delete Exchange Rate

    [HttpDelete]
    public async Task<IActionResult> DeleteExchangeRate(int id)
    {
        var result = await _bL_ExchangeRate.DeleteExchangeRate(id);
        return Content(result);
    }

    #endregion

    #region Get Db Credentials from env

    //[HttpGet]
    //[Route("/getvalue")]
    //public IActionResult GetValue()
    //{
    //    var DBConnection = Environment.GetEnvironmentVariable("Sharmal_DBConnection");
    //    var Database = Environment.GetEnvironmentVariable("Sharmal_Database");
    //    var UserId = Environment.GetEnvironmentVariable("Sharmal_UserID");
    //    var Password = Environment.GetEnvironmentVariable("Sharmal_Password");
    //    var result =
    //        $"Data Source={DBConnection};Database={Database};User ID={UserId};Password={Password};TrustServerCertificate=True;";
    //    return Ok(result);
    //}

    #endregion
}
