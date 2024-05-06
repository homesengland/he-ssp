using HE.Investment.AHP.Domain.PrefillData.Data;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investment.AHP.Domain.PrefillData.Repositories;

public interface IAhpPrefillDataRepository
{
    Task<AhpSitePrefillData> GetAhpSitePrefillData(FrontDoorProjectId projectId, FrontDoorSiteId siteId, CancellationToken cancellationToken);
}
