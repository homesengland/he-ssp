using HE.Investments.Common.Contract;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Loans.IntegrationTests.Crm;

public class LoanApplicationCrmRepository
{
    private readonly IOrganizationServiceAsync2 _serviceClient;

    public LoanApplicationCrmRepository(IOrganizationServiceAsync2 serviceClient)
    {
        _serviceClient = serviceClient;
    }

    public async Task ChangeApplicationStatus(string loanApplicationId, ApplicationStatus applicationStatus)
    {
        var crmStatus = ApplicationStatusMapper.MapToCrmStatus(applicationStatus);

        var request = new invln_changeloanapplicationexternalstatusRequest
        {
            invln_loanapplicationid = loanApplicationId,
            invln_statusexternal = crmStatus,
        };

        await _serviceClient.ExecuteAsync(request, CancellationToken.None);
    }
}
