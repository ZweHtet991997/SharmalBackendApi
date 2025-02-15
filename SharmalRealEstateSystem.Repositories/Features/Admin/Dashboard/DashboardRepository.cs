using SharmalRealEstateSystem.Models.Enums.Property;

namespace SharmalRealEstateSystem.Repositories.Features.Admin.Dashboard;

public class DashboardRepository : IDashboardRepository
{
    private readonly AppDbContext _appDbContext;
    private readonly DapperService _dapperService;

    public DashboardRepository(AppDbContext appDbContext, DapperService dapperService)
    {
        _appDbContext = appDbContext;
        _dapperService = dapperService;
    }

    public async Task<Result<DashboardResponseModel>> GetDashboardData(CancellationToken cs)
    {
        Result<DashboardResponseModel> result;
        try
        {
            int totalInquiryCount = await _appDbContext
                .TblInquires.Where(x => !x.IsDeleted)
                .CountAsync(cancellationToken: cs);

            int inquiryPropertyForSaleCount = await _dapperService.GetTotalCountAsync(CommonQuery.Sp_InquiryPropertyForSaleCount);

            int inquiryPropertyForRentCount = await _dapperService.GetTotalCountAsync(CommonQuery.Sp_InquiryPropertyForRentCount);

            int totalPropertyCount = await _appDbContext
                .TblProperties.Where(x => !x.IsDeleted)
                .CountAsync(cancellationToken: cs);

            int propertyForSaleCount = await _appDbContext
                .TblProperties.Where(x => !x.IsDeleted &&
                x.Status == Convert.ToString(EnumPropertyStatus.ရောင်းရန်))
                .CountAsync(cancellationToken: cs);

            int propertyForRentCount = await _appDbContext
                .TblProperties.Where(x => !x.IsDeleted &&
                x.Status == Convert.ToString(EnumPropertyStatus.ငှားရန်))
                .CountAsync(cancellationToken: cs);

            int carCount = await _appDbContext
                .TblCars.Where(x => !x.IsDeleted)
                .CountAsync(cancellationToken: cs);

            int inquiryCarCount = await _dapperService.GetTotalCountAsync(CommonQuery.Sp_InquiryCarCount);

            int inquiryOtherCount = await _appDbContext.TblInquires
                .Where(x => x.PropertyId == null && x.CarId == null && !x.IsDeleted)
                .CountAsync(cancellationToken: cs);

            int adsCount = await _appDbContext
                .TblAds.Where(x => !x.IsDeleted)
                .CountAsync(cancellationToken: cs);

            var model = new DashboardResponseModel()
            {
                TotalAdsCount = adsCount,
                TotalInquiryCount = totalInquiryCount,
                InquiryPropertyForSaleCount = inquiryPropertyForSaleCount,
                InquiryPropertyForRentCount = inquiryPropertyForRentCount,
                PropertyForSaleCount = propertyForSaleCount,
                PropertyForRentCount = propertyForRentCount,
                TotalCarCount = carCount,
                InquiryCarCount = inquiryCarCount,
                InquiryOtherCount = inquiryOtherCount,
                TotalPropertyCount = totalPropertyCount
            };

            result = Result<DashboardResponseModel>.SuccessResult(model);
        }
        catch (Exception ex)
        {
            result = Result<DashboardResponseModel>.FailureResult(ex);
        }

    result:
        return result;
    }
}
