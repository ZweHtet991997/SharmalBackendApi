namespace SharmalRealEstateSystem.Repositories.Features.Admin.Inquiry;

public class InquiryRepository : IInquiryRepository
{
    private readonly AppDbContext _context;
    private readonly DapperService _dapperService;

    public InquiryRepository(AppDbContext context, DapperService dapperService)
    {
        _context = context;
        _dapperService = dapperService;
    }

    #region Get Inquiry List

    public async Task<Result<InquiryListResponseModel>> GetInquiryList(
        GetFilterInquiryRequestModel requestModel
    )
    {
        Result<InquiryListResponseModel> responseModel;
        try
        {
            var parameters = new
            {
                requestModel.PageNo,
                requestModel.PageSize,
                requestModel.Status,
                requestModel.InquiryStatus
            };
            List<InquiryDataModel> lst = new();

            #region FilterType is Property

            if (requestModel.FilterType == Convert.ToString(EnumFilerType.Property))
            {
                lst = await _dapperService.QueryAsync<InquiryDataModel>(
                    CommonQuery.GetFilterPropertyInquiry,
                    parameters,
                    CommandType.StoredProcedure
                );
            }

            #endregion

            #region FilterType is Car

            if (requestModel.FilterType == Convert.ToString(EnumFilerType.Car))
            {
                #region Car / Unread

                if (requestModel.InquiryStatus == Convert.ToString(EnumInquiryStatus.Unread))
                {
                    var dataLst = await _context
                        .TblInquires.Where(x => !x.IsDone && !x.IsDeleted && x.CarId != null)
                        .ToListAsync();

                    lst = dataLst
                        .Select(x => new InquiryDataModel
                        {
                            Description = x.Description,
                            CarId = x.CarId,
                            IsDeleted = x.IsDeleted,
                            CreatedDate = x.CreatedDate,
                            Email = x.Email,
                            InquiresId = x.InquiresId,
                            IsDone = x.IsDone,
                            PhoneNumber = x.PhoneNumber,
                            PropertyId = x.PropertyId,
                            UpdatedDate = x.UpdatedDate,
                            UserName = x.UserName
                        })
                        .ToList();
                }
                #endregion

                #region Car / Done

                else if (requestModel.InquiryStatus == Convert.ToString(EnumInquiryStatus.Done))
                {
                    var dataLst = await _context
                        .TblInquires.Where(x => x.IsDone && !x.IsDeleted && x.CarId != null)
                        .ToListAsync();

                    lst = dataLst
                        .Select(x => new InquiryDataModel
                        {
                            Description = x.Description,
                            CarId = x.CarId,
                            IsDeleted = x.IsDeleted,
                            CreatedDate = x.CreatedDate,
                            Email = x.Email,
                            InquiresId = x.InquiresId,
                            IsDone = x.IsDone,
                            PhoneNumber = x.PhoneNumber,
                            PropertyId = x.PropertyId,
                            UpdatedDate = x.UpdatedDate,
                            UserName = x.UserName
                        })
                        .ToList();
                }
                #endregion

                #region Car / All

                else
                {
                    var dataLst = await _context
                        .TblInquires.Where(x => !x.IsDeleted && x.CarId != null)
                        .ToListAsync();

                    lst = dataLst
                        .Select(x => new InquiryDataModel
                        {
                            Description = x.Description,
                            CarId = x.CarId,
                            IsDeleted = x.IsDeleted,
                            CreatedDate = x.CreatedDate,
                            Email = x.Email,
                            InquiresId = x.InquiresId,
                            IsDone = x.IsDone,
                            PhoneNumber = x.PhoneNumber,
                            PropertyId = x.PropertyId,
                            UpdatedDate = x.UpdatedDate,
                            UserName = x.UserName
                        })
                        .ToList();
                }

                #endregion
            }

            #endregion

            #region Filter is Other

            if (requestModel.FilterType == Convert.ToString(EnumFilerType.Other))
            {
                #region Other / Unread

                if (requestModel.InquiryStatus == Convert.ToString(EnumInquiryStatus.Unread))
                {
                    var dataLst = await _context
                        .TblInquires.Where(x =>
                            x.PropertyId == null && x.CarId == null && !x.IsDone && !x.IsDeleted
                        )
                        .ToListAsync();

                    lst = dataLst
                        .Select(x => new InquiryDataModel
                        {
                            Description = x.Description,
                            CarId = x.CarId,
                            IsDeleted = x.IsDeleted,
                            CreatedDate = x.CreatedDate,
                            Email = x.Email,
                            InquiresId = x.InquiresId,
                            IsDone = x.IsDone,
                            PhoneNumber = x.PhoneNumber,
                            PropertyId = x.PropertyId,
                            UpdatedDate = x.UpdatedDate,
                            UserName = x.UserName
                        })
                        .ToList();
                }
                #endregion

                #region Other / Done

                else if (requestModel.InquiryStatus == Convert.ToString(EnumInquiryStatus.Done))
                {
                    var dataLst = await _context
                        .TblInquires.Where(x =>
                            x.PropertyId == null && x.CarId == null && x.IsDone && !x.IsDeleted
                        )
                        .ToListAsync();

                    lst = dataLst
                        .Select(x => new InquiryDataModel
                        {
                            Description = x.Description,
                            CarId = x.CarId,
                            IsDeleted = x.IsDeleted,
                            CreatedDate = x.CreatedDate,
                            Email = x.Email,
                            InquiresId = x.InquiresId,
                            IsDone = x.IsDone,
                            PhoneNumber = x.PhoneNumber,
                            PropertyId = x.PropertyId,
                            UpdatedDate = x.UpdatedDate,
                            UserName = x.UserName
                        })
                        .ToList();
                }
                #endregion

                #region Other / All

                else
                {
                    var dataLst = await _context
                        .TblInquires.Where(x =>
                            x.PropertyId == null && x.CarId == null && !x.IsDeleted
                        )
                        .ToListAsync();

                    lst = dataLst
                        .Select(x => new InquiryDataModel
                        {
                            Description = x.Description,
                            CarId = x.CarId,
                            IsDeleted = x.IsDeleted,
                            CreatedDate = x.CreatedDate,
                            Email = x.Email,
                            InquiresId = x.InquiresId,
                            IsDone = x.IsDone,
                            PhoneNumber = x.PhoneNumber,
                            PropertyId = x.PropertyId,
                            UpdatedDate = x.UpdatedDate,
                            UserName = x.UserName
                        })
                        .ToList();
                }

                #endregion
            }

            #endregion

            var model = new InquiryListResponseModel(lst);

            responseModel = Result<InquiryListResponseModel>.SuccessResult(model);
        }
        catch (Exception ex)
        {
            responseModel = Result<InquiryListResponseModel>.FailureResult(ex);
        }

        return responseModel;
    }

