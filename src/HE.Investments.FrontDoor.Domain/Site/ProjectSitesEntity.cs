using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.FrontDoor.Contract.Site.Enums;
using HE.Investments.FrontDoor.Domain.Site.Repository;
using HE.Investments.FrontDoor.Domain.Site.ValueObjects;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.FrontDoor.Domain.Site;

public class ProjectSitesEntity
{
    public ProjectSitesEntity(FrontDoorProjectId projectId, IList<ProjectSiteEntity>? sites = null)
    {
        ProjectId = projectId;
        Sites.AddRange(sites);
    }

    public FrontDoorProjectId ProjectId { get; }

    public IList<ProjectSiteEntity> Sites { get; } = [];

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

    public async Task RemoveSite(IRemoveSiteRepository removeSiteRepository, FrontDoorSiteId siteId, UserAccount userAccount, RemoveSiteAnswer? removeAnswer, CancellationToken cancellationToken)
    {
        if (removeAnswer.IsNotProvided() || removeAnswer == RemoveSiteAnswer.Undefined)
        {
            OperationResult.ThrowValidationError(nameof(RemoveSiteAnswer), "Select yes if you want to remove this site");
        }
        else if (removeAnswer == RemoveSiteAnswer.Yes)
        {
            await removeSiteRepository.Remove(siteId, userAccount, cancellationToken);
        }
    }

    public async Task RemoveAllProjectSites(IRemoveSiteRepository removeSiteRepository, UserAccount userAccount, CancellationToken cancellationToken)
    {
        foreach (var site in Sites)
        {
            await removeSiteRepository.Remove(site.Id, userAccount, cancellationToken);
        }
    }

    public bool AreSitesValidForLoanApplication()
    {
        return Sites.Count == 1 && Sites.All(site => site.IsSiteValidForLoanApplication());
    }

    public void AllSitesAreFilled()
    {
        foreach (var site in Sites)
        {
            site.CanBeCompleted();
        }
    }
}
