namespace SharmalRealEstateSystem.Repositories.Features.Admin.Feature;

public class FeatureRepository : IFeatureRepository
{
    private readonly AppDbContext _context;

    public FeatureRepository(AppDbContext context)
    {
        _context = context;
    }

    #region Get Feature List

    public async Task<Result<FeatureListResponseModel>> GetFeatureList(int? isDeleted)
    {
        Result<FeatureListResponseModel> responseModel;
        try
        {
            List<TblFeature> lst = new();
            FeatureListResponseModel model = new();

            if (isDeleted == 1)
            {
                lst = await GetDeletedFeatureListAsync();
                model = GetFeatureListResponseModel(lst);

                responseModel = Result<FeatureListResponseModel>.SuccessResult(model);
                goto result;
            }

            if (isDeleted == 0)
            {
                lst = await GetActiveFeatureList();
                model = GetFeatureListResponseModel(lst);

                responseModel = Result<FeatureListResponseModel>.SuccessResult(model);
                goto result;
            }

            lst = await GetAllFeatureList();
            model = GetFeatureListResponseModel(lst);

            responseModel = Result<FeatureListResponseModel>.SuccessResult(model);
        }
        catch (Exception ex)
        {
            responseModel = Result<FeatureListResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Create Feature

    public async Task<Result<FeatureResponseModel>> CreateFeature(FeatureRequestModel requestModel)
    {
        Result<FeatureResponseModel> responseModel;
        try
        {
            bool isDuplicate = await _context.TblFeatures.AnyAsync(x =>
                x.Name == requestModel.Name && !x.IsDeleted
            );
            if (isDuplicate)
            {
                responseModel = GetFeatureDuplicateResult();
                goto result;
            }

            await _context.TblFeatures.AddAsync(requestModel.Map());
            await _context.SaveChangesAsync();

            responseModel = GetFeatureSaveSuccessResult();
        }
        catch (Exception ex)
        {
            responseModel = Result<FeatureResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Update Feature

    public async Task<Result<FeatureResponseModel>> UpdateFeature(
        FeatureRequestModel requestModel,
        string featureId
    )
    {
        Result<FeatureResponseModel> responseModel;
        try
        {
            bool isDuplicate = await _context.TblFeatures.AnyAsync(x =>
                x.Name == requestModel.Name && !x.IsDeleted && x.FeatureId != featureId
            );
            if (isDuplicate)
            {
                responseModel = GetFeatureDuplicateResult();
                goto result;
            }

            var item = await _context.TblFeatures.FindAsync(featureId);
            if (item is null)
            {
                responseModel = GetFeatureNotFoundResult();
                goto result;
            }

            item.Name = requestModel.Name;
            _context.Update(item);
            //int result = await _context.SaveChangesAsync();
            await _context.SaveChangesAsync();

            responseModel = GetFeatureUpdateSuccessResult();
        }
        catch (Exception ex)
        {
            responseModel = Result<FeatureResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Update Feature V1

    public async Task<Result<FeatureResponseModel>> UpdateFeatureV1(
        FeatureRequestModel requestModel,
        string featureId
    )
    {
        Result<FeatureResponseModel> responseModel;
        try
        {
            #region Feature Duplicate

            bool isDuplicate = await UpdateFeatureDuplicate(requestModel.Name!, featureId);
            if (isDuplicate)
            {
                responseModel = GetFeatureDuplicateResult();
                goto result;
            }

            #endregion

            #region Not Found Condition

            var item = await _context.TblFeatures.FindAsync(featureId);
            if (item is null)
            {
                responseModel = GetFeatureNotFoundResult();
                goto result;
            }

            #endregion

            item.Name = requestModel.Name;
            _context.Update(item);
            await _context.SaveChangesAsync();

            responseModel = GetFeatureUpdateSuccessResult();
        }
        catch (Exception ex)
        {
            responseModel = Result<FeatureResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Reactivate Feature

    public async Task<Result<FeatureResponseModel>> ReactivateFeature(string featureId)
    {
        Result<FeatureResponseModel> responseModel;
        try
        {
            #region Not Found Condition

            var item = await _context.TblFeatures.FirstOrDefaultAsync(x =>
                x.FeatureId == featureId && x.IsDeleted
            );
            if (item is null)
            {
                responseModel = Result<FeatureResponseModel>.FailureResult(
                    MessageResource.NotFound,
                    EnumStatusCode.NotFound
                );
                goto result;
            }

            #endregion

            item.IsDeleted = false;
            _context.TblFeatures.Update(item);
            //int result = await _context.SaveChangesAsync();
            await _context.SaveChangesAsync();

            responseModel = GetFeatureUpdateSuccessResult();
        }
        catch (Exception ex)
        {
            responseModel = Result<FeatureResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Delete Feature

    public async Task<Result<FeatureResponseModel>> DeleteFeature(string featureId)
    {
        Result<FeatureResponseModel> responseModel;
        try
        {
            #region Not Found Condition

            var item = await _context.TblFeatures.FindAsync(featureId);
            if (item is null)
            {
                responseModel = GetFeatureNotFoundResult();
                goto result;
            }

            #endregion

            item.IsDeleted = true;
            _context.Update(item);
            //int result = await _context.SaveChangesAsync();
            await _context.SaveChangesAsync();

            responseModel = GetFeatureDeleteSuccessResult();
        }
        catch (Exception ex)
        {
            responseModel = Result<FeatureResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    private async Task<List<TblFeature>> GetDeletedFeatureListAsync()
    {
        return await _context
            .TblFeatures.OrderByDescending(x => x.FeatureId)
            .Where(x => x.IsDeleted)
            .ToListAsync();
    }

    private async Task<List<TblFeature>> GetActiveFeatureList()
    {
        return await _context
            .TblFeatures.OrderByDescending(x => x.FeatureId)
            .Where(x => !x.IsDeleted)
            .ToListAsync();
    }

    private async Task<List<TblFeature>> GetAllFeatureList()
    {
        return await _context.TblFeatures.OrderByDescending(x => x.FeatureId).ToListAsync();
    }

    private FeatureListResponseModel GetFeatureListResponseModel(List<TblFeature> lst) =>
        new() { DataLst = lst.Select(x => x.Map()).ToList() };

    private async Task<bool> UpdateFeatureDuplicate(string featureName, string featureId) =>
        await _context.TblFeatures.AnyAsync(x =>
            x.Name == featureName && !x.IsDeleted && x.FeatureId != featureId
        );

    private Result<FeatureResponseModel> GetFeatureSaveSuccessResult() =>
        Result<FeatureResponseModel>.SuccessResult(
            MessageResource.SaveSuccess,
            EnumStatusCode.Success
        );

    private Result<FeatureResponseModel> GetFeatureUpdateSuccessResult() =>
        Result<FeatureResponseModel>.SuccessResult(
            MessageResource.UpdateSuccess,
            EnumStatusCode.Success
        );

    private Result<FeatureResponseModel> GetFeatureDuplicateResult() =>
        Result<FeatureResponseModel>.FailureResult("Feature Duplicate.", EnumStatusCode.Conflict);

    private Result<FeatureResponseModel> GetFeatureNotFoundResult() =>
        Result<FeatureResponseModel>.FailureResult("Feature Not Found.", EnumStatusCode.NotFound);

    private Result<FeatureResponseModel> GetFeatureDeleteSuccessResult() =>
        Result<FeatureResponseModel>.SuccessResult(
            MessageResource.DeleteSuccess,
            EnumStatusCode.Success
        );
}
