extern alias Org;

using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using Org::HE.Investments.Organisation.Services;

namespace HE.Investment.AHP.Domain.Programme;

public class AhpProgrammeRepository : IAhpProgrammeRepository
{
    private readonly IProgrammeService _programmeService;

    public AhpProgrammeRepository(IProgrammeService programmeService)
    {
        _programmeService = programmeService;
    }

    public async Task<AhpProgramme> GetProgramme(AhpApplicationId applicationId, CancellationToken cancellationToken)
    {
        var programme = await _programmeService.Get(cancellationToken);
        if (programme == null)
        {
            throw new InvalidOperationException("Cannot find Programme.");
        }

        //// TODO: Task 88889: [Portal] Use Milestone framework from CRM
        return new AhpProgramme(programme.startOn, programme.endOn, MilestoneFramework.Default);
    }
}
