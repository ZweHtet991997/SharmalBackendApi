namespace SharmalRealEstateSystem.Repositories.Features.Admin.AdsPage;

public class AdsPageRepository : IAdsPageRepository
{
    private readonly AppDbContext _context;

    public AdsPageRepository(AppDbContext context)
    {
        _context = context;
    }

    #region Get Ads Page List

    public async Task<Result<AdsPageListResponseModel>> GetAdsPageList(int? isDeleted)
    {
        Result<AdsPageListResponseModel> responseModel;
        try
        {
            var query = _context.TblAdsPages.OrderByDescending(x => x.AdsPageId);
            List<TblAdsPage> lst = new();
            var model = new AdsPageListResponseModel();

            if (isDeleted == 0)
            {
                responseModel = await GetActiveAdsPagesResult(query);
                goto result;
            }

            if (isDeleted == 1)
            {
                responseModel = await GetDeletedAdsPagesResult(query);
                goto result;
            }

            responseModel = await GetAllAdsPagesResult(query);
        }
        catch (Exception ex)
        {
            responseModel = Result<AdsPageListResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Create Ads Page

    public async Task<Result<AdsPageResponseModel>> CreateAdsPage(AdsPageRequestModel requestModel)
    {
        Result<AdsPageResponseModel> responseModel;
        try
        {
            bool isDuplicate = await CreateAdsPageDuplicate(requestModel.Pages!);

            if (isDuplicate)
            {
                responseModel = GetDuplicateAdsPageResponseModelResult();
                goto result;
            }

            await _context.TblAdsPages.AddAsync(requestModel.Map());
            int result = await _context.SaveChangesAsync();

            responseModel = Result<AdsPageResponseModel>.ExecuteResult(result);
        }
        catch (Exception ex)
        {
            responseModel = Result<AdsPageResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Update Ads Page

    public async Task<Result<AdsPageResponseModel>> UpdateAdsPage(
        AdsPageRequestModel requestModel,
        string adsPageId
    )
    {
        Result<AdsPageResponseModel> responseModel;
        try
        {
            #region Not Found Condition

            var item = await GetActiveAdsPageById(adsPageId);
            if (item is null)
            {
                responseModel = GetAdsPageNotFoundResult();
                goto result;
            }

            #endregion

            #region Duplicate

            bool isDuplicate = await UpdateAdsPageDuplicate(requestModel.Pages!, adsPageId);
            if (isDuplicate)
            {
                responseModel = GetDuplicateAdsPageResponseModelResult();
                goto result;
            }

            #endregion

            item.Pages = requestModel.Pages;
            _context.TblAdsPages.Update(item);
            int result = await _context.SaveChangesAsync();

            responseModel = Result<AdsPageResponseModel>.ExecuteResult(result);
        }
        catch (Exception ex)
        {
            responseModel = Result<AdsPageResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Reactivate Ads Page

    public async Task<Result<AdsPageResponseModel>> ReactivateAdsPage(string adsPageId)
    {
        Result<AdsPageResponseModel> responseModel;
        try
        {
            #region Not Found

            var item = await GetDeletedAdsPageById(adsPageId);
            if (item is null)
            {
                responseModel = GetAdsPageNotFoundResult();
                goto result;
            }

            #endregion

            item.IsDeleted = false;
            _context.TblAdsPages.Update(item);
            int result = await _context.SaveChangesAsync();

            responseModel = Result<AdsPageResponseModel>.ExecuteResult(result);
        }
        catch (Exception ex)
        {
            responseModel = Result<AdsPageResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Delete Ads Page

    public async Task<Result<AdsPageResponseModel>> DeleteAdsPage(string adsPageId)
    {
        Result<AdsPageResponseModel> responseModel;
        try
        {
            #region Not Found

            var item = await GetActiveAdsPageById(adsPageId);
            if (item is null)
            {
                responseModel = GetAdsPageNotFoundResult();
                goto result;
            }

            #endregion

            item.IsDeleted = true;
            _context.TblAdsPages.Update(item);
            int result = await _context.SaveChangesAsync();

            responseModel = Result<AdsPageResponseModel>.ExecuteResult(result);
        }
        catch (Exception ex)
        {
            responseModel = Result<AdsPageResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    private async Task<List<TblAdsPage>> GetActiveAdsPages(IQueryable<TblAdsPage> query) =>
        await query.Where(x => !x.IsDeleted).ToListAsync();

    private async Task<List<TblAdsPage>> GetDeletedAdsPages(IQueryable<TblAdsPage> query) =>
        await query.Where(x => x.IsDeleted).ToListAsync();

    private async Task<List<TblAdsPage>> GetAllAdsPages(IQueryable<TblAdsPage> query) =>
        await query.ToListAsync();

    private async Task<Result<AdsPageListResponseModel>> GetActiveAdsPagesResult(
        IQueryable<TblAdsPage> query
    )
    {
        var lst = await GetActiveAdsPages(query);
        var model = new AdsPageListResponseModel(lst.Select(x => x.Map()).ToList());
        return Result<AdsPageListResponseModel>.SuccessResult(model);
    }

    private async Task<Result<AdsPageListResponseModel>> GetDeletedAdsPagesResult(
        IQueryable<TblAdsPage> query
    )
    {
        var lst = await GetDeletedAdsPages(query);
        var model = new AdsPageListResponseModel(lst.Select(x => x.Map()).ToList());
        return Result<AdsPageListResponseModel>.SuccessResult(model);
    }

    private async Task<Result<AdsPageListResponseModel>> GetAllAdsPagesResult(
        IQueryable<TblAdsPage> query
    )
    {
        var lst = await GetAllAdsPages(query);
        var model = new AdsPageListResponseModel(lst.Select(x => x.Map()).ToList());
        return Result<AdsPageListResponseModel>.SuccessResult(model);
    }

    private async Task<bool> CreateAdsPageDuplicate(string pages) =>
        await _context.TblAdsPages.AnyAsync(x => x.Pages == pages && !x.IsDeleted);

    private Result<AdsPageResponseModel> GetDuplicateAdsPageResponseModelResult() =>
        Result<AdsPageResponseModel>.FailureResult(
            MessageResource.Duplicate,
            EnumStatusCode.Conflict
        );

    private async Task<TblAdsPage?> GetActiveAdsPageById(string adsPageId)
    {
        return await _context.TblAdsPages.FirstOrDefaultAsync(x =>
            x.AdsPageId == adsPageId && !x.IsDeleted
        );
    }

    private async Task<TblAdsPage?> GetDeletedAdsPageById(string adsPageId)
    {
        return await _context.TblAdsPages.FirstOrDefaultAsync(x =>
            x.AdsPageId == adsPageId && x.IsDeleted
        );
    }

    private Result<AdsPageResponseModel> GetAdsPageNotFoundResult() =>
        Result<AdsPageResponseModel>.FailureResult(
            MessageResource.NotFound,
            EnumStatusCode.NotFound
        );

    private async Task<bool> UpdateAdsPageDuplicate(string pages, string adsPageId) =>
        await _context.TblAdsPages.AnyAsync(x =>
            x.Pages == pages && !x.IsDeleted && x.AdsPageId != adsPageId
        );
}
