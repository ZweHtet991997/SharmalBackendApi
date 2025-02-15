namespace SharmalRealEstateSystem.Api.Features.Admin.Feature;

[Route("api/v1")]
[ApiController]
public class FeatureController : BaseController
{
    #region Initializations

    private readonly BL_Feature _bLFeature;

    public FeatureController(BL_Feature bLFeature)
    {
        _bLFeature = bLFeature;
    }

    #endregion

    #region Get Features

    [HttpGet]
    [Route("feature-feature")]
    public async Task<IActionResult> GetFeatures(int? isDeleted)
    {
        var result = await _bLFeature.GetFeatureList(isDeleted);
        return Content(result);
    }

    #endregion

    #region Create Feature

    [HttpPost]
    [Route("admin-feature")]
    public async Task<IActionResult> CreateFeature([FromBody] FeatureRequestModel requestModel)
    {
        var result = await _bLFeature.CreateFeature(requestModel);
        return Content(result);
    }

    #endregion

    #region Update Feature

    [HttpPut]
    [Route("admin-feature")]
    public async Task<IActionResult> UpdateFeature(
        [FromBody] FeatureRequestModel requestModel,
        string id
    )
    {
        var result = await _bLFeature.UpdateFeature(requestModel, id);
        return Content(result);
    }

    #endregion

    #region Reactivate Feature

    [HttpPatch]
    [Route("admin-feature")]
    public async Task<IActionResult> ReactivateFeature(string id)
    {
        var result = await _bLFeature.ReactivateFeature(id);
        return Content(result);
    }

    #endregion

    #region Delete Feature

    [HttpDelete]
    [Route("admin-feature")]
    public async Task<IActionResult> DeleteFeature(string id)
    {
        var result = await _bLFeature.DeleteFeature(id);
        return Content(result);
    }

    #endregion
}
