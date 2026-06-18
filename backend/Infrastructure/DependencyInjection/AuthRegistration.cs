using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NoteAppApi.Infrastructure.Configuration;

namespace NoteAppApi.Infrastructure.DependencyInjection;

public static class AuthRegistration
{
    public static IServiceCollection AddJwtAuth(
        this IServiceCollection services,
        IConfiguration config)
    {
        var jwt = config.GetSection("Jwt").Get<JwtSettings>();

        services.Configure<JwtSettings>(config.GetSection("Jwt"));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt.Issuer,
                    ValidAudience = null,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwt.Key))
                };
            });

        services.AddAuthorization();

        return services;
    }
}
