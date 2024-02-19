extern alias Org;

using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Services;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investment.AHP.Domain.Programme.Crm;

public class ProgrammeCrmContext : IProgrammeCrmContext
{
    private readonly ICrmService _service;

    public ProgrammeCrmContext(ICrmService service)
    {
        _service = service;
    }

    public async Task<ProgrammeDto> GetProgramme(string programmeId, CancellationToken cancellationToken)
    {
        var request = new invln_getsingleprogrammeRequest
        {
            invln_programmeId = programmeId,
        };

        var response = await _service.ExecuteAsync<invln_getsingleprogrammeRequest, invln_getsingleprogrammeResponse, ProgrammeDto>(
            request,
            r => r.invln_programme,
            cancellationToken);

        // TODO: #88889 Use Milestone framework from CRM
        response.milestoneFrameworkItem = new List<MilestoneFrameworkItemDto>
        {
            new() { milestone = 1, name = "Acquisition", percentPaid = 0.5m },
            new() { milestone = 2, name = "StartOnSite", percentPaid = 0.4m },
            new() { milestone = 3, name = "Completion", percentPaid = 0.1m },
        };

        return response;
    }
}
