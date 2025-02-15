using System;
using System.Collections.Generic;

namespace SharmalRealEstateSystem.DbService.AppDbContexts;

public partial class TblMemberPlan
{
    public int MemberPlanId { get; set; }

    public string MemberPlan { get; set; } = null!;

    public string ActiveDate { get; set; } = null!;

    public string ExpireDate { get; set; } = null!;

    public bool IsExtend { get; set; }

    public bool IsActive { get; set; }
}
