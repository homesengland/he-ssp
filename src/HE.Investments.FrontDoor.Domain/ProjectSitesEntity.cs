using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Contract.Site;
using HE.Investments.FrontDoor.Domain.Site;
using HE.Investments.FrontDoor.Domain.Site.ValueObjects;

namespace HE.Investments.FrontDoor.Domain;

public class ProjectSitesEntity
{
    private readonly IList<ProjectSiteEntity> _sites = new List<ProjectSiteEntity>();

    public ProjectSitesEntity(FrontDoorProjectId projectId, IList<ProjectSiteEntity> sites)
    {
        ProjectId = projectId;
        _sites.AddRange(sites);
    }

    public FrontDoorProjectId ProjectId { get; }

    public ProjectSiteEntity CreateNewSite(SiteName siteName)
    {
        if (_sites.Any(x => x.Name == siteName))
        {
            OperationResult.ThrowValidationError("Name", "This name has already been used on another site");
        }

        var newSite = new ProjectSiteEntity(FrontDoorSiteId.New(), ProjectId, siteName);
        _sites.Add(newSite);
        return newSite;
    }

    public ProjectSiteEntity ChangeSiteName(FrontDoorSiteId siteId, SiteName siteName)
    {
        var site = _sites.Single(x => x.Id == siteId);
        site.ProvideName(siteName);
        return site;
    }

    public FrontDoorSiteId? LastSiteId()
    {
        return _sites.MinBy(x => x.CreatedOn)?.Id;
    }
}
