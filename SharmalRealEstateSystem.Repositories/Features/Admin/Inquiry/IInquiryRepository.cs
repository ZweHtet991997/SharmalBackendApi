namespace SharmalRealEstateSystem.Repositories.Features.Admin.Inquiry;

public interface IInquiryRepository
{
    Task<Result<InquiryListResponseModel>> GetInquiryList(
        GetFilterInquiryRequestModel requestModel
    );
    Task<Result<InquiryListResponseModel>> GetInquiryListV1(
        GetFilterInquiryRequestModel requestModel
    );
    Task<Result<InquiryListResponseModelV2>> GetInquiryListV2(
        GetFilterInquiryRequestModel requestModel
    );
    Task<Result<InquiryListResponseModelV2>> GetInquiryListV3(
        GetFilterInquiryRequestModel requestModel
    );
    Task<Result<InquiryResponseModel>> CreateInquiry(InquiryRequestModel requestModel);
    Task<Result<InquiryResponseModel>> UpdateInquiry(
        UpdateInquiryRequestModel requestModel,
        string id
    );
    Task<Result<InquiryResponseModel>> PatchInquiry(string id);
    Task<Result<InquiryResponseModel>> DeleteInquiry(string id);
}
