using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NoteAppApi.Entities;
using NoteAppApi.Infrastructure.Configuration;

namespace NoteAppApi.Infrastructure.Security;

public class TokenService
{
    private readonly JwtSettings _jwt;

    public TokenService(IOptions<JwtSettings> jwt)
    {
        _jwt = jwt.Value;
    }

    public string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim("userId", user.Id.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.ExpireMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
