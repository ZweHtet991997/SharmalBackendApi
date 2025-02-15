using System;
using System.Collections.Generic;

namespace SharmalRealEstateSystem.DbService.AppDbContexts;

public partial class TblFeature
{
    public string FeatureId { get; set; } = null!;

    public string? Name { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<TblPropertyFeature> TblPropertyFeatures { get; set; } = new List<TblPropertyFeature>();
}
