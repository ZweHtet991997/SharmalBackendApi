namespace SharmalRealEstateSystem.Models.Features.Admin.Ads;

public class AdsRequestModel
{
    public string Title { get; set; } = null!;
    public string AdsCode { get; set; } = null!;

    public string TargetUrl { get; set; } = null!;

    public string? AdsLayout { get; set; }

    public string StartDate { get; set; } = null!;

    public string EndDate { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public List<AdsPagePlacementRequestModel>? AdsPagePlacements { get; set; }

    public List<IFormFile>? Files { get; set; }
}

public class UpdateAdsRequestModel
{
    public string Title { get; set; } = null!;
    public string AdsCode { get; set; } = null!;

    public string TargetUrl { get; set; } = null!;

    public string? AdsLayout { get; set; }

    public string StartDate { get; set; } = null!;

    public string EndDate { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;

    public List<AdsPagePlacementRequestModel>? AdsPagePlacements { get; set; }
    public List<IFormFile>? Files { get; set; }
    public List<UpdateAdsRemoveFileRequestModel>? RemoveFiles { get; set; }
}

public class AdsPagePlacementRequestModel
{
    public string AdsPageId { get; set; }
}

public class UpdateAdsRemoveFileRequestModel
{
    public string ImageId { get; set; }
}
