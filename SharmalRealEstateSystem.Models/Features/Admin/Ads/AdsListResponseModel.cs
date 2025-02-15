using SharmalRealEstateSystem.Models.Features.Admin.AdsPagePlacement;

namespace SharmalRealEstateSystem.Models.Features.Admin.Ads;

public record AdsListResponseModel(List<AdsDataModel> DataLst, PageSettingModel PageSetting);
