namespace SharmalRealEstateSystem.Api.Features.Admin.AdsPage;

[ApiController]
[Route("api/v1")]
public class AdsPageController : BaseController
{
    #region Initializations

    private readonly BL_AdsPage _bL_AdsPage;

    public AdsPageController(BL_AdsPage bL_AdsPage)
    {
        _bL_AdsPage = bL_AdsPage;
    }

    #endregion

    #region Get Ads Page List

    [HttpGet]
    [Route("feature-ads-page")]
    public async Task<IActionResult> GetAdsPageList(int? isDeleted)
    {
        var result = await _bL_AdsPage.GetAdsPageList(isDeleted);
        return Content(result);
    }

    #endregion

    #region Create Ads Page

    [HttpPost]
    [Route("admin-ads-page")]
    public async Task<IActionResult> CreateAdsPage(AdsPageRequestModel requestModel)
    {
        var result = await _bL_AdsPage.CreateAdsPage(requestModel);
        return Content(result);
    }

    #endregion

    #region Update Ads Page

    [HttpPut]
    [Route("admin-ads-page")]
    public async Task<IActionResult> UpdateAdsPage(AdsPageRequestModel requestModel, string id)
    {
        var result = await _bL_AdsPage.UpdateAdsPage(requestModel, id);
        return Content(result);
    }

    #endregion

    #region Reactivate Ads Page

    [HttpPatch]
    [Route("admin-ads-page")]
    public async Task<IActionResult> ReactivateAdsPage(string id)
    {
        var result = await _bL_AdsPage.ReactivateAdsPage(id);
        return Content(result);
    }

    #endregion

    #region Delete Ads Page

    [HttpDelete]
    [Route("admin-ads-page")]
    public async Task<IActionResult> DeleteAdsPage(string id)
    {
        var result = await _bL_AdsPage.DeleteAdsPage(id);
        return Content(result);
    }

    #endregion
}
