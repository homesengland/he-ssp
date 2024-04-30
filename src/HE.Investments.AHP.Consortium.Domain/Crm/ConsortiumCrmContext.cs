extern alias Org;

using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Services;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.AHP.Consortium.Domain.Crm;

public class ConsortiumCrmContext : IConsortiumCrmContext
{
    private static readonly IDictionary<string, IList<string>> JoinRequests = new Dictionary<string, IList<string>>();

    private static readonly IDictionary<string, IList<string>> RemoveRequests = new Dictionary<string, IList<string>>();

    private readonly ICrmService _service;

    public ConsortiumCrmContext(ICrmService service)
    {
        _service = service;
    }

    public async Task<ConsortiumDto> GetConsortium(string consortiumId, string organisationId, CancellationToken cancellationToken)
    {
        var request = new invln_getconsortiumRequest
        {
            invln_consortiumid = consortiumId,
            invln_memberorganisationid = organisationId,
        };

        var consortium = await _service.ExecuteAsync<invln_getconsortiumRequest, invln_getconsortiumResponse, ConsortiumDto>(
            request,
            x => x.invln_consortium,
            cancellationToken);

        if (JoinRequests.TryGetValue(consortiumId, out var joinRequests))
        {
            consortium.members.AddRange(joinRequests.Select(x => new ConsortiumMemberDto
            {
                id = x,
                name = "Dummy",
                status = 858110001,
            }));
        }

        if (RemoveRequests.TryGetValue(consortiumId, out var removeRequests))
        {
            consortium.members.AddRange(removeRequests.Select(x => new ConsortiumMemberDto
            {
                id = x,
                name = "Dummy",
                status = 858110005,
            }));
        }

        return consortium;
    }

    public async Task<bool> IsConsortiumExistForProgrammeAndOrganisation(string programmeId, string organisationId, CancellationToken cancellationToken)
    {
        var request = new invln_IsConsortiumExistForProgrammeAndOrganisationRequest { invln_programmeid = programmeId, invln_organisationid = organisationId, };

        return await _service.ExecuteAsync<invln_IsConsortiumExistForProgrammeAndOrganisationRequest, invln_IsConsortiumExistForProgrammeAndOrganisationResponse>(
            request,
            x => x.invln_isconsortiumexist,
            cancellationToken);
    }

    public async Task<string> CreateConsortium(
        string userId,
        string programmeId,
        string consortiumName,
        string leadOrganisationId,
        CancellationToken cancellationToken)
    {
        var request = new invln_setconsortiumRequest
        {
            invln_programmeId = programmeId,
            invln_leadpartnerid = leadOrganisationId,
            invln_userid = userId,
            invln_consortiumname = consortiumName,
        };

        return await _service.ExecuteAsync<invln_setconsortiumRequest, invln_setconsortiumResponse>(
            request,
            x => x.invln_consortiumid,
            cancellationToken);
    }

    public Task CreateJoinConsortiumRequest(string consortiumId, string organisationId, string userId, CancellationToken cancellationToken)
    {
        // TODO: make request to CRM
        if (JoinRequests.TryGetValue(consortiumId, out var requests))
        {
            requests.Add(organisationId);
            return Task.CompletedTask;
        }

        JoinRequests.Add(consortiumId, new List<string> { organisationId });
        return Task.CompletedTask;
    }

    public Task CreateRemoveFromConsortiumRequest(string consortiumId, string organisationId, string userId, CancellationToken cancellationToken)
    {
        // TODO: make request to CRM
        if (JoinRequests.TryGetValue(consortiumId, out var joinRequests))
        {
            joinRequests.Remove(organisationId);
        }

        if (RemoveRequests.TryGetValue(consortiumId, out var removeRequests))
        {
            removeRequests.Add(organisationId);
            return Task.CompletedTask;
        }

        RemoveRequests.Add(consortiumId, new List<string> { organisationId });
        return Task.CompletedTask;
    }

    public Task CancelPendingConsortiumRequest(string consortiumId, string organisationId, string userId, CancellationToken cancellationToken)
    {
        // TODO: make request to CRM
        if (JoinRequests.TryGetValue(consortiumId, out var joinRequests))
        {
            joinRequests.Remove(organisationId);
        }

        if (RemoveRequests.TryGetValue(consortiumId, out var removeRequests))
        {
            removeRequests.Remove(organisationId);
        }

        return Task.CompletedTask;
    }
}
