extern alias Org;

using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Programme.Config;
using HE.Investment.AHP.Domain.Programme.Crm;

namespace HE.Investment.AHP.Domain.Programme;

public class AhpProgrammeRepository : IAhpProgrammeRepository
{
    private readonly IProgrammeCrmContext _crmContext;

    private readonly IProgrammeSettings _settings;

    public AhpProgrammeRepository(IProgrammeCrmContext crmContext, IProgrammeSettings settings)
    {
        _settings = settings;
        _crmContext = crmContext;
    }

    public async Task<AhpProgramme> GetProgramme(CancellationToken cancellationToken)
    {
        var programme = await _crmContext.GetProgramme(_settings.AhpProgrammeId, cancellationToken)
            ?? throw new InvalidOperationException("Cannot find AHP Programme");

        var acquisition = programme.milestoneFrameworkItem.SingleOrDefault(x => x.milestone == 1)?.percentPaid ??
                          throw new InvalidOperationException("Milestone framework does not have Acquisition percentage set.");

        var startOnSite = programme.milestoneFrameworkItem.SingleOrDefault(x => x.milestone == 2)?.percentPaid ??
                          throw new InvalidOperationException("Milestone framework does not have StartOnSite percentage set.");

        var completion = programme.milestoneFrameworkItem.SingleOrDefault(x => x.milestone == 3)?.percentPaid ??
                         throw new InvalidOperationException("Milestone framework does not have Completion percentage set.");

        return new AhpProgramme(
            _settings.AhpProgrammeId,
            programme.name,
            new ProgrammeDates(
                DateOnly.FromDateTime(programme.startOn ?? throw new InvalidOperationException("Ahp programme does not have Start Date set.")),
                DateOnly.FromDateTime(programme.endOn ?? throw new InvalidOperationException("Ahp programme does not have End Date set.")),
                programme.startOnSiteStartDate.HasValue ? DateOnly.FromDateTime(programme.startOnSiteStartDate.Value) : null,
                programme.startOnSiteEndDate.HasValue ? DateOnly.FromDateTime(programme.startOnSiteEndDate.Value) : null,
                programme.completionStartDate.HasValue ? DateOnly.FromDateTime(programme.completionStartDate.Value) : null,
                programme.completionEndDate.HasValue ? DateOnly.FromDateTime(programme.completionEndDate.Value) : null),
            new MilestoneFramework(acquisition, startOnSite, completion));
    }
}
