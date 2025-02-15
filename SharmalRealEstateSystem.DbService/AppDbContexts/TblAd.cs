using System;
using System.Collections.Generic;

namespace SharmalRealEstateSystem.DbService.AppDbContexts;

public partial class TblAd
{
    public string AdsId { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string TargetUrl { get; set; } = null!;

    public string? AdsLayout { get; set; }

    public string StartDate { get; set; } = null!;

    public string EndDate { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public string? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }
}
