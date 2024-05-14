using HE.Investments.Common.Contract.Enum;
using HE.Investments.FrontDoor.Domain.Project;
using HE.Investments.FrontDoor.Domain.Site;

namespace HE.Investments.FrontDoor.Domain.Services.Strategies;

public interface IProjectConversionStrategy
{
    Task<ApplicationType> Apply(ProjectEntity project, ProjectSitesEntity projectSites, CancellationToken cancellationToken);
}
