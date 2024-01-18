using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Account.Contract.UserOrganisation;
using HE.Investments.Account.Shared.Repositories;

namespace HE.Investment.AHP.Domain.Programme;

public class AhpProgrammeRepository : IAhpProgrammeRepository
{
    private readonly IProgrammeRepository _programmeRepository;

    public AhpProgrammeRepository(IProgrammeRepository programmeRepository)
    {
        _programmeRepository = programmeRepository;
    }

    public async Task<AhpProgramme> GetProgramme(AhpApplicationId applicationId, CancellationToken cancellationToken)
    {
        var programme = await _programmeRepository.GetCurrentProgramme(ProgrammeType.Ahp, cancellationToken);

        // TODO: Task 88889: [Portal] Use Milestone framework from CRM
        return new AhpProgramme(programme.StartAt, programme.EndAt, MilestoneFramework.Default);
    }
}
