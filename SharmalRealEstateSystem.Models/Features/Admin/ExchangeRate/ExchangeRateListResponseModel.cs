namespace SharmalRealEstateSystem.Models.Features.Admin.ExchangeRate;

public class ExchangeRateListResponseModel
{
    public List<ExchangeRateDataModel> DataLst { get; set; }
}

public class ExchangeRateDataModel
{
    public int ExchangeRateId { get; set; }

    public string? Currency { get; set; }

    public decimal? ExchangeRate { get; set; }
    public string Image { get; set; }
    public string? UpdatedDate { get; set; }
}
