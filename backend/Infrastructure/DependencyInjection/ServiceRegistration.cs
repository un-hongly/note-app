using NoteAppApi.Infrastructure.Security;
using NoteAppApi.Services;

namespace NoteAppApi.Infrastructure.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<TokenService>();
        services.AddScoped<AuthService>();
        services.AddScoped<NoteService>();

        return services;
    }
}
