using System;
using System.Collections.Generic;

namespace SharmalRealEstateSystem.DbService.AppDbContexts;

public partial class TblAdsPage
{
    public string AdsPageId { get; set; } = null!;

    public string? Pages { get; set; }

    public bool IsDeleted { get; set; }
}
