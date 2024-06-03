using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.AHP.Consortium.Shared.UserContext;
using HE.Investments.Common.Contract;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Services;
using HE.Investments.Common.Extensions;
using HE.Investments.Programme.Contract;

namespace HE.Investments.AHP.Consortium.Shared.Repositories;

public class ConsortiumCrmRepository : IConsortiumRepository
{
    private readonly ICrmService _service;

    public ConsortiumCrmRepository(ICrmService service)
    {
        _service = service;
    }

    public async Task<ConsortiumBasicInfo> GetConsortium(OrganisationId organisationId, ProgrammeId programmeId)
    {
        var request = new invln_getconsortiumsRequest
        {
            invln_organisationid = organisationId.Value.ToGuidAsString(),
        };

        var consortiumsList = await _service.ExecuteAsync<invln_getconsortiumsRequest, invln_getconsortiumsResponse, IList<ConsortiumDto>>(
            request,
            x => string.IsNullOrWhiteSpace(x.invln_consortiums) ? "[]" : x.invln_consortiums,
            CancellationToken.None);

        var consortiumDto = consortiumsList.FirstOrDefault(x => x.programmeId == programmeId.ToGuidAsString());
        return consortiumDto.IsProvided() ?
            new ConsortiumBasicInfo(
                ConsortiumId.From(consortiumDto!.id),
                consortiumDto.leadPartnerId == organisationId.Value,
                consortiumDto.members?.Select(x => OrganisationId.From(x.id)).ToList() ?? []) :
            ConsortiumBasicInfo.NoConsortium;
    }
}
