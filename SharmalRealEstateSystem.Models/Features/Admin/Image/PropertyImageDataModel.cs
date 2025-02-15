namespace SharmalRealEstateSystem.Models.Features.Admin.Image;

public class PropertyImageDataModel
{
    public string ImageId { get; set; } = null!;
    public string? PropertyId { get; set; }
    public string ImageName { get; set; } = null!;
    public string CreatedBy { get; set; } = null!;
}
