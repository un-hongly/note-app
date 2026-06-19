using System.Threading.RateLimiting;

namespace NoteAppApi.Infrastructure.DependencyInjection;

public static class RateLimitRegistration
{
    public static IServiceCollection AddRateLimiting(this IServiceCollection services, IConfiguration config)
    {
        var permitLimit = config.GetValue<int>("RateLimiting:Limit", 100);
        var windowMinutes = config.GetValue<int>("RateLimiting:WindowMinutes", 1);

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = permitLimit,
                        Window = TimeSpan.FromMinutes(windowMinutes),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    }));
        });

        return services;
    }
}
