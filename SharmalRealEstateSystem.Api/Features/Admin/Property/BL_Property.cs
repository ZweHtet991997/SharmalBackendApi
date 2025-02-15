using SharmalRealEstateSystem.Models.Resources;

namespace SharmalRealEstateSystem.Api.Features.Admin.Property;

public class BL_Property
{
    #region Initializations

    private readonly IAdminUnitOfWork _adminUnitOfWork;

    public BL_Property(IAdminUnitOfWork adminUnitOfWork)
    {
        _adminUnitOfWork = adminUnitOfWork;
    }

    #endregion

    #region Get List

    public async Task<Result<PropertyListResponseModel>> GetList(
        GetPropertyListRequestModel requestModel
    )
    {
        Result<PropertyListResponseModel> responseModel;
        try
        {
            if (requestModel.PageNo <= 0)
            {
                responseModel = GetInvalidPageNoResult();
                goto result;
            }

            if (requestModel.PageSize <= 0)
            {
                responseModel = GetInvalidPageSizeResult();
                goto result;
            }

            responseModel = await _adminUnitOfWork.PropertyRepository.GetList(requestModel);
        }
        catch (Exception ex)
        {
            responseModel = Result<PropertyListResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region GetPropertyByPropertyId
    public async Task<Result<PropertyDataModel>> GetPropertyByPropertyId(string id)
    {
        Result<PropertyDataModel> responseModel;
        try
        {
            if (id.IsNullOrEmpty())
            {
                responseModel = Result<PropertyDataModel>.FailureResult(MessageResource.InvalidId);
                goto result;
            }

            responseModel = await _adminUnitOfWork.PropertyRepository.GetPropertyByPropertyId(id);
        }
        catch (Exception ex)
        {
            responseModel = Result<PropertyDataModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }
    #endregion


    #region GetListByUserId
    public async Task<Result<PropertyListResponseModel>> GetListByUserId(string userId, int pageNo, int pageSize)
    {
        Result<PropertyListResponseModel> responseModel;
        try
        {
            if (pageNo <= 0)
            {
                responseModel = GetInvalidPageNoResult();
                goto result;
            }

            if (pageSize <= 0)
            {
                responseModel = GetInvalidPageSizeResult();
                goto result;
            }

            if (userId.IsNullOrEmpty())
            {
                responseModel = Result<PropertyListResponseModel>.FailureResult(MessageResource.InvalidId);
                goto result;
            }

            responseModel = await _adminUnitOfWork.PropertyRepository.GetListByUserId(userId, pageNo, pageSize);
        }
        catch (Exception ex)
        {
            responseModel = Result<PropertyListResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Create Property

    public async Task<Result<PropertyResponseModel>> CreateProperty(
        PropertyRequestModel requestModel
    )
    {
        Result<PropertyResponseModel> responseModel;
        try
        {
            #region Files Validation

            if (requestModel.Files is null || requestModel.Files.Count <= 0)
            {
                responseModel = Result<PropertyResponseModel>.FailureResult("Invalid Files.");
                goto result;
            }

            #endregion

            #region If not null, Secondary Phone Number Validation

            if (!requestModel.SecondaryPhoneNumber.IsNullOrEmpty())
            {
                if (requestModel.SecondaryPhoneNumber.Length != 11)
                {
                    responseModel = GetInvalidSecondaryPhoneNumberResult();
                    goto result;
                }
            }

            #endregion

            #region Enum Validation

            //if (!Enum.IsDefined(typeof(EnumPropertyStatus), requestModel.Status))
            //{
            //    responseModel = GetInvalidPropertyStatusResult();
            //    goto result;
            //}

            //if (!Enum.IsDefined(typeof(EnumPropertyType), requestModel.Type))
            //{
            //    responseModel = GetInvalidPropertyTypeResult();
            //    goto result;
            //}

            //if (!requestModel.Condition.IsNullOrEmpty())
            //{
            //    if (!Enum.IsDefined(typeof(EnumPropertyCondition), requestModel.Condition))
            //    {
            //        responseModel = GetInvalidPropertyCondition();
            //        goto result;
            //    }
            //}

            //if (!requestModel.Furnished.IsNullOrEmpty())
            //{
            //    if (!Enum.IsDefined(typeof(EnumPropertyFurnishType), requestModel.Furnished))
            //    {
            //        responseModel = GetInvalidPropertyFurnishType();
            //        goto result;
            //    }
            //}


            #endregion

            if (!requestModel.Furnished!.IsNullOrEmpty())
            {
                var furnishTypeList = FurnishTypeList.furnishTypeList;
                if (!furnishTypeList.Contains(requestModel.Furnished!))
                {
                    responseModel = GetInvalidPropertyFurnishType();
                    goto result;
                }
            }

            #endregion

            responseModel = await _adminUnitOfWork.PropertyRepository.CreatePropertyV1(
                requestModel
            );
        }
        catch (Exception ex)
        {
            responseModel = Result<PropertyResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }


    #region Update Property

    public async Task<Result<PropertyResponseModel>> UpdateProperty(
        UpdatePropertyRequestModel requestModel,
        string propertyId
    )
    {
        Result<PropertyResponseModel> responseModel;
        try
        {
            #region Id Validation

            if (propertyId.IsNullOrEmpty())
            {
                responseModel = GetInvalidIdResult();
                goto result;
            }

            #endregion

            #region If not null, Secondary Phone Number Validation

            if (!requestModel.SecondaryPhoneNumber.IsNullOrEmpty())
            {
                if (requestModel.SecondaryPhoneNumber.Length != 11)
                {
                    responseModel = GetInvalidSecondaryPhoneNumberResult();
                    goto result;
                }
            }

            #endregion

            #region Enum Validation

            //if (!Enum.IsDefined(typeof(EnumPropertyStatus), requestModel.Status))
            //{
            //    responseModel = GetInvalidPropertyStatusResult();
            //    goto result;
            //}

            //if (!Enum.IsDefined(typeof(EnumPropertyType), requestModel.Type))
            //{
            //    responseModel = GetInvalidPropertyTypeResult();
            //    goto result;
            //}

            //if (!Enum.IsDefined(typeof(EnumPropertyCondition), requestModel.Condition))
            //{
            //    responseModel = GetInvalidPropertyCondition();
            //    goto result;
            //}

            if (!requestModel.Furnished!.IsNullOrEmpty())
            {
                var furnishTypeList = FurnishTypeList.furnishTypeList;
                if (!furnishTypeList.Contains(requestModel.Furnished!))
                {
                    responseModel = GetInvalidPropertyFurnishType();
                    goto result;
                }
            }

            #endregion

            responseModel = await _adminUnitOfWork.PropertyRepository.UpdateProperty(
                requestModel,
                propertyId
            );
        }
        catch (Exception ex)
        {
            responseModel = Result<PropertyResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    #region Delete Property

    public async Task<Result<PropertyResponseModel>> DeleteProperty(string propertyId)
    {
        Result<PropertyResponseModel> responseModel;
        try
        {
            #region Id Validation

            if (propertyId.IsNullOrEmpty())
            {
                responseModel = GetInvalidIdResult();
                goto result;
            }

            #endregion

            responseModel = await _adminUnitOfWork.PropertyRepository.DeletePropertyV1(propertyId);
        }
        catch (Exception ex)
        {
            responseModel = Result<PropertyResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    private Result<PropertyResponseModel> GetInvalidIdResult() =>
        Result<PropertyResponseModel>.FailureResult(MessageResource.InvalidId);

    private Result<PropertyResponseModel> GetInvalidPropertyStatusResult() =>
        Result<PropertyResponseModel>.FailureResult("Invalid Property Status.");

    private Result<PropertyResponseModel> GetInvalidPropertyTypeResult() =>
        Result<PropertyResponseModel>.FailureResult("Invalid Type.");

    private Result<PropertyResponseModel> GetInvalidPropertyCondition() =>
        Result<PropertyResponseModel>.FailureResult("Invalid Condition.");

    private Result<PropertyResponseModel> GetInvalidPropertyFurnishType() =>
        Result<PropertyResponseModel>.FailureResult("Invalid Furnish Type.");

    private Result<PropertyResponseModel> GetInvalidSecondaryPhoneNumberResult() =>
        Result<PropertyResponseModel>.FailureResult("Secondary Phone Number is invalid.");

    private Result<PropertyListResponseModel> GetInvalidPageNoResult() =>
        Result<PropertyListResponseModel>.FailureResult(MessageResource.InvalidPageNo);

    private Result<PropertyListResponseModel> GetInvalidPageSizeResult() =>
        Result<PropertyListResponseModel>.FailureResult(MessageResource.InvalidPageSize);
}
