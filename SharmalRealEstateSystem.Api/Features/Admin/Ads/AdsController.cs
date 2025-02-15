namespace SharmalRealEstateSystem.Api.Features.Admin.Ads;

[Route("api/v1/feature-ads")]
[ApiController]
public class AdsController : BaseController
{
    #region Initializations

    private readonly BL_Ads _bL_Ads;

    public AdsController(BL_Ads bL_Ads)
    {
        _bL_Ads = bL_Ads;
    }

    #endregion

    #region Get Ads List

    [HttpPost("filter")]
    public async Task<IActionResult> GetAdsList(
        [FromBody] FilterAdsRequestModel filterAdsRequestModel
    )
    {
        var result = await _bL_Ads.GetAdsList(filterAdsRequestModel);
        return Content(result);
    }

    #endregion

    #region Get Ads List By User Id

    [HttpGet]
    public async Task<IActionResult> GetAdsListByUserId(string userId, int pageNo, int pageSize)
    {
        var result = await _bL_Ads.GetAdsListByUserId(userId, pageNo, pageSize);
        return Content(result);
    }

    #endregion

    #region Create Ads

    [HttpPost]
    public async Task<IActionResult> CreateAds([FromForm] AdsRequestModel requestModel)
    {
        var result = await _bL_Ads.CreateAds(requestModel);
        return Content(result);
    }

    #endregion

    #region Update Ads

    [HttpPut]
    public async Task<IActionResult> UpdateAds(
        [FromForm] UpdateAdsRequestModel requestModel,
        string id
    )
    {
        var result = await _bL_Ads.UpdateAds(requestModel, id);
        return Content(result);
    }

    #endregion

    #region Patch Ads

    //[HttpPatch]
    //public async Task<IActionResult> PatchAds(string id)
    //{
    //    var result = await _bL_Ads.PatchAds(id);
    //    return Content(result);
    //}

    #endregion

    #region Delete Ads

    [HttpDelete]
    public async Task<IActionResult> DeleteAds(string id)
    {
        var result = await _bL_Ads.DeleteAds(id);
        return Content(result);
    }

    #endregion
}
