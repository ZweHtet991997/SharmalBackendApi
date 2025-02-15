using System;
using System.Collections.Generic;

namespace SharmalRealEstateSystem.DbService.AppDbContexts;

public partial class TblImage
{
    public string ImageId { get; set; } = null!;

    public string? PropertyId { get; set; }

    public string? CarId { get; set; }

    public string? AdsId { get; set; }

    public string ImageName { get; set; } = null!;
}
