namespace SharmalRealEstateSystem.Api.Features.Admin.Inquiry;

[Route("api/v1/feature-inquiry")]
[ApiController]
public class InquiryController : BaseController
{
    #region Initializations

    private readonly BL_Inquiry _bL_Inquiry;

    public InquiryController(BL_Inquiry bL_Inquiry)
    {
        _bL_Inquiry = bL_Inquiry;
    }

    #endregion

    #region Get Inquiry List

    [HttpPost("filter")]
    public async Task<IActionResult> GetInquiryList([FromBody] GetFilterInquiryRequestModel requestModel)
    {
        var result = await _bL_Inquiry.GetInquiryList(requestModel);
        return Content(result);
    }

    #endregion

    #region Create Inquiry

    [HttpPost]
    public async Task<IActionResult> CreateInquiry([FromBody] InquiryRequestModel requestModel)
    {
        var result = await _bL_Inquiry.CreateInquiry(requestModel);
        return Content(result);
    }

    #endregion

    #region Update Inquiry

    //[HttpPut]
    //public async Task<IActionResult> UpdateInquiry(
    //    [FromBody] UpdateInquiryRequestModel requestModel,
    //    string id
    //)
    //{
    //    var result = await _bL_Inquiry.UpdateInquiry(requestModel, id);
    //    return Content(result);
    //}

    #endregion

    #region Patch Inquiry

    [HttpPatch]
    public async Task<IActionResult> PatchInquiry(string id)
    {
        var result = await _bL_Inquiry.PatchInquiry(id);
        return Content(result);
    }

    #endregion

    #region Delete Inquiry

    [HttpDelete]
    public async Task<IActionResult> DeleteInquiry(string id)
    {
        var result = await _bL_Inquiry.DeleteInquiry(id);
        return Content(result);
    }

    #endregion
}
