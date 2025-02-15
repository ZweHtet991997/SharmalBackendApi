namespace SharmalRealEstateSystem.Models.Features.Admin.ExchangeRate;

public class ExchangeRateRequestModel
{
    public string? Currency { get; set; }

    public decimal? ExchangeRate { get; set; }
}

public class UpdateExchangeRateRequestModel
{
    public List<UpdateExchangeRateDataModel>? ExchangeRates { get; set; }
}

public class UpdateExchangeRateDataModel
{
    public int ExchangeRateId { get; set; }

    public string? Currency { get; set; }

    public decimal? ExchangeRate { get; set; }
}
