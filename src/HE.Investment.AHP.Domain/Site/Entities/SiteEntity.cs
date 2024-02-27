extern alias Org;

using HE.Investment.AHP.Contract.Common.Enums;
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
using HE.Investments.Common.Messages;
using LocalAuthority = Org::HE.Investments.Organisation.LocalAuthorities.ValueObjects.LocalAuthority;
using Section106 = HE.Investment.AHP.Domain.Site.ValueObjects.Section106;
using SiteModernMethodsOfConstruction = HE.Investment.AHP.Domain.Site.ValueObjects.Mmc.SiteModernMethodsOfConstruction;
using SiteRuralClassification = HE.Investment.AHP.Domain.Site.ValueObjects.SiteRuralClassification;
using SiteTypeDetails = HE.Investment.AHP.Domain.Site.ValueObjects.SiteTypeDetails;
using SiteUseDetails = HE.Investment.AHP.Domain.Site.ValueObjects.SiteUseDetails;

namespace HE.Investment.AHP.Domain.Site.Entities;

public class SiteEntity : DomainEntity, IQuestion
{
    private readonly ModificationTracker _modificationTracker = new();

    public SiteEntity(
        SiteId id,
        SiteName name,
        SiteStatus status = SiteStatus.InProgress,
        Section106? section106 = null,
        LocalAuthority? localAuthority = null,
        PlanningDetails? planningDetails = null,
        NationalDesignGuidePriorities? nationalDesignGuidePriorities = null,
        BuildingForHealthyLifeType? buildingForHealthyLife = null,
        NumberOfGreenLights? numberOfGreenLights = null,
        LandAcquisitionStatus? landAcquisitionStatus = null,
        TenderingStatusDetails? tenderingStatusDetails = null,
        StrategicSiteDetails? strategicSiteDetails = null,
        SiteTypeDetails? siteTypeDetails = null,
        SiteUseDetails? siteUseDetails = null,
        SiteRuralClassification? ruralClassification = null,
        EnvironmentalImpact? environmentalImpact = null,
        SiteModernMethodsOfConstruction? modernMethodsOfConstruction = null,
        SiteProcurements? siteProcurements = null)
    {
        Id = id;
        Name = name;
        Status = status;
        Section106 = section106 ?? new Section106();
        LocalAuthority = localAuthority;
        PlanningDetails = planningDetails ?? PlanningDetailsFactory.CreateEmpty();
        NationalDesignGuidePriorities = nationalDesignGuidePriorities ?? new NationalDesignGuidePriorities();
        BuildingForHealthyLife = buildingForHealthyLife ?? BuildingForHealthyLifeType.Undefined;
        NumberOfGreenLights = numberOfGreenLights;
        LandAcquisitionStatus = landAcquisitionStatus ?? new LandAcquisitionStatus();
        TenderingStatusDetails = tenderingStatusDetails ?? new TenderingStatusDetails();
        StrategicSiteDetails = strategicSiteDetails ?? new StrategicSiteDetails();
        SiteTypeDetails = siteTypeDetails ?? new SiteTypeDetails();
        SiteUseDetails = siteUseDetails ?? new SiteUseDetails();
        RuralClassification = ruralClassification ?? new SiteRuralClassification();
        EnvironmentalImpact = environmentalImpact;
        ModernMethodsOfConstruction = modernMethodsOfConstruction ?? new SiteModernMethodsOfConstruction();
        Procurements = siteProcurements ?? new SiteProcurements();
    }

    public SiteId Id { get; set; }

    public SiteName Name { get; private set; }

    public SiteStatus Status { get; private set; }

    public Section106 Section106 { get; private set; }

    public LocalAuthority? LocalAuthority { get; private set; }

    public PlanningDetails PlanningDetails { get; private set; }

    public NationalDesignGuidePriorities NationalDesignGuidePriorities { get; private set; }

    public BuildingForHealthyLifeType BuildingForHealthyLife { get; private set; }

    public NumberOfGreenLights? NumberOfGreenLights { get; private set; }

    public LandAcquisitionStatus LandAcquisitionStatus { get; private set; }

    public TenderingStatusDetails TenderingStatusDetails { get; private set; }

    public StrategicSiteDetails StrategicSiteDetails { get; private set; }

    public SiteTypeDetails SiteTypeDetails { get; private set; }

    public SiteUseDetails SiteUseDetails { get; private set; }

    public SiteRuralClassification RuralClassification { get; private set; }

    public EnvironmentalImpact? EnvironmentalImpact { get; private set; }

    public SiteModernMethodsOfConstruction ModernMethodsOfConstruction { get; private set; }

    public SiteProcurements Procurements { get; private set; }

    public bool IsModified => _modificationTracker.IsModified;

    public static SiteEntity NewSite()
    {
        return new SiteEntity(SiteId.New(), new SiteName("New Site"));
    }

    public async Task ProvideName(SiteName siteName, ISiteNameExist siteNameExist, CancellationToken cancellationToken)
    {
        if (await siteNameExist.IsExist(siteName, Id, cancellationToken))
        {
            OperationResult.New()
                .AddValidationError(nameof(Name), "There is already a site with this name. Enter a different name")
                .CheckErrors();
        }

        Name = _modificationTracker.Change(Name, siteName, MarkAsNotCompleted);
    }

    public void ProvideSection106(Section106 section106)
    {
        Section106 = _modificationTracker.Change(Section106, section106, MarkAsNotCompleted);
    }

