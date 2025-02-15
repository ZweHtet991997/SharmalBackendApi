using SharmalRealEstateSystem.Shared.Configs;

namespace SharmalRealEstateSystem.Shared.Services.AuthServices;

public class TokenValidationService
{
    private readonly IConfiguration _configuration;

    public TokenValidationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ClaimsPrincipal ValidateToken(string token)
    {
        try
        {
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(Deployment.IsDevelopment()
                ? JWTConfig.UATJWTKey
                : JWTConfig.JWTKey);

            TokenValidationParameters parameters =
                new()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidAudience = Deployment.IsDevelopment() ? JWTConfig.UATAudience : JWTConfig.Audience,
                    ValidIssuer = Deployment.IsDevelopment() ? JWTConfig.UATIssuer : JWTConfig.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            ClaimsPrincipal principal = tokenHandler.ValidateToken(
                token,
                parameters,
                out SecurityToken securityToken
            );

            return principal;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
