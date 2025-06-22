using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using eep_backend.Models.UserModuleModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class JwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);
        
        var expiresInMinutes = int.TryParse(jwtSettings["ExpiresInMinutes"], out var minutes) ? minutes : 60;
        var expires = DateTime.UtcNow.AddMinutes(expiresInMinutes);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Login ?? ""),
            new Claim(ClaimTypes.Name, user.Login ?? ""),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        if (user.RoleId.HasValue)
        {
            claims.Add(new Claim(ClaimTypes.Role, user.RoleId.Value.ToString()));
        }

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}