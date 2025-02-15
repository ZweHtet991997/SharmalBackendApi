namespace SharmalRealEstateSystem.Models.Features.Admin.Property;

public record PropertyDataModel(
    PropertyModel Property,
    List<PropertyFeatureModel> PropertyFeatures,
    List<PropertyImageDataModel> Images
);
