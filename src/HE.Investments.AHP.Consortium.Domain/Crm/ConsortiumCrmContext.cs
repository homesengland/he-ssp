extern alias Org;

using HE.Investments.Common.Contract;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Services;
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
            invln_consortiumid = ShortGuid.ToGuidAsString(consortiumId),
            invln_memberorganisationid = ShortGuid.ToGuidAsString(organisationId),
        };

        var consortium = await _service.ExecuteAsync<invln_getconsortiumRequest, invln_getconsortiumResponse, ConsortiumDto>(
            request,
            x => x.invln_consortium,
            cancellationToken);

        if (JoinRequests.TryGetValue(consortiumId, out var joinRequests))
        {
            consortium.members.AddRange(joinRequests.Select(x => new ConsortiumMemberDto { id = x, name = "Dummy", status = 858110001, }));
        }

        if (RemoveRequests.TryGetValue(consortiumId, out var removeRequests))
        {
            consortium.members.AddRange(removeRequests.Select(x => new ConsortiumMemberDto { id = x, name = "Dummy", status = 858110005, }));
        }

        return consortium;
    }

    public async Task<bool> IsConsortiumExistForProgrammeAndOrganisation(string programmeId, string organisationId, CancellationToken cancellationToken)
    {
        var request = new invln_IsConsortiumExistForProgrammeAndOrganisationRequest
        {
            invln_programmeid = ShortGuid.ToGuidAsString(programmeId),
            invln_organisationid = ShortGuid.ToGuidAsString(organisationId),
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
            invln_programmeId = ShortGuid.ToGuidAsString(programmeId),
            invln_leadpartnerid = ShortGuid.ToGuidAsString(leadOrganisationId),
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
            requests.Add(ShortGuid.ToGuidAsString(organisationId));
            return Task.CompletedTask;
        }

        JoinRequests.Add(ShortGuid.ToGuidAsString(consortiumId), new List<string> { ShortGuid.ToGuidAsString(organisationId) });
        return Task.CompletedTask;
    }

    public Task CreateRemoveFromConsortiumRequest(string consortiumId, string organisationId, string userId, CancellationToken cancellationToken)
    {
        // TODO: make request to CRM
        if (JoinRequests.TryGetValue(ShortGuid.ToGuidAsString(consortiumId), out var joinRequests))
        {
            joinRequests.Remove(ShortGuid.ToGuidAsString(organisationId));
        }

        if (RemoveRequests.TryGetValue(ShortGuid.ToGuidAsString(consortiumId), out var removeRequests))
        {
            removeRequests.Add(ShortGuid.ToGuidAsString(organisationId));
            return Task.CompletedTask;
        }

        RemoveRequests.Add(ShortGuid.ToGuidAsString(consortiumId), new List<string> { ShortGuid.ToGuidAsString(organisationId) });
        return Task.CompletedTask;
    }

    public Task CancelPendingConsortiumRequest(string consortiumId, string organisationId, string userId, CancellationToken cancellationToken)
    {
        // TODO: make request to CRM
        if (JoinRequests.TryGetValue(ShortGuid.ToGuidAsString(consortiumId), out var joinRequests))
        {
            joinRequests.Remove(ShortGuid.ToGuidAsString(organisationId));
        }

        if (RemoveRequests.TryGetValue(ShortGuid.ToGuidAsString(consortiumId), out var removeRequests))
        {
            removeRequests.Remove(ShortGuid.ToGuidAsString(organisationId));
        }

        return Task.CompletedTask;
    }
}
