using SharmalRealEstateSystem.Models.Features.Admin.Dashboard;

namespace SharmalRealEstateSystem.Repositories.Features.Admin.Dashboard;

public interface IDashboardRepository
{
    Task<Result<DashboardResponseModel>> GetDashboardData(CancellationToken cs);
}
