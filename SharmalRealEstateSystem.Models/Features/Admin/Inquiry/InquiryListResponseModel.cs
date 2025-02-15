namespace SharmalRealEstateSystem.Models.Features.Admin.Inquiry;

public record InquiryListResponseModel(List<InquiryDataModel> DataLst);
public record InquiryListResponseModelV2(List<InquiryDataModel> DataLst, PageSettingModel PageSetting);
