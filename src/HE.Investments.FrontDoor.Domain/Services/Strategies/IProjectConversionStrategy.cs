using HE.Investments.FrontDoor.Domain.Project;
using HE.Investments.FrontDoor.Domain.Site;
using HE.UtilsService.BannerNotification.Shared;

namespace HE.Investments.FrontDoor.Domain.Services.Strategies;

public interface IProjectConversionStrategy
{
    Task<ApplicationType> Apply(ProjectEntity project, ProjectSitesEntity projectSites, CancellationToken cancellationToken);
}