    #endregion

    #region Get Inquiry List V1

    public async Task<Result<InquiryListResponseModel>> GetInquiryListV1(
        GetFilterInquiryRequestModel requestModel
    )
    {
        Result<InquiryListResponseModel> responseModel;
        try
        {
            var parameters = new
            {
                requestModel.PageNo,
                requestModel.PageSize,
                requestModel.Status,
                requestModel.InquiryStatus
            };
            List<InquiryDataModel> lst = new();

            #region FilterType is Property

            if (requestModel.FilterType == Convert.ToString(EnumFilerType.Property))
            {
                lst = await _dapperService.QueryAsync<InquiryDataModel>(
                    CommonQuery.GetFilterPropertyInquiry,
                    parameters,
                    CommandType.StoredProcedure
                );
            }

            #endregion

            #region FilterType is Car

            if (requestModel.FilterType == Convert.ToString(EnumFilerType.Car))
            {
                #region Car / Unread

                if (requestModel.InquiryStatus == Convert.ToString(EnumInquiryStatus.Unread))
                {
                    var dataLst = await GetCarUnReadList();
                    lst = dataLst.Select(x => x.Change()).ToList();
                }
                #endregion

                #region Car / Done

                else if (requestModel.InquiryStatus == Convert.ToString(EnumInquiryStatus.Done))
                {
                    var dataLst = await GetCarDoneList();
                    lst = dataLst.Select(x => x.Change()).ToList();
                }
                #endregion

                #region Car / All

                else
                {
                    var dataLst = await GetCarList();
                    lst = dataLst.Select(x => x.Change()).ToList();
                }

                #endregion
            }

            #endregion

            #region Filter is Other

            if (requestModel.FilterType == Convert.ToString(EnumFilerType.Other))
            {
                #region Other / Unread

                if (requestModel.InquiryStatus == Convert.ToString(EnumInquiryStatus.Unread))
                {
                    var dataLst = await GetOtherUnReadList();
                    lst = dataLst.Select(x => x.Change()).ToList();
                }
                #endregion

                #region Other / Done

                else if (requestModel.InquiryStatus == Convert.ToString(EnumInquiryStatus.Done))
                {
                    var dataLst = await GetOtherDoneList();
                    lst = dataLst.Select(x => x.Change()).ToList();
                }
                #endregion

                #region Other / All

                else
                {
                    var dataLst = await GetOtherList();
                    lst = dataLst.Select(x => x.Change()).ToList();
                }

                #endregion
            }

            #endregion

            var model = new InquiryListResponseModel(lst);

            responseModel = Result<InquiryListResponseModel>.SuccessResult(model);
        }
        catch (Exception ex)
        {
            responseModel = Result<InquiryListResponseModel>.FailureResult(ex);
        }

        return responseModel;
    }

