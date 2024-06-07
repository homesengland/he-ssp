using System.Diagnostics.CodeAnalysis;
using HE.Investments.Common.Infrastructure.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Common.WWW.Infrastructure.HealthChecks.Connectors;

public sealed class CrmHealthCheckConnector : IHealthCheck
{
    private readonly ICrmConnectionTester _crmConnectionTester;

    private readonly ILogger<CrmHealthCheckConnector> _logger;

    public CrmHealthCheckConnector(ICrmConnectionTester crmConnectionTester, ILogger<CrmHealthCheckConnector> logger)
    {
        _crmConnectionTester = crmConnectionTester;
        _logger = logger;
    }

    [SuppressMessage("Design", "CA1031: Do not catch general exception types", Justification = "We need to catch all exceptions from CRM in Health Check")]
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await _crmConnectionTester.TestCrmConnection(cancellationToken);
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{nameof(CrmHealthCheckConnector)} failed");
            return HealthCheckResult.Unhealthy("CRM connection not available");
        }
    }
}
