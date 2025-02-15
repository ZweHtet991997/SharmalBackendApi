namespace SharmalRealEstateSystem.Models.Features.Admin.Inquiry;

public class InquiryRequestModel
{
    public string? PropertyId { get; set; }

    public string? CarId { get; set; }

    public string UserName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Description { get; set; } = null!;
}

public class UpdateInquiryRequestModel
{
    public string UserName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Description { get; set; } = null!;
}
