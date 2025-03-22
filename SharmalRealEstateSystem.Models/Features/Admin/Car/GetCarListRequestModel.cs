namespace SharmalRealEstateSystem.Models.Features.Admin.Car;

public class GetCarListRequestModel
{
    public int PageNo { get; set; }
    public int PageSize { get; set; }
    public string? Location { get; set; }
    public string? City { get; set; }
    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
    public string? EnginePower { get; set; }
    public string? Gearbox { get; set; }
    public string? SteerPosition { get; set; }
    public string? Color { get; set; }
    public string? Code { get; set; }
    public string? Condition { get; set; }
    public string? MinYear { get; set; }
    public string? MaxYear { get; set; }
    public string? FuelType { get; set; }
    public string? IsPopular { get; set; }
    public string? IsHotDeal { get; set; }
    public int? MinPrice { get; set; }
    public int? MaxPrice { get; set; }
    public int? MinMileAge { get; set; }
    public int? MaxMileAge { get; set; }
    public string? BuildType { get; set; }
}