    public void ProvideLocalAuthority(LocalAuthority? localAuthority)
    {
        LocalAuthority = _modificationTracker.Change(LocalAuthority, localAuthority, MarkAsNotCompleted);
    }

    public void ProvidePlanningDetails(PlanningDetails planningDetails)
    {
        PlanningDetails = _modificationTracker.Change(PlanningDetails, planningDetails, MarkAsNotCompleted);
    }

    public void ProvideNationalDesignGuidePriorities(NationalDesignGuidePriorities nationalDesignGuidePriorities)
    {
        NationalDesignGuidePriorities = _modificationTracker.Change(NationalDesignGuidePriorities, nationalDesignGuidePriorities, MarkAsNotCompleted);
    }

    public void ProvideBuildingForHealthyLife(BuildingForHealthyLifeType buildingForHealthyLife)
    {
        BuildingForHealthyLife = _modificationTracker.Change(BuildingForHealthyLife, buildingForHealthyLife, MarkAsNotCompleted);
        if (BuildingForHealthyLife != BuildingForHealthyLifeType.Yes)
        {
            ProvideNumberOfGreenLights(null);
        }
    }

    public void ProvideNumberOfGreenLights(NumberOfGreenLights? numberOfGreenLights)
    {
        NumberOfGreenLights = _modificationTracker.Change(NumberOfGreenLights, numberOfGreenLights, MarkAsNotCompleted);
    }

    public void ProvideLandAcquisitionStatus(LandAcquisitionStatus landAcquisitionStatus)
    {
        LandAcquisitionStatus = _modificationTracker.Change(LandAcquisitionStatus, landAcquisitionStatus, MarkAsNotCompleted);
    }

    public void ProvideTenderingStatusDetails(TenderingStatusDetails tenderingStatusDetails)
    {
        TenderingStatusDetails = _modificationTracker.Change(TenderingStatusDetails, tenderingStatusDetails, MarkAsNotCompleted);
    }

    public void ProvideStrategicSiteDetails(StrategicSiteDetails details)
    {
        StrategicSiteDetails = _modificationTracker.Change(StrategicSiteDetails, details, MarkAsNotCompleted);
    }

    public void ProvideSiteTypeDetails(SiteTypeDetails details)
    {
        SiteTypeDetails = _modificationTracker.Change(SiteTypeDetails, details, MarkAsNotCompleted);
    }

    public void ProvideSiteUseDetails(SiteUseDetails details)
    {
        SiteUseDetails = _modificationTracker.Change(SiteUseDetails, details, MarkAsNotCompleted);
    }

    public void ProvideRuralClassification(SiteRuralClassification ruralClassification)
    {
        RuralClassification = _modificationTracker.Change(RuralClassification, ruralClassification, MarkAsNotCompleted);
    }

    public void ProvideEnvironmentalImpact(EnvironmentalImpact? environmentalImpact)
    {
        EnvironmentalImpact = _modificationTracker.Change(EnvironmentalImpact, environmentalImpact, MarkAsNotCompleted);
    }

    public void ProvideModernMethodsOfConstruction(SiteModernMethodsOfConstruction modernMethodsOfConstruction)
    {
        ModernMethodsOfConstruction = _modificationTracker.Change(ModernMethodsOfConstruction, modernMethodsOfConstruction, MarkAsNotCompleted);
    }

    public void ProvideProcurement(SiteProcurements procurements)
    {
        Procurements = _modificationTracker.Change(Procurements, procurements, MarkAsNotCompleted);
    }

    public void Complete(IsSectionCompleted isSectionCompleted)
    {
        if (isSectionCompleted == IsSectionCompleted.Undefied)
        {
            OperationResult.New().AddValidationError(nameof(IsSectionCompleted), "Select whether you have completed this section").CheckErrors();
        }

        if (isSectionCompleted == IsSectionCompleted.No)
        {
            MarkAsNotCompleted();
            return;
        }

        if (!IsAnswered())
        {
            OperationResult.New()
                .AddValidationError(nameof(IsSectionCompleted), ValidationErrorMessage.SectionIsNotCompleted)
                .CheckErrors();
        }

        Status = _modificationTracker.Change(Status, SiteStatus.Completed);
    }

    public bool IsAnswered()
    {
        return Section106.IsAnswered() &&
               LocalAuthority.IsProvided() &&
               PlanningDetails.IsAnswered() &&
               NationalDesignGuidePriorities.IsAnswered() &&
               BuildingForHealthyLife != BuildingForHealthyLifeType.Undefined &&
               LandAcquisitionStatus.IsAnswered() &&
               TenderingStatusDetails.IsAnswered() &&
               StrategicSiteDetails.IsAnswered() &&
               SiteTypeDetails.IsAnswered() &&
               SiteUseDetails.IsAnswered() &&
               RuralClassification.IsAnswered() &&
               EnvironmentalImpact.IsProvided() &&
               ModernMethodsOfConstruction.IsAnswered() &&
               Procurements.IsAnswered() &&
               BuildConditionalRouteCompletionPredicates().All(isCompleted => isCompleted());
    }

    private IEnumerable<Func<bool>> BuildConditionalRouteCompletionPredicates()
    {
        if (BuildingForHealthyLife == BuildingForHealthyLifeType.Yes)
        {
            yield return () => NumberOfGreenLights.IsProvided();
        }
    }

    private void MarkAsNotCompleted()
    {
        Status = _modificationTracker.Change(Status, SiteStatus.InProgress);
    }
}
