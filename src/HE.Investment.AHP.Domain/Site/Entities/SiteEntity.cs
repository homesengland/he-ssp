using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Site.Entities;

public class SiteEntity
{
    private readonly ModificationTracker _modificationTracker = new();

    public SiteEntity(
        SiteId id,
        SiteName name,
        Section106 section106,
        string? localAuthority = null,
        PlanningDetails? planningDetails = null)
    {
        Id = id;
        Name = name;
        Status = SiteStatus.NotReady;
        Section106 = section106;
        LocalAuthority = localAuthority;
        PlanningDetails = planningDetails;
    }

    public SiteEntity()
    {
        Id = SiteId.New();
        Name = new SiteName("New Site");
        Status = SiteStatus.NotReady;
        Section106 = new Section106();
    }

    public SiteId Id { get; set; }

    public SiteName Name { get; private set; }

    public Section106 Section106 { get; private set; }

    public string? LocalAuthority { get; }

    public PlanningDetails? PlanningDetails { get; private set; }

    public SiteStatus Status { get; }

    public async Task ProvideName(SiteName siteName, ISiteNameExist siteNameExist, CancellationToken cancellationToken)
    {
        if (await siteNameExist.IsExist(siteName, Id, cancellationToken))
        {
            OperationResult.New()
                .AddValidationError(nameof(Name), "There is already a site with this name. Enter a different name")
                .CheckErrors();
        }

        Name = _modificationTracker.Change(Name, siteName);
    }

    public void ProvideSection106(Section106 section106)
    {
        Section106 = _modificationTracker.Change(Section106, section106);
    }

    public void ProvidePlanningDetails(PlanningDetails planningDetails)
    {
        PlanningDetails = _modificationTracker.Change(PlanningDetails, planningDetails);
    }
}
