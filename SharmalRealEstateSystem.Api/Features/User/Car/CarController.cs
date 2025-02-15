namespace SharmalRealEstateSystem.Api.Features.User.Car;

[Route("api/v1/user-car")]
[ApiController]
public class CarController : BaseController
{
    private readonly BL_Car _bL_Car;

    public CarController(BL_Car bL_Car)
    {
        _bL_Car = bL_Car;
    }

    //[HttpPost("filter")]
    //public async Task<IActionResult> GetCarList([FromBody] GetCarListRequestModel requestModel)
    //{
    //    var result = await _bL_Car.GetCarList(requestModel);
    //    return Content(result);
    //}
}
