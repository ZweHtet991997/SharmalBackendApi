using System;
using System.Collections.Generic;

namespace SharmalRealEstateSystem.DbService.AppDbContexts;

public partial class TblAdsPagePlacement
{
    public string AdsPagePlacementId { get; set; } = null!;

    public string? AdsId { get; set; }

    public string? AdsPageId { get; set; }
}
