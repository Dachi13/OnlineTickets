namespace Auth.Api.Auth;

public static class JwtConfigurationHelper
{
    static IConfiguration _configuration = new ConfigurationBuilder().
        AddJsonFile("appsettings.json")
        .Build();

    public static TokenValidationParameters GetTokenValidationParameters(string secretKey)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _configuration["Jwt:Issuer"],
            ValidAudience = _configuration["Jwt:Audience"],
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };

        return tokenValidationParameters;
    }
}