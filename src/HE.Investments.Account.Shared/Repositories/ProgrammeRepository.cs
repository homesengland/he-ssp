using HE.Investments.Account.Contract.UserOrganisation;
using HE.Investments.Organisation.Services;

namespace HE.Investments.Account.Shared.Repositories;

public class ProgrammeRepository : IProgrammeRepository
{
    private readonly IProgrammeService _programmeService;

    public ProgrammeRepository(IProgrammeService programmeService)
    {
        _programmeService = programmeService;
    }

    public async Task<ProgrammeBasicInfo> GetCurrentProgramme(ProgrammeType programmeType)
    {
        var programme = await _programmeService.Get();
        if (programme == null)
        {
            throw new InvalidOperationException("Cannot find Programme.");
        }

        if (programme.startOn == null || programme.endOn == null)
        {
            throw new InvalidOperationException("Programme must have StartDate and EndDate.");
        }

        return new ProgrammeBasicInfo(DateOnly.FromDateTime(programme.startOn.Value), DateOnly.FromDateTime(programme.endOn.Value));
    }
}
