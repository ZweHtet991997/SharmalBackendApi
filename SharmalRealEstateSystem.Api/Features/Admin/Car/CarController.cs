namespace SharmalRealEstateSystem.Api.Features.Admin.Car;

[Route("api/v1/feature-car")]
[ApiController]
public class CarController : BaseController
{
    #region Initializations

    private readonly BL_Car _bL_Car;

    public CarController(BL_Car bL_Car)
    {
        _bL_Car = bL_Car;
    }

    #endregion

    #region Get Car List

    [HttpPost("filter")]
    public async Task<IActionResult> GetCarList(GetCarListRequestModel requestModel)
    {
        var result = await _bL_Car.GetCarList(requestModel);
        return Content(result);
    }

    #endregion

    #region Get Car By Car Id

    [HttpGet("car-id")]
    public async Task<IActionResult> GetCarByCarId(string carId)
    {
        var result = await _bL_Car.GetCarByCarId(carId);
        return Content(result);

    }
    #endregion

    #region Get Car List By User Id

    [HttpGet]
    public async Task<IActionResult> GetCarListByUserId(string userId, int pageNo, int pageSize)
    {
        var result = await _bL_Car.GetCarListByUserId(userId, pageNo, pageSize);
        return Content(result);
    }

    #endregion

    #region Create Car

    [HttpPost]
    public async Task<IActionResult> CreateCar([FromForm] CarRequestModel requestModel)
    {
        var result = await _bL_Car.CreateCar(requestModel);
        return Content(result);
    }

    #endregion

    #region Update Car

    [HttpPut]
    public async Task<IActionResult> UpdateCar(
        [FromForm] UpdateCarRequestModel requestModel,
        string id
    )
    {
        var result = await _bL_Car.UpdateCar(requestModel, id);
        return Content(result);
    }

    #endregion

    #region Delete Car

    [HttpDelete]
    public async Task<IActionResult> DeleteCar(string id)
    {
        var result = await _bL_Car.DeleteCar(id);
        return Content(result);
    }

    #endregion
}
