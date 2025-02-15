namespace SharmalRealEstateSystem.Models.Features.Admin.Property;

public class GetPropertyListRequestModel
{
    public int PageNo { get; set; }
    public int PageSize { get; set; }
    public string? Location { get; set; }
    public string? City { get; set; }
    public string? Furnished { get; set; }
    public string? Type { get; set; }
    public string? Code { get; set; }
    public string? PaymentOption { get; set; }
    public string? Status { get; set; }
    public int? MinPrice { get; set; }
    public int? MaxPrice { get; set; }
    public int? MinBedRooms { get; set; }
    public int? MaxBedRooms { get; set; }
    public int? MinBathRooms { get; set; }
    public int? MaxBathRooms { get; set; }
    public string? IsPopular { get; set; }
    public string? IsHotDeal { get; set; }
}
