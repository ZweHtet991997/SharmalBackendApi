namespace SharmalRealEstateSystem.Models.Features.Admin.Auth;

public class UserModel
{
    public string UserId { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    [JsonIgnore]
    public string Password { get; set; } = null!;

    public string UserRole { get; set; } = null!;

    public string CreatedDate { get; set; } = null!;

    [JsonIgnore]
    public bool IsDeleted { get; set; }

    public int? FailedCount { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }
}
