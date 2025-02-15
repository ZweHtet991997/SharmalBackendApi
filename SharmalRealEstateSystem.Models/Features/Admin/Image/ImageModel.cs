namespace SharmalRealEstateSystem.Models.Features.Admin.Image;

public class ImageModel
{
    public string ImageId { get; set; } = null!;

    public string? PropertyId { get; set; }

    public string? CarId { get; set; }

    public string? AdsId { get; set; }

    public string ImageName { get; set; } = null!;
}
