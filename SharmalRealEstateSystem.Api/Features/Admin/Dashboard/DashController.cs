namespace SharmalRealEstateSystem.Api.Features.Admin.Dashboard;

[Route("api/v1/feature-dashboard")]
[ApiController]
public class DashController : BaseController
{
    private readonly IDashboardRepository _dashboardRepository;

    public DashController(IDashboardRepository dashboardRepository)
    {
        _dashboardRepository = dashboardRepository;
    }

    #region Get Dashboard Data

    [HttpGet]
    public async Task<IActionResult> GetDashboardData(CancellationToken cs)
    {
        var result = await _dashboardRepository.GetDashboardData(cs);
        return Content(result);
    }

    #endregion
}
