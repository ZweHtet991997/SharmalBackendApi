using System;
using System.Collections.Generic;

namespace SharmalRealEstateSystem.DbService.AppDbContexts;

public partial class TblExchangeRate
{
    public int ExchangeRateId { get; set; }

    public string? Currency { get; set; }

    public decimal? ExchangeRate { get; set; }

    public string? UpdatedDate { get; set; }
}
