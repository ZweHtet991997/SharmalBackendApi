using System;
using System.Collections.Generic;

namespace SharmalRealEstateSystem.DbService.AppDbContexts;

public partial class TblProperty
{
    public string PropertyId { get; set; } = null!;

    public string Title { get; set; } = null!;
    public string? Code { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Type { get; set; } = null!;

    public int Price { get; set; }

    public string PaymentOption { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string City { get; set; } = null!;

    public string? NumberOfViewers { get; set; }

    public int? Bedrooms { get; set; }

    public int? Bathrooms { get; set; }

    public string Area { get; set; } = null!;

    public string? Condition { get; set; }

    public string? Floor { get; set; }

    public string Description { get; set; } = null!;

    public string? Furnished { get; set; }

    public string? MapUrl { get; set; }

    public string SellerName { get; set; } = null!;

    public string PrimaryPhoneNumber { get; set; } = null!;

    public string? SecondaryPhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string CreatedDate { get; set; } = null!;

    public string? UpdatedBy { get; set; }

    public string? UpdatedDate { get; set; }

    public bool IsDeleted { get; set; }

    public bool? IsPopular { get; set; }

    public bool? IsHotDeal { get; set; }

    public int? Approved { get; set; }
}
