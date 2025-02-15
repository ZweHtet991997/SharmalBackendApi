namespace SharmalRealEstateSystem.Models.Features.Admin.PropertyFeature;

public class PropertyFeatureModel
{
    public string PropertyFeatureId { get; set; } = null!;

    public string? PropertyId { get; set; }

    public string? FeatureId { get; set; }
    public string? FeatureName { get; set; }
}
