using System.Security.Claims;
using NoteAppApi.Common;

namespace NoteAppApi.Infrastructure.DependencyInjection;

public static class AuthDataRegistration
{
    public static IServiceCollection AddAuthData(this IServiceCollection services)
    {
        services.AddScoped(provider =>
        {
            var contextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
            var userIdClaim = contextAccessor.HttpContext?.User.FindFirst("userId")?.Value;

            Console.WriteLine(userIdClaim);
            if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedAccessException("User is not authenticated.");

            return new AuthData(userId);
        });

        return services;
    }
}
