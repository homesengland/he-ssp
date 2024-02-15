extern alias Org;

using Org::HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investment.AHP.Domain.Programme.Crm;

public class ProgrammeCrmContext : IProgrammeCrmContext
{
    public ProgrammeCrmContext()
    {
    }

    public async Task<ProgrammeDto> GetProgramme(string programmeId, CancellationToken cancellationToken)
    {
        return await Task.FromResult(new ProgrammeDto
        {
            name = "AHP",
            startOn = new DateTime(2021, 6, 10, 0, 0, 0, DateTimeKind.Utc),
            endOn = new DateTime(2024, 5, 10, 0, 0, 0, DateTimeKind.Utc),
            milestoneFrameworkItem = new List<MilestoneFrameworkItemDto>
            {
                new() { milestone = 1, name = "Acquisition", percentPaid = 0.5m },
                new() { milestone = 2, name = "StartOnSite", percentPaid = 0.4m },
                new() { milestone = 3, name = "Completion", percentPaid = 0.1m },
            },
        });
    }
}
