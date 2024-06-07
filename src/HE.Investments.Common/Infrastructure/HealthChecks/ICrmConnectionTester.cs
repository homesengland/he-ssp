namespace HE.Investments.Common.Infrastructure.HealthChecks;

public interface ICrmConnectionTester
{
    Task TestCrmConnection(CancellationToken cancellationToken);
}
