using HE.Investments.Common.Infrastructure.HealthChecks;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Common.CRM.Infrastructure;

internal sealed class CrmConnectionTester : ICrmConnectionTester
{
    private readonly IOrganizationServiceAsync2 _serviceClient;

    public CrmConnectionTester(IOrganizationServiceAsync2 serviceClient)
    {
        _serviceClient = serviceClient;
    }

    public async Task TestCrmConnection(CancellationToken cancellationToken)
    {
        await _serviceClient.ExecuteAsync(new WhoAmIRequest(), cancellationToken);
    }
}
