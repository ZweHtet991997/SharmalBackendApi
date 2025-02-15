namespace SharmalRealEstateSystem.Models.Features.Admin.Feature;

public class UpdateFeatureRequestModel
{
    public string? FeatureId { get; set; } = null!;
    public string? PropertyId { get; set; } = null!;
    public string? Name { get; set; }
}
