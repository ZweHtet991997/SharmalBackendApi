using SharmalRealEstateSystem.Models.Enums.Inquiry;

namespace SharmalRealEstateSystem.Api.Features.Admin.Inquiry;

public class BL_Inquiry
{
    #region Initializations

    private readonly IAdminUnitOfWork _adminUnitOfWork;

    public BL_Inquiry(IAdminUnitOfWork adminUnitOfWork)
    {
        _adminUnitOfWork = adminUnitOfWork;
    }

    #endregion

    #region Get Inquiry List

    public async Task<Result<InquiryListResponseModelV2>> GetInquiryList(
        GetFilterInquiryRequestModel requestModel
    )
    {
        Result<InquiryListResponseModelV2> responseModel;
        try
        {
            #region Filter Type is mandatory

            if (requestModel.FilterType!.IsNullOrEmpty())
            {
                responseModel = GetInvalidFilterTypeResult();
                goto result;
            }

            #endregion

            #region Filter Type Checking

            if (!Enum.IsDefined(typeof(EnumFilerType), requestModel.FilterType!))
            {
                responseModel = GetInvalidFilterTypeResult();
                goto result;
            }

            #endregion

            #region Page No & Page Size

            if (requestModel.PageNo <= 0)
            {
                responseModel = Result<InquiryListResponseModelV2>.FailureResult(
                    MessageResource.InvalidPageNo
                );
                goto result;
            }

            if (requestModel.PageSize <= 0)
            {
                responseModel = Result<InquiryListResponseModelV2>.FailureResult(
                    MessageResource.InvalidPageSize
                );
                goto result;
            }

            #endregion

            #region Property/Car Status

            if (!requestModel.Status!.IsNullOrEmpty())
            {
                if (!Enum.IsDefined(typeof(EnumPropertyStatus), requestModel.Status!))
                {
                    responseModel = GetInvalidStatusResult();
                    goto result;
                }
            }
            else
            {
                requestModel.Status = null;
            }

            #endregion

            #region Inquiry Status

            if (!requestModel.InquiryStatus!.IsNullOrEmpty())
            {
                if (!Enum.IsDefined(typeof(EnumInquiryStatus), requestModel.InquiryStatus!))
                {
                    responseModel = GetInvalidInquiryStatusResult();
                    goto result;
                }
            }
            else
            {
                requestModel.InquiryStatus = null;
            }

            #endregion

            responseModel = await _adminUnitOfWork.InquiryRepository.GetInquiryListV3(requestModel);
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
            //if (requestModel.PropertyId!.IsNullOrEmpty() && requestModel.CarId!.IsNullOrEmpty())
            //{
            //    responseModel = Result<InquiryResponseModel>.FailureResult(
            //        "Car Id or Property Id is required."
            //    );
            //    goto result;
            //}

            responseModel = await _adminUnitOfWork.InquiryRepository.CreateInquiry(requestModel);
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
            if (id.IsNullOrEmpty())
            {
                responseModel = GetInvalidIdResult();
                goto result;
            }

            responseModel = await _adminUnitOfWork.InquiryRepository.UpdateInquiry(
                requestModel,
                id
            );
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
            if (id.IsNullOrEmpty())
            {
                responseModel = GetInvalidIdResult();
                goto result;
            }

            responseModel = await _adminUnitOfWork.InquiryRepository.PatchInquiry(id);
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
            if (id.IsNullOrEmpty())
            {
                responseModel = GetInvalidIdResult();
                goto result;
            }

            responseModel = await _adminUnitOfWork.InquiryRepository.DeleteInquiry(id);
        }
        catch (Exception ex)
        {
            responseModel = Result<InquiryResponseModel>.FailureResult(ex);
        }

    result:
        return responseModel;
    }

    #endregion

    private Result<InquiryResponseModel> GetInvalidIdResult() =>
        Result<InquiryResponseModel>.FailureResult(MessageResource.InvalidId);

    private Result<InquiryListResponseModelV2> GetInvalidStatusResult() =>
        Result<InquiryListResponseModelV2>.FailureResult("Invalid Status.");

    private Result<InquiryListResponseModelV2> GetInvalidInquiryStatusResult() =>
        Result<InquiryListResponseModelV2>.FailureResult("Invalid Inquiry Status.");

    private Result<InquiryListResponseModelV2> GetInvalidFilterTypeResult() =>
        Result<InquiryListResponseModelV2>.FailureResult("Invalid Filter Type.");

    private Result<InquiryResponseModel> GetInvalidInquiryTypeResult() =>
        Result<InquiryResponseModel>.FailureResult("Invalid Inquiry Type.");
}
