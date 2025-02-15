namespace SharmalRealEstateSystem.Api.Features.User.Property;

[Route("api/v1/user-property")]
[ApiController]
public class PropertyController : BaseController
{
    private readonly BL_Property _bL_Property;

    public PropertyController(BL_Property bL_Property)
    {
        _bL_Property = bL_Property;
    }

    //[HttpPost("filter")]
    //public async Task<IActionResult> GetPropertyList(
    //    [FromBody] GetPropertyListRequestModel requestModel
    //)
    //{
    //    var result = await _bL_Property.GetList(requestModel);
    //    return Content(result);
    //}
}
