namespace SharmalRealEstateSystem.Api.Features.Admin.Auth;

[Route("api/v1/admin-account")]
[ApiController]
public class AuthController : BaseController
{
    #region Initializations

    private readonly BL_Auth _bL_Auth;

    public AuthController(BL_Auth bL_Auth)
    {
        _bL_Auth = bL_Auth;
    }

    #endregion

    #region Get User List

    [HttpGet]
    public async Task<IActionResult> GetUserList(int pageNo, int pageSize)
    {
        var result = await _bL_Auth.GetUserList(pageNo, pageSize);
        return Content(result);
    }

    #endregion

    #region Register

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestModel requestModel)
    {
        var result = await _bL_Auth.Register(requestModel);
        return Content(result);
    }

    #endregion

    #region Login

    [HttpPost("/api/v1/feature-account/login")]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel requestModel)
    {
        var result = await _bL_Auth.Login(requestModel);
        return Content(result);
    }

    #endregion

    #region Reset Fail Count

    [HttpPatch]
    public async Task<IActionResult> ResetFailCount(string id)
    {
        var result = await _bL_Auth.ResetFailCount(id);
        return Content(result);
    }

    #endregion

    //[HttpPut("/api/v1/account")]
    //public async Task<IActionResult> UpdateUser([FromBody] UpdateAuthRequestModel requestModel, string id)
    //{
    //    var context = HttpContext;
    //    var result = await _bL_Auth.UpdateUser(requestModel, id, context);
    //    return Content(result);
    //}

    #region Update Profile

    [HttpPut("update-profile")]
    public async Task<IActionResult> UpdateProfile(
        [FromBody] UpdateProfileRequestModel requestModel,
        string id
    )
    {
        var result = await _bL_Auth.UpdateProfile(requestModel, id, HttpContext);
        return Content(result);
    }

    #endregion

    #region Update Password

    [HttpPut("/api/v1/feature-account/update-password")]
    public async Task<IActionResult> UpdatePassword(
        [FromBody] UpdatePasswordRequestModel requestModel,
        string id
    )
    {
        var result = await _bL_Auth.UpdatePassword(requestModel, id, HttpContext);
        return Content(result);
    }

    #endregion

    #region Delete User

    [HttpDelete]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var result = await _bL_Auth.DeleteUser(id);
        return Content(result);
    }

    #endregion
}
