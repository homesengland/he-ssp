using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HE.Investments.Common.WWW.Infrastructure.HealthChecks;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHeHealthChecks(this IServiceCollection services, params Type[] healthChecks)
    {
        var builder = services.AddHealthChecks();

        foreach (var healthCheckType in healthChecks)
        {
            if (!typeof(IHealthCheck).IsAssignableFrom(healthCheckType))
            {
                throw new ArgumentException($"{healthCheckType.Name} must implement {nameof(IHealthCheck)}");
            }

            builder.Add(new HealthCheckRegistration(
                healthCheckType.Name,
                x => (IHealthCheck)x.GetRequiredService(healthCheckType),
                HealthStatus.Unhealthy,
                null));
        }

        return services.RegisterHealthChecks(healthChecks);
    }

    private static IServiceCollection RegisterHealthChecks(this IServiceCollection services, Type[] healthChecks)
    {
        foreach (var healthCheckType in healthChecks)
        {
            services.TryAddScoped(healthCheckType);
        }

        return services;
    }
}
