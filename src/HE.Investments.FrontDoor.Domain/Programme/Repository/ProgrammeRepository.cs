using HE.Investments.FrontDoor.Domain.Programme.Crm;

namespace HE.Investments.FrontDoor.Domain.Programme.Repository;

public class ProgrammeRepository : IProgrammeRepository
{
    private readonly IProgrammeCrmContext _crmContext;

    public ProgrammeRepository(IProgrammeCrmContext crmContext)
    {
        _crmContext = crmContext;
    }

    public async Task<IEnumerable<ProgrammeDetails>> GetProgrammes(CancellationToken cancellationToken)
    {
        var programmes = await _crmContext.GetProgrammes(cancellationToken)
                        ?? throw new InvalidOperationException("Cannot find any AHP Programme");

        return programmes.Select(programme => new ProgrammeDetails(
                programme.name,
                programme.startOn.HasValue ? DateOnly.FromDateTime(programme.startOn.Value) : null,
                programme.endOn.HasValue ? DateOnly.FromDateTime(programme.endOn.Value) : null,
                programme.startOnSiteEndDate.HasValue ? DateOnly.FromDateTime(programme.startOnSiteEndDate.Value) : null))
            .ToList();
    }
}
