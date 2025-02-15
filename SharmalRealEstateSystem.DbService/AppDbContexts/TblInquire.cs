using System;
using System.Collections.Generic;

namespace SharmalRealEstateSystem.DbService.AppDbContexts;

public partial class TblInquire
{
    public string InquiresId { get; set; } = null!;

    public string? PropertyId { get; set; }

    public string? CarId { get; set; }

    public string? CreatedDate { get; set; }

    public string? UpdatedDate { get; set; }

    public string UserName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool IsDone { get; set; }

    public bool IsDeleted { get; set; }
}
