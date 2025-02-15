using SharmalRealEstateSystem.Shared.Configs;

namespace SharmalRealEstateSystem.Shared.Services.AuthServices;

public class JWTAuth
{
    private readonly IConfiguration _configuration;

    public JWTAuth(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetJWTToken(JwtResponseModel jwtResponseModel)
    {
        try
        {
            var claims = new[]
            {
                //new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]!),
                new Claim(JwtRegisteredClaimNames.Sub, Deployment.IsDevelopment() ? JWTConfig.UATSubject : JWTConfig.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("UserId", jwtResponseModel.UserId),
                new Claim("UserName", jwtResponseModel.UserName),
                new Claim("Email", jwtResponseModel.Email),
                new Claim("UserRole", jwtResponseModel.UserRole),
                new Claim(ClaimTypes.Role, jwtResponseModel.UserRole)
            };

            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("Sharmal_UAT_JWT_Key")!));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Deployment.IsDevelopment()
                ? JWTConfig.UATJWTKey : JWTConfig.JWTKey));

            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                Deployment.IsDevelopment() ? JWTConfig.UATIssuer : JWTConfig.Issuer,
                Deployment.IsDevelopment() ? JWTConfig.UATAudience : JWTConfig.Audience,

                //Environment.GetEnvironmentVariable("Sharmal_UAT_Issuer"),
                //Environment.GetEnvironmentVariable("Sharmal_UAT_Audience"),

                //_configuration["Jwt:Issuer"],
                //_configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(10),
                signingCredentials: signIn
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
