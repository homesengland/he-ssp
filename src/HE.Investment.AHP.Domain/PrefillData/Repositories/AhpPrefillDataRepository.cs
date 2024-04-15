using HE.Investment.AHP.Domain.PrefillData.Data;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Repositories;

namespace HE.Investment.AHP.Domain.PrefillData.Repositories;

public class AhpPrefillDataRepository : IAhpPrefillDataRepository
{
    private readonly IPrefillDataRepository _prefillDataRepository;

    public AhpPrefillDataRepository(IPrefillDataRepository prefillDataRepository)
    {
        _prefillDataRepository = prefillDataRepository;
    }

    public async Task<AhpSitePrefillData> GetAhpSitePrefillData(FrontDoorProjectId projectId, FrontDoorSiteId siteId, CancellationToken cancellationToken)
    {
        var prefillData = await _prefillDataRepository.GetSitePrefillData(projectId, siteId, cancellationToken);

        return new AhpSitePrefillData(
            projectId,
            siteId,
            prefillData.Name,
            prefillData.LocalAuthorityName,
            prefillData.PlanningStatus);
    }
}
