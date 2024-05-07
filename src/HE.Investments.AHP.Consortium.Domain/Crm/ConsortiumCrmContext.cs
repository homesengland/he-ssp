extern alias Org;

using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Services;
using HE.Investments.Common.Extensions;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.AHP.Consortium.Domain.Crm;

public class ConsortiumCrmContext : IConsortiumCrmContext
{
    private static readonly Dictionary<string, IList<string>> JoinRequests = new();

    private static readonly Dictionary<string, IList<string>> RemoveRequests = new();

    private readonly ICrmService _service;

    public ConsortiumCrmContext(ICrmService service)
    {
        _service = service;
    }

    public async Task<ConsortiumDto> GetConsortium(string consortiumId, string organisationId, CancellationToken cancellationToken)
    {
        var request = new invln_getconsortiumRequest
        {
            invln_consortiumid = consortiumId.ToGuidAsString(),
            invln_memberorganisationid = organisationId.TryToGuidAsString(),
        };

        var consortium = await _service.ExecuteAsync<invln_getconsortiumRequest, invln_getconsortiumResponse, ConsortiumDto>(
            request,
            x => x.invln_consortium,
            cancellationToken);

        if (JoinRequests.TryGetValue(consortiumId.ToGuidAsString(), out var joinRequests))
        {
            consortium.members.AddRange(joinRequests.Select(x => new ConsortiumMemberDto { id = x, name = "Dummy", status = 858110001, }));
        }

        if (RemoveRequests.TryGetValue(consortiumId.ToGuidAsString(), out var removeRequests))
        {
            consortium.members.AddRange(removeRequests.Select(x => new ConsortiumMemberDto { id = x, name = "Dummy", status = 858110005, }));
        }

        return consortium;
    }

    public async Task<IList<ConsortiumDto>> GetConsortiumsListByMemberId(string organisationId, CancellationToken cancellationToken)
    {
        var request = new invln_getconsortiumsRequest()
        {
            invln_organisationid = organisationId.TryToGuidAsString(),
        };

        var consortiumsList = await _service.ExecuteAsync<invln_getconsortiumsRequest, invln_getconsortiumsResponse, IList<ConsortiumDto>>(
            request,
            x => x.invln_consortiums,
            cancellationToken);

        return consortiumsList;
    }

    public async Task<bool> IsConsortiumExistForProgrammeAndOrganisation(string programmeId, string organisationId, CancellationToken cancellationToken)
    {
        var request = new invln_IsConsortiumExistForProgrammeAndOrganisationRequest
        {
            invln_programmeid = programmeId.ToGuidAsString(),
            invln_organisationid = organisationId.TryToGuidAsString(),
        };

        return await _service
            .ExecuteAsync<invln_IsConsortiumExistForProgrammeAndOrganisationRequest, invln_IsConsortiumExistForProgrammeAndOrganisationResponse>(
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
            invln_programmeId = programmeId.ToGuidAsString(),
            invln_leadpartnerid = leadOrganisationId.TryToGuidAsString(),
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
        if (JoinRequests.TryGetValue(consortiumId.ToGuidAsString(), out var requests))
        {
            requests.Add(organisationId.ToGuidAsString());
            return Task.CompletedTask;
        }

        JoinRequests.Add(consortiumId.ToGuidAsString(), new List<string> { organisationId.ToGuidAsString() });
        return Task.CompletedTask;
    }

    public Task CreateRemoveFromConsortiumRequest(string consortiumId, string organisationId, string userId, CancellationToken cancellationToken)
    {
        // TODO: make request to CRM
        if (JoinRequests.TryGetValue(consortiumId.ToGuidAsString(), out var joinRequests))
        {
            joinRequests.Remove(organisationId.ToGuidAsString());
        }

        if (RemoveRequests.TryGetValue(consortiumId.ToGuidAsString(), out var removeRequests))
        {
            removeRequests.Add(organisationId.ToGuidAsString());
            return Task.CompletedTask;
        }

        RemoveRequests.Add(consortiumId.ToGuidAsString(), new List<string> { organisationId.ToGuidAsString() });
        return Task.CompletedTask;
    }

    public Task CancelPendingConsortiumRequest(string consortiumId, string organisationId, string userId, CancellationToken cancellationToken)
    {
        // TODO: make request to CRM
        if (JoinRequests.TryGetValue(consortiumId.ToGuidAsString(), out var joinRequests))
        {
            joinRequests.Remove(organisationId.ToGuidAsString());
        }

        if (RemoveRequests.TryGetValue(consortiumId.ToGuidAsString(), out var removeRequests))
        {
            removeRequests.Remove(organisationId.ToGuidAsString());
        }

        return Task.CompletedTask;
    }
}
