namespace SharmalRealEstateSystem.Models.Features.Admin.Property;

public class UpdatePropertyRequestModel
{
    public string Title { get; set; } = null!;
    public string Code { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Type { get; set; } = null!;

    public int Price { get; set; }

    public string PaymentOption { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string City { get; set; } = null!;

    public string NumberOfViewers { get; set; } = null!;

    public int Bedrooms { get; set; }

    public string Area { get; set; } = null!;

    public string Condition { get; set; } = null!;

    public string? Floor { get; set; }

    public string Description { get; set; } = null!;

    public string Furnished { get; set; } = null!;

    public string? MapUrl { get; set; }

    public string SellerName { get; set; } = null!;

    public string PrimaryPhoneNumber { get; set; } = null!;

    public string? SecondaryPhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string UpdatedBy { get; set; }
    public string CreatedBy { get; set; }

    public bool? IsPopular { get; set; }

    public bool? IsHotDeal { get; set; }

    public List<PropertyFeatureRequestModel>? Features { get; set; }
    public List<RemoveImageRequestModel>? RemoveImages { get; set; }
    public List<IFormFile>? Files { get; set; }
}
