namespace SharmalRealEstateSystem.Repositories.Features.Admin.ExchangeRate;

public class ExchangeRateRepository : IExchangeRateRepository
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ExchangeRateRepository(AppDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    #region Get Exchange Rate List

    public async Task<Result<ExchangeRateListResponseModel>> GetExchangeRateList()
    {
        Result<ExchangeRateListResponseModel> responseModel;
        try
        {
            var lst = await _context.TblExchangeRates.ToListAsync();
            var model = new List<ExchangeRateDataModel>();
            string basePath = _webHostEnvironment.WebRootPath;

            lst.ForEach(item => model.Add(item.Map(basePath)));

            responseModel = Result<ExchangeRateListResponseModel>.SuccessResult(
                new ExchangeRateListResponseModel { DataLst = model }
            );
        }
        catch (Exception ex)
        {
            responseModel = Result<ExchangeRateListResponseModel>.FailureResult(ex);
        }

        return responseModel;
    }

    #endregion

    #region Update Exchange Rate

    public async Task<Result<ExchangeRateResponseModel>> UpdateExchangeRate(
        UpdateExchangeRateRequestModel requestModel
    )
    {
        Result<ExchangeRateResponseModel> responseModel;
        try
        {
            foreach (var exchangeRate in requestModel.ExchangeRates!)
            {
                var item = await _context.TblExchangeRates.FindAsync(exchangeRate.ExchangeRateId);
                if (item is null)
                {
                    responseModel = GetExchangeRateNotFoundResult();
                    goto result;
                }

                item.Currency = exchangeRate.Currency;
                item.ExchangeRate = exchangeRate.ExchangeRate;
                _context.TblExchangeRates.Update(item);
            }

            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

            var lst = await _context.TblExchangeRates.ToListAsync();
            var myanmarTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Myanmar Standard Time");
            var myanmarDateTime = TimeZoneInfo.ConvertTime(
                DateTime.Now,
                TimeZoneInfo.Local,
                myanmarTimeZone
            );

            foreach (var item in lst)
            {
                item.UpdatedDate = DevCode.GetCurrentMyanmarDateTime();
                _context.TblExchangeRates.Update(item);
            }

            await _context.SaveChangesAsync();

            responseModel = Result<ExchangeRateResponseModel>.SuccessResult(
                MessageResource.UpdateSuccess,
                EnumStatusCode.Success
            );
        }
        catch (Exception ex)
        {
            responseModel = Result<ExchangeRateResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Delete Exchange Rate

    public async Task<Result<ExchangeRateResponseModel>> DeleteExchangeRate(int id)
    {
        Result<ExchangeRateResponseModel> responseModel;
        try
        {
            var item = await _context.TblExchangeRates.FindAsync(id);
            if (item is null)
            {
                responseModel = GetExchangeRateNotFoundResult();
                goto result;
            }

            _context.TblExchangeRates.Remove(item);
            await _context.SaveChangesAsync();

            responseModel = Result<ExchangeRateResponseModel>.SuccessResult(
                MessageResource.DeleteSuccess,
                EnumStatusCode.Success
            );
        }
        catch (Exception ex)
        {
            responseModel = Result<ExchangeRateResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    private Result<ExchangeRateResponseModel> GetExchangeRateNotFoundResult() =>
        Result<ExchangeRateResponseModel>.FailureResult(
            MessageResource.NotFound,
            EnumStatusCode.NotFound
        );
}
