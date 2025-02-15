namespace SharmalRealEstateSystem.Models.Features.Admin.Auth;

public class LoginRequestModel
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}
