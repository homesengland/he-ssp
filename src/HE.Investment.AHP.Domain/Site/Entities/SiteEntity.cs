extern alias Org;

using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investment.AHP.Domain.Site.ValueObjects.Factories;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning;
using HE.Investment.AHP.Domain.Site.ValueObjects.StrategicSite;
using HE.Investment.AHP.Domain.Site.ValueObjects.TenderingStatus;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using LocalAuthority = Org::HE.Investments.Organisation.LocalAuthorities.ValueObjects.LocalAuthority;
using Section106 = HE.Investment.AHP.Domain.Site.ValueObjects.Section106;

namespace HE.Investment.AHP.Domain.Site.Entities;

public class SiteEntity : DomainEntity, IQuestion
{
    private readonly ModificationTracker _modificationTracker = new();

    public SiteEntity(
        SiteId id,
        SiteName name,
        Section106 section106,
        PlanningDetails planningDetails,
        NationalDesignGuidePriorities nationalDesignGuidePriorities,
        NumberOfGreenLights? numberOfGreenLights,
        TenderingStatusDetails tenderingStatusDetails,
        StrategicSiteDetails strategicSiteDetails,
        LocalAuthority? localAuthority = null,
        BuildingForHealthyLifeType buildingForHealthyLife = BuildingForHealthyLifeType.Undefined)
    {
        Id = id;
        Name = name;
        Status = SiteStatus.NotReady;
        Section106 = section106;
        LocalAuthority = localAuthority;
        PlanningDetails = planningDetails;
        NationalDesignGuidePriorities = nationalDesignGuidePriorities;
        BuildingForHealthyLife = buildingForHealthyLife;
        NumberOfGreenLights = numberOfGreenLights;
        TenderingStatusDetails = tenderingStatusDetails;
        StrategicSiteDetails = strategicSiteDetails;
    }

    public SiteEntity()
    {
        Id = SiteId.New();
        Name = new SiteName("New Site");
        Status = SiteStatus.NotReady;
        Section106 = new Section106();
        PlanningDetails = PlanningDetailsFactory.CreateEmpty();
        NationalDesignGuidePriorities = new NationalDesignGuidePriorities();
        TenderingStatusDetails = new TenderingStatusDetails();
        StrategicSiteDetails = new StrategicSiteDetails();
    }

    public SiteId Id { get; set; }

    public SiteName Name { get; private set; }

    public SiteStatus Status { get; }

    public Section106 Section106 { get; private set; }

    public LocalAuthority? LocalAuthority { get; private set; }

    public PlanningDetails PlanningDetails { get; private set; }

    public NationalDesignGuidePriorities NationalDesignGuidePriorities { get; private set; }

    public BuildingForHealthyLifeType BuildingForHealthyLife { get; private set; }

    public NumberOfGreenLights? NumberOfGreenLights { get; private set; }

    public TenderingStatusDetails TenderingStatusDetails { get; private set; }

    public StrategicSiteDetails StrategicSiteDetails { get; private set; }

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

    public void ProvideLocalAuthority(LocalAuthority? localAuthority)
    {
        LocalAuthority = _modificationTracker.Change(LocalAuthority, localAuthority);
    }

    public void ProvideNationalDesignGuidePriorities(NationalDesignGuidePriorities nationalDesignGuidePriorities)
    {
        NationalDesignGuidePriorities = _modificationTracker.Change(NationalDesignGuidePriorities, nationalDesignGuidePriorities);
    }

    public void ProvideBuildingForHealthyLife(BuildingForHealthyLifeType buildingForHealthyLife)
    {
        BuildingForHealthyLife = _modificationTracker.Change(BuildingForHealthyLife, buildingForHealthyLife);
    }

    public void ProvideNumberOfGreenLights(NumberOfGreenLights? numberOfGreenLights)
    {
        NumberOfGreenLights = _modificationTracker.Change(NumberOfGreenLights, numberOfGreenLights);
    }

    public void ProvideTenderingStatusDetails(TenderingStatusDetails tenderingStatusDetails)
    {
        TenderingStatusDetails = _modificationTracker.Change(TenderingStatusDetails, tenderingStatusDetails);
    }

    public void ProvideStrategicSiteDetails(StrategicSiteDetails details)
    {
        StrategicSiteDetails = _modificationTracker.Change(StrategicSiteDetails, details);
    }

    public bool IsAnswered()
    {
        return PlanningDetails.IsAnswered() &&
               TenderingStatusDetails.IsAnswered() &&
               StrategicSiteDetails.IsAnswered()
               && BuildingForHealthyLife != BuildingForHealthyLifeType.Undefined
               && BuildConditionalRouteCompletionPredicates().All(isCompleted => isCompleted());
    }

    private IEnumerable<Func<bool>> BuildConditionalRouteCompletionPredicates()
    {
        if (BuildingForHealthyLife == BuildingForHealthyLifeType.Yes)
        {
            yield return () => NumberOfGreenLights.IsProvided();
        }
    }
}
