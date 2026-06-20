using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InvoiceApp.Application.Interfaces;
using InvoiceApp.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace InvoiceApp.Infrastructure.Services;

public class JwtTokenService : ITokenService
{
    private readonly string _secret;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expiryHours;

    public JwtTokenService(IConfiguration configuration)
    {
        var section = configuration.GetSection("JwtSettings");
        _secret = section["Secret"]
            ?? throw new InvalidOperationException("JwtSettings:Secret no configurado.");
        _issuer = section["Issuer"] ?? "InvoiceApp";
        _audience = section["Audience"] ?? "InvoiceAppClient";
        _expiryHours = int.TryParse(section["ExpiryHours"], out var h) ? h : 8;
    }

    public string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: GetExpiry(),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public DateTime GetExpiry() => DateTime.UtcNow.AddHours(_expiryHours);
}
