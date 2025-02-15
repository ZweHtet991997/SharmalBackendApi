namespace SharmalRealEstateSystem.Models.Features.Jwt;

public class JwtResponseModel
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string UserRole { get; set; }

    public JwtResponseModel(string userId, string userName, string email, string userRole)
    {
        UserName = userName;
        Email = email;
        UserRole = userRole;
        UserId = userId;
    }

    public JwtResponseModel()
    {
    }
}
