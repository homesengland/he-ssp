using HE.Investments.Common.Contract;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Services;
using HE.Investments.Common.Extensions;
using HE.Investments.IntegrationTestsFramework.Auth;

namespace HE.Investments.AHP.IntegrationTests.Crm;

public class AhpCrmContext
{
    private readonly ICrmService _service;

    public AhpCrmContext(ICrmService service)
    {
        _service = service;
    }

    public async Task ChangeApplicationStatus(string applicationId, ApplicationStatus applicationStatus, ILoginData loginData)
    {
        var crmStatus = AhpApplicationStatusMapper.MapToCrmStatus(applicationStatus);

        var request = new invln_changeahpapplicationstatusRequest
        {
            invln_applicationid = applicationId.ToGuidAsString(),
            invln_organisationid = loginData.OrganisationId,
            invln_userid = loginData.UserGlobalId,
            invln_newapplicationstatus = crmStatus,
            invln_changereason = "[IntegrationTests]",
        };

        await _service.ExecuteAsync<invln_changeahpapplicationstatusRequest, invln_changeahpapplicationstatusResponse>(
            request,
            r => r.ResponseName,
            CancellationToken.None);
    }

    public async Task<bool> HasConsortium(ILoginData loginData)
    {
        var request = new invln_getconsortiumsRequest { invln_organisationid = loginData.OrganisationId };
        var response = await _service.ExecuteAsync<invln_getconsortiumsRequest, invln_getconsortiumsResponse>(
            request,
            r => r.invln_consortiums,
            CancellationToken.None);

        return !string.IsNullOrWhiteSpace(response);
    }
}