using System;
using System.Collections.Generic;

namespace SharmalRealEstateSystem.DbService.AppDbContexts;

public partial class TblPropertyFeature
{
    public string PropertyFeatureId { get; set; } = null!;

    public string PropertyId { get; set; } = null!;

    public string FeatureId { get; set; } = null!;

    public virtual TblFeature Feature { get; set; } = null!;
}
