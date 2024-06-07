using System.Diagnostics.CodeAnalysis;
using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Common.WWW.Infrastructure.HealthChecks.Connectors;

public sealed class RedisHealthCheckConnector : IHealthCheck
{
    private readonly ICacheService _cacheConfig;

    private readonly ILogger<RedisHealthCheckConnector> _logger;

    public RedisHealthCheckConnector(ICacheService cacheConfig, ILogger<RedisHealthCheckConnector> logger)
    {
        _cacheConfig = cacheConfig;
        _logger = logger;
    }

    [SuppressMessage("Design", "CA1031: Do not catch general exception types", Justification = "We need to catch all exceptions from Cache service in Health Check")]
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            _cacheConfig.GetValue<string>("health-check-key");
            return Task.FromResult(HealthCheckResult.Healthy());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{nameof(RedisHealthCheckConnector)} failed");
            return Task.FromResult(HealthCheckResult.Unhealthy("Cache service connection is not available"));
        }
    }
}
