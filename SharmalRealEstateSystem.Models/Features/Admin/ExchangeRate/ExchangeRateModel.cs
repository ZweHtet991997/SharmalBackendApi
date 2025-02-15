namespace SharmalRealEstateSystem.Models.Features.Admin.ExchangeRate;

public class ExchangeRateModel
{
    public int ExchangeRateId { get; set; }

    public string? Currency { get; set; }

    public decimal? ExchangeRate { get; set; }

    public string? UpdatedDate { get; set; }
}
