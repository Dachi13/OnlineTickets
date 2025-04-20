using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.BearerToken;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services
    .AddAuthentication(BearerTokenDefaults.AuthenticationScheme)
    .AddBearerToken();

var app = builder.Build();

app.MapGet("/login", () =>
{
    var claims = new List<Claim>
    {
        new Claim("sub", Guid.NewGuid().ToString())
    };

    var identity = new ClaimsIdentity(claims, BearerTokenDefaults.AuthenticationScheme);
    var principal = new ClaimsPrincipal(identity);

    return Results.SignIn(principal, authenticationScheme: BearerTokenDefaults.AuthenticationScheme);
});

app.UseAuthentication();

app.UseAuthorization();

app.MapReverseProxy();

app.Run();