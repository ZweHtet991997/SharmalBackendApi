namespace SharmalRealEstateSystem.Models.Features.Admin.Auth;

public class RegisterRequestModel
{
    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string UserRole { get; set; } = null!;

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }
}
