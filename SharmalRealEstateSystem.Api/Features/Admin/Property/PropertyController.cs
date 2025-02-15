namespace SharmalRealEstateSystem.Api.Features.Admin.Property;

[Route("api/v1/feature-property")]
[ApiController]
public class PropertyController : BaseController
{
    #region Initializations

    private readonly BL_Property _bL_Property;

    public PropertyController(BL_Property bL_Property)
    {
        _bL_Property = bL_Property;
    }

    #endregion

    #region Get List

    [HttpPost("filter")]
    public async Task<IActionResult> GetList(GetPropertyListRequestModel requestModel)
    {
        var result = await _bL_Property.GetList(requestModel);
        return Content(result);
    }

    #endregion

    [HttpGet]
    public async Task<IActionResult> GetListByUserId(string userId, int pageNo, int pageSize)
    {
        var result = await _bL_Property.GetListByUserId(userId, pageNo, pageSize);
        return Content(result);
    }

    [HttpGet("property-id")]
    public async Task<IActionResult> GetPropertyByPropertyId(string propertyId)
    {
        var result = await _bL_Property.GetPropertyByPropertyId(propertyId);
        return Content(result);
    }

    #region Create Property

    [HttpPost]
    public async Task<IActionResult> CreateProperty([FromForm] PropertyRequestModel requestModel)
    {
        var result = await _bL_Property.CreateProperty(requestModel);
        return Content(result);
    }

    #endregion

    #region Update Property

    [HttpPut]
    public async Task<IActionResult> UpdateProperty(
        [FromForm] UpdatePropertyRequestModel requestModel,
        string id
    )
    {
        var result = await _bL_Property.UpdateProperty(requestModel, id);
        return Content(result);
    }

    #endregion

    #region Delete Property

    [HttpDelete]
    public async Task<IActionResult> DeleteProperty(string id)
    {
        var result = await _bL_Property.DeleteProperty(id);
        return Content(result);
    }

    #endregion
}
