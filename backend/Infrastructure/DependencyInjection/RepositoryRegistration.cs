using NoteAppApi.Infrastructure.Persistence;
using NoteAppApi.Repositories;

namespace NoteAppApi.Infrastructure.DependencyInjection;

public static class RepositoryRegistration
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<DapperContext>();
        services.AddScoped<UserRepository>();
        services.AddScoped<NoteRepository>();
        return services;
    }
}
