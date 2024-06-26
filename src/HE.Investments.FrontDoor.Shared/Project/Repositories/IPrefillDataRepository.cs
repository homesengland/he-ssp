using HE.Investments.Account.Shared.User;
using HE.Investments.FrontDoor.Shared.Project.Data;

namespace HE.Investments.FrontDoor.Shared.Project.Repositories;

public interface IPrefillDataRepository
{
    Task<ProjectPrefillData> GetProjectPrefillData(FrontDoorProjectId projectId, UserAccount userAccount, CancellationToken cancellationToken);

    Task<SitePrefillData> GetSitePrefillData(FrontDoorProjectId projectId, FrontDoorSiteId siteId, CancellationToken cancellationToken);

    Task MarkProjectAsUsed(FrontDoorProjectId projectId, CancellationToken cancellationToken);
}
