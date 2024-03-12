using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Contract.Site;
using HE.Investments.FrontDoor.Domain.Site.ValueObjects;

namespace HE.Investments.FrontDoor.Domain.Site;

public class ProjectSitesEntity
{
    public ProjectSitesEntity(FrontDoorProjectId projectId, IList<ProjectSiteEntity>? sites = null)
    {
        ProjectId = projectId;
        Sites.AddRange(sites);
    }

    public FrontDoorProjectId ProjectId { get; }

    public IList<ProjectSiteEntity> Sites { get; } = new List<ProjectSiteEntity>();

    public ProjectSiteEntity CreateNewSite(SiteName siteName)
    {
        if (Sites.Any(x => x.Name == siteName))
        {
            OperationResult.ThrowValidationError("Name", ValidationMessages.SiteNameIsAlreadyInUser);
        }

        var newSite = new ProjectSiteEntity(ProjectId, FrontDoorSiteId.New(), siteName);
        Sites.Add(newSite);
        return newSite;
    }

    public ProjectSiteEntity ChangeSiteName(FrontDoorSiteId siteId, SiteName siteName)
    {
        var site = Sites.Single(x => x.Id == siteId);
        if (Sites.Any(x => x.Name == siteName && x.Id != siteId))
        {
            OperationResult.ThrowValidationError("Name", ValidationMessages.SiteNameIsAlreadyInUser);
        }

        site.ProvideName(siteName);
        return site;
    }

    public FrontDoorSiteId? LastSiteId()
    {
        return Sites.MaxBy(x => x.CreatedOn)?.Id;
    }
}
