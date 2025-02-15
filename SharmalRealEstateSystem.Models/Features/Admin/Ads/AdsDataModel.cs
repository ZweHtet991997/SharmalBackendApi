using SharmalRealEstateSystem.Models.Features.Admin.AdsPagePlacement;

namespace SharmalRealEstateSystem.Models.Features.Admin.Ads;

public class AdsDataModel
{
    public AdsModel Ads { get; set; }
    public List<AdsPagePlacementModel> AdsPagePlacements { get; set; }
    public List<AdsImageModel> Images { get; set; }
}
