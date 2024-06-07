using HE.Investments.Common.Infrastructure.Cache.Config;
using HealthChecks.Redis;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HE.Investments.Common.WWW.Infrastructure.HealthChecks.Connectors;

public sealed class RedisHealthCheckConnector : IHealthCheck
{
    private readonly ICacheConfig _cacheConfig;

    public RedisHealthCheckConnector(ICacheConfig cacheConfig)
    {
        _cacheConfig = cacheConfig;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(_cacheConfig.RedisConnectionString) || _cacheConfig.RedisConnectionString == "off")
        {
            return HealthCheckResult.Healthy("Redis is disabled.");
        }

        return await new RedisHealthCheck(_cacheConfig.RedisConnectionString).CheckHealthAsync(context, cancellationToken);
    }
}
