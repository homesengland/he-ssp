using HE.Investments.FrontDoor.Domain.Programme.Crm;

namespace HE.Investments.FrontDoor.Domain.Programme.Repository;

public class ProgrammeRepository : IProgrammeRepository
{
    private readonly IProgrammeCrmContext _crmContext;

    public ProgrammeRepository(IProgrammeCrmContext crmContext)
    {
        _crmContext = crmContext;
    }

    public async Task<bool> IsAnyAhpProgrammeAvailable(DateOnly? expectedStartDate, CancellationToken cancellationToken)
    {
        var programmes = await _crmContext.GetProgrammes(cancellationToken)
                        ?? throw new InvalidOperationException("Cannot find any AHP Programme");

        foreach (var programme in programmes)
        {
            var isEndDateValid = programme.endOn.HasValue
                                 && expectedStartDate > DateOnly.FromDateTime(programme.endOn.Value);
            var isStartOnSiteEndDateValid = programme.startOnSiteEndDate.HasValue
                                            && expectedStartDate > DateOnly.FromDateTime(programme.startOnSiteEndDate.Value);

            if (isEndDateValid && isStartOnSiteEndDateValid)
            {
                return true;
            }
        }

        return false;
    }
}