    #endregion

    #region Get Inquiry List V2

    public async Task<Result<InquiryListResponseModelV2>> GetInquiryListV2(
        GetFilterInquiryRequestModel requestModel
    )
    {
        Result<InquiryListResponseModelV2> responseModel;
        try
        {
            var parameters = new
            {
                requestModel.PageNo,
                requestModel.PageSize,
                requestModel.Status,
                requestModel.InquiryStatus
            };
            List<InquiryDataModel> lst = new();

            #region FilterType is Property

            if (requestModel.FilterType == Convert.ToString(EnumFilerType.Property))
            {
                lst = await _dapperService.QueryAsync<InquiryDataModel>(
                    CommonQuery.GetFilterPropertyInquiry,
                    parameters,
                    CommandType.StoredProcedure
                );
            }

            #endregion

            #region FilterType is Car

            if (requestModel.FilterType == Convert.ToString(EnumFilerType.Car))
            {
                #region Car / Unread

                if (requestModel.InquiryStatus == Convert.ToString(EnumInquiryStatus.Unread))
                {
                    var query = _context.TblInquires.Where(x =>
                        !x.IsDone && !x.IsDeleted && x.CarId != null
                    );

                    var totalCount = await query.CountAsync();
                    var pageCount = totalCount / requestModel.PageSize;

                    if (totalCount % requestModel.PageSize > 0)
                    {
                        pageCount++;
                    }

                    var dataLst = await query.ToListAsync();
                    lst = dataLst.Select(x => x.Change()).ToList();

                    var pageSettingModel = new PageSettingModel(
                        requestModel.PageNo,
                        requestModel.PageSize,
                        pageCount,
                        totalCount
                    );
                    var model = new InquiryListResponseModelV2(lst, pageSettingModel);

                    responseModel = Result<InquiryListResponseModelV2>.SuccessResult(model);
                    goto result;
                }
                #endregion

                #region Car / Done

                else if (requestModel.InquiryStatus == Convert.ToString(EnumInquiryStatus.Done))
                {
                    var query = _context.TblInquires.Where(x =>
                        x.IsDone && !x.IsDeleted && x.CarId != null
                    );

                    var totalCount = await query.CountAsync();
                    var pageCount = totalCount / requestModel.PageSize;

                    if (totalCount % requestModel.PageSize > 0)
                    {
                        pageCount++;
                    }

                    var dataLst = await query.ToListAsync();
                    lst = dataLst.Select(x => x.Change()).ToList();

                    var pageSettingModel = new PageSettingModel(
                        requestModel.PageNo,
                        requestModel.PageSize,
                        pageCount,
                        totalCount
                    );
                    var model = new InquiryListResponseModelV2(lst, pageSettingModel);

                    responseModel = Result<InquiryListResponseModelV2>.SuccessResult(model);
                    goto result;
                }
                #endregion

                #region Car / All

                else
                {
                    var query = _context.TblInquires.Where(x => !x.IsDeleted && x.CarId != null);

                    var dataLst = await query.ToListAsync();
                    lst = dataLst.Select(x => x.Change()).ToList();

                    var totalCount = await query.CountAsync();
                    var pageCount = totalCount / requestModel.PageSize;

                    if (totalCount % requestModel.PageSize > 0)
                    {
                        pageCount++;
                    }

                    var pageSettingModel = new PageSettingModel(
                        requestModel.PageNo,
                        requestModel.PageSize,
                        pageCount,
                        totalCount
                    );
                    var model = new InquiryListResponseModelV2(lst, pageSettingModel);

                    responseModel = Result<InquiryListResponseModelV2>.SuccessResult(model);
                    goto result;
                }

                #endregion
            }

            #endregion

            #region Filter is Other

            if (requestModel.FilterType == Convert.ToString(EnumFilerType.Other))
            {
                #region Other / Unread

                if (requestModel.InquiryStatus == Convert.ToString(EnumInquiryStatus.Unread))
                {
                    var query = _context.TblInquires.Where(x =>
                        x.PropertyId == null && x.CarId == null && !x.IsDone && !x.IsDeleted
                    );

                    var totalCount = await query.CountAsync();
                    var pageCount = totalCount / requestModel.PageSize;
                    if (totalCount % requestModel.PageSize > 0)
                    {
                        pageCount++;
                    }

                    var dataLst = await query.ToListAsync();
                    lst = dataLst.Select(x => x.Change()).ToList();

                    var pageSettingModel = new PageSettingModel(
                        requestModel.PageNo,
                        requestModel.PageSize,
                        pageCount,
                        totalCount
                    );
                    var model = new InquiryListResponseModelV2(lst, pageSettingModel);

                    responseModel = Result<InquiryListResponseModelV2>.SuccessResult(model);
                    goto result;
                }
                #endregion

                #region Other / Done

                else if (requestModel.InquiryStatus == Convert.ToString(EnumInquiryStatus.Done))
                {
                    var query = _context.TblInquires.Where(x =>
                        x.PropertyId == null && x.CarId == null && x.IsDone && !x.IsDeleted
                    );

                    var totalCount = await query.CountAsync();
                    var pageCount = totalCount / requestModel.PageSize;
                    if (totalCount % requestModel.PageSize > 0)
                    {
                        pageCount++;
                    }

                    var dataLst = await GetOtherDoneList();
                    lst = dataLst.Select(x => x.Change()).ToList();

                    var pageSettingModel = new PageSettingModel(
                        requestModel.PageNo,
                        requestModel.PageSize,
                        pageCount,
                        totalCount
                    );
                    var model = new InquiryListResponseModelV2(lst, pageSettingModel);

                    responseModel = Result<InquiryListResponseModelV2>.SuccessResult(model);
                    goto result;
                }
                #endregion

                #region Other / All

                else
                {
                    var query = _context.TblInquires.Where(x =>
                        x.PropertyId == null && x.CarId == null && !x.IsDeleted
                    );

                    var totalCount = await query.CountAsync();
                    var pageCount = totalCount / requestModel.PageSize;
                    if (totalCount % requestModel.PageSize > 0)
                    {
                        pageCount++;
                    }

                    var dataLst = await GetOtherList();
                    lst = dataLst.Select(x => x.Change()).ToList();

                    var pageSettingModel = new PageSettingModel(
                        requestModel.PageNo,
                        requestModel.PageSize,
                        pageCount,
                        totalCount
                    );
                    var model = new InquiryListResponseModelV2(lst, pageSettingModel);

                    responseModel = Result<InquiryListResponseModelV2>.SuccessResult(model);
                    goto result;
                }

                #endregion
            }

            #endregion

            responseModel = Result<InquiryListResponseModelV2>.SuccessResult();
        }
        catch (Exception ex)
        {
            responseModel = Result<InquiryListResponseModelV2>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Get Inquiry List V3

    public async Task<Result<InquiryListResponseModelV2>> GetInquiryListV3(
        GetFilterInquiryRequestModel requestModel
    )
    {
        Result<InquiryListResponseModelV2> responseModel;
        try
        {
            var parameters = new
            {
                requestModel.PageNo,
                requestModel.PageSize,
                requestModel.Status,
                requestModel.InquiryStatus
            };
            List<InquiryDataModel> lst = new();

            #region FilterType is Property

            if (requestModel.FilterType == Convert.ToString(EnumFilerType.Property))
            {
                lst = await _dapperService.QueryAsync<InquiryDataModel>(
                    CommonQuery.GetFilterPropertyInquiry,
                    parameters,
                    CommandType.StoredProcedure
                );

                //var (dataLst, totalCount) = await _dapperService.QueryMultipleAsync<InquiryDataModel>(
                //    CommonQuery.GetFilterPropertyInquiry,
                //    parameters,
                //    CommandType.StoredProcedure
                //    );

                var totalCount = await _dapperService.GetTotalCountAsync(
                    CommonQuery.PropertyInquiryCountResult,
                    new { requestModel.Status, requestModel.InquiryStatus },
                    CommandType.StoredProcedure
                );

                var pageCount = totalCount / requestModel.PageSize;
                if (totalCount % requestModel.PageSize > 0)
                {
                    pageCount++;
                }
                var pageSettingModel = new PageSettingModel(
                    requestModel.PageNo,
                    requestModel.PageSize,
                    pageCount,
                    totalCount
                );

                var model = new InquiryListResponseModelV2(lst, pageSettingModel);
                responseModel = Result<InquiryListResponseModelV2>.SuccessResult(model);

                goto result;
            }

            #endregion

            #region FilterType is Car

            if (requestModel.FilterType == Convert.ToString(EnumFilerType.Car))
            {
                #region Car / Unread

                if (requestModel.InquiryStatus == Convert.ToString(EnumInquiryStatus.Unread))
                {
                    var query = _context.TblInquires.Where(x =>
                        !x.IsDone && !x.IsDeleted && x.CarId != null
                    );
                    responseModel = await GetInquiryListResultV2(
                        query,
                        requestModel.PageNo,
                        requestModel.PageSize
                    );

                    goto result;
                }
                #endregion

                #region Car / Done

                else if (requestModel.InquiryStatus == Convert.ToString(EnumInquiryStatus.Done))
                {
                    var query = _context.TblInquires.Where(x =>
                        x.IsDone && !x.IsDeleted && x.CarId != null
                    );

                    responseModel = await GetInquiryListResultV2(
                        query,
                        requestModel.PageNo,
                        requestModel.PageSize
                    );
                    goto result;
                }
                #endregion

                #region Car / All

                else
                {
                    var query = _context.TblInquires.Where(x => !x.IsDeleted && x.CarId != null);

                    responseModel = await GetInquiryListResultV2(
                        query,
                        requestModel.PageNo,
                        requestModel.PageSize
                    );
                    goto result;
                }

                #endregion
            }

            #endregion

            #region Filter is Other

            if (requestModel.FilterType == Convert.ToString(EnumFilerType.Other))
            {
                #region Other / Unread

                if (requestModel.InquiryStatus == Convert.ToString(EnumInquiryStatus.Unread))
                {
                    var query = _context.TblInquires.Where(x =>
                        x.PropertyId == null && x.CarId == null && !x.IsDone && !x.IsDeleted
                    );

                    responseModel = await GetInquiryListResultV2(
                        query,
                        requestModel.PageNo,
                        requestModel.PageSize
                    );
                    goto result;
                }
                #endregion

                #region Other / Done

                else if (requestModel.InquiryStatus == Convert.ToString(EnumInquiryStatus.Done))
                {
                    var query = _context.TblInquires.Where(x =>
                        x.PropertyId == null && x.CarId == null && x.IsDone && !x.IsDeleted
                    );

                    responseModel = await GetInquiryListResultV2(
                        query,
                        requestModel.PageNo,
                        requestModel.PageSize
                    );
                    goto result;
                }
                #endregion

                #region Other / All

                else
                {
                    var query = _context.TblInquires.Where(x =>
                        x.PropertyId == null && x.CarId == null && !x.IsDeleted
                    );

                    responseModel = await GetInquiryListResultV2(
                        query,
                        requestModel.PageNo,
                        requestModel.PageSize
                    );
                    goto result;
                }

                #endregion
            }

            #endregion

            responseModel = Result<InquiryListResponseModelV2>.SuccessResult();
        }
        catch (Exception ex)
        {
            responseModel = Result<InquiryListResponseModelV2>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Create Inquiry

    public async Task<Result<InquiryResponseModel>> CreateInquiry(InquiryRequestModel requestModel)
    {
        Result<InquiryResponseModel> responseModel;
        try
        {
            #region Check Property Valid

            if (requestModel.PropertyId is not null && !requestModel.PropertyId.IsNullOrEmpty())
            {
                var property = await _context.TblProperties.FirstOrDefaultAsync(x =>
                    x.PropertyId == requestModel.PropertyId && !x.IsDeleted
                );
                if (property is null)
                {
                    responseModel = Result<InquiryResponseModel>.FailureResult(
                        "Property Not Found.",
                        EnumStatusCode.NotFound
                    );
                    goto result;
                }
            }

            #endregion

            await _context.TblInquires.AddAsync(requestModel.Map());
            int result = await _context.SaveChangesAsync();

            responseModel = Result<InquiryResponseModel>.ExecuteResult(result);
        }
        catch (Exception ex)
        {
            responseModel = Result<InquiryResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Update Inquiry

    public async Task<Result<InquiryResponseModel>> UpdateInquiry(
        UpdateInquiryRequestModel requestModel,
        string id
    )
    {
        Result<InquiryResponseModel> responseModel;
        try
        {
            var item = await GetActiveInquiryById(id);
            if (item is null)
            {
                responseModel = Result<InquiryResponseModel>.FailureResult(
                    MessageResource.NotFound,
                    EnumStatusCode.NotFound
                );
                goto result;
            }

            item.UpdatedDate = DevCode.GetCurrentMyanmarDateTime();
            item.UserName = requestModel.UserName;
            item.PhoneNumber = requestModel.PhoneNumber;
            item.Email = requestModel.Email;
            item.Description = requestModel.Description;

            _context.TblInquires.Update(item);
            int result = await _context.SaveChangesAsync();

            responseModel = Result<InquiryResponseModel>.ExecuteResult(result);
        }
        catch (Exception ex)
        {
            responseModel = Result<InquiryResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Patch Inquiry

    public async Task<Result<InquiryResponseModel>> PatchInquiry(string id)
    {
        Result<InquiryResponseModel> responseModel;
        try
        {
            var item = await GetActiveInquiryById(id);
            if (item is null)
            {
                responseModel = Result<InquiryResponseModel>.FailureResult(
                    MessageResource.NotFound,
                    EnumStatusCode.NotFound
                );
                goto result;
            }

            item.IsDone = !item.IsDone;

            _context.TblInquires.Update(item);
            int result = await _context.SaveChangesAsync();

            responseModel = Result<InquiryResponseModel>.ExecuteResult(result);
        }
        catch (Exception ex)
        {
            responseModel = Result<InquiryResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Delete Inquiry

    public async Task<Result<InquiryResponseModel>> DeleteInquiry(string id)
    {
        Result<InquiryResponseModel> responseModel;
        try
        {
            var item = await GetActiveInquiryById(id);
            if (item is null)
            {
                responseModel = Result<InquiryResponseModel>.FailureResult(
                    MessageResource.NotFound,
                    EnumStatusCode.NotFound
                );
                goto result;
            }

            item.IsDeleted = true;

            _context.TblInquires.Update(item);
            int result = await _context.SaveChangesAsync();

            responseModel = Result<InquiryResponseModel>.ExecuteResult(result);
        }
        catch (Exception ex)
        {
            responseModel = Result<InquiryResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion


    #region Get Active Inquiry ById

    private async Task<TblInquire?> GetActiveInquiryById(string id)
    {
        return await _context.TblInquires.FirstOrDefaultAsync(x =>
            x.InquiresId == id && !x.IsDeleted
        );
    }

    #endregion

    #region Get Car UnRead List

    private async Task<List<TblInquire>> GetCarUnReadList() =>
        await _context
            .TblInquires.Where(x => !x.IsDone && !x.IsDeleted && x.CarId != null)
            .ToListAsync();

    #endregion

    #region Get Car Done List

    private async Task<List<TblInquire>> GetCarDoneList() =>
        await _context
            .TblInquires.Where(x => x.IsDone && !x.IsDeleted && x.CarId != null)
            .ToListAsync();

    #endregion

    #region Get Car List

    private async Task<List<TblInquire>> GetCarList() =>
        await _context.TblInquires.Where(x => !x.IsDeleted && x.CarId != null).ToListAsync();

    #endregion

    #region Get Other UnRead List

    private async Task<List<TblInquire>> GetOtherUnReadList() =>
        await _context
            .TblInquires.Where(x =>
                x.PropertyId == null && x.CarId == null && !x.IsDone && !x.IsDeleted
            )
            .ToListAsync();

    #endregion

    #region Get Other Done List

    private async Task<List<TblInquire>> GetOtherDoneList() =>
        await _context
            .TblInquires.Where(x =>
                x.PropertyId == null && x.CarId == null && x.IsDone && !x.IsDeleted
            )
            .ToListAsync();

    #endregion

    #region Get Other List

    private async Task<List<TblInquire>> GetOtherList() =>
        await _context
            .TblInquires.Where(x => x.PropertyId == null && x.CarId == null && !x.IsDeleted)
            .ToListAsync();

    #endregion

    private async Task<Result<InquiryListResponseModelV2>> GetInquiryListResultV2(
        IQueryable<TblInquire> query,
        int pageNo,
        int pageSize
    )
    {
        var totalCount = await query.CountAsync();
        var pageCount = totalCount / pageSize;

        if (totalCount % pageSize > 0)
        {
            pageCount++;
        }

        var dataLst = await query.ToListAsync();
        var lst = dataLst.Select(x => x.Change()).ToList();

        var pageSettingModel = new PageSettingModel(pageNo, pageSize, pageCount, totalCount);
        var model = new InquiryListResponseModelV2(lst, pageSettingModel);

        return Result<InquiryListResponseModelV2>.SuccessResult(model);
    }
}
