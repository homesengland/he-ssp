extern alias Org;

using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning;
using HE.Investment.AHP.Domain.Site.ValueObjects.StrategicSite;
using HE.Investment.AHP.Domain.Site.ValueObjects.TenderingStatus;
using HE.Investments.TestsUtils.TestFramework;
using LocalAuthority = Org::HE.Investments.Organisation.LocalAuthorities.ValueObjects.LocalAuthority;
using SiteModernMethodsOfConstruction = HE.Investment.AHP.Domain.Site.ValueObjects.Mmc.SiteModernMethodsOfConstruction;
using SiteRuralClassification = HE.Investment.AHP.Domain.Site.ValueObjects.SiteRuralClassification;
using SiteTypeDetails = HE.Investment.AHP.Domain.Site.ValueObjects.SiteTypeDetails;
using SiteUseDetails = HE.Investment.AHP.Domain.Site.ValueObjects.SiteUseDetails;

namespace HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;

public class SiteEntityBuilder : TestObjectBuilder<SiteEntityBuilder, SiteEntity>
{
    private SiteEntityBuilder()
        : base(SiteEntity.NewSite(null, null))
    {
    }

    protected override SiteEntityBuilder Builder => this;

    public static SiteEntityBuilder New() => new();

    public SiteEntityBuilder WithStatus(SiteStatus value) => SetProperty(x => x.Status, value);

    public SiteEntityBuilder WithSection106(Section106 value) => SetProperty(x => x.Section106, value);

    public SiteEntityBuilder WithLocalAuthority(LocalAuthority? value) => SetProperty(x => x.LocalAuthority, value);

    public SiteEntityBuilder WithPlanningDetails(PlanningDetails value) => SetProperty(x => x.PlanningDetails, value);

    public SiteEntityBuilder WithNationalDesignGuidePriorities(NationalDesignGuidePriorities? value) =>
        SetProperty(x => x.NationalDesignGuidePriorities, value);

    public SiteEntityBuilder WithBuildingForHealthyLife(BuildingForHealthyLifeType value) => SetProperty(x => x.BuildingForHealthyLife, value);

    public SiteEntityBuilder WithNumberOfGreenLights(NumberOfGreenLights? value) => SetProperty(x => x.NumberOfGreenLights, value);

    public SiteEntityBuilder WithLandAcquisitionStatus(LandAcquisitionStatus value) => SetProperty(x => x.LandAcquisitionStatus, value);

    public SiteEntityBuilder WithTenderingStatusDetails(TenderingStatusDetails value) => SetProperty(x => x.TenderingStatusDetails, value);

    public SiteEntityBuilder WithStrategicDetails(StrategicSiteDetails value) => SetProperty(x => x.StrategicSiteDetails, value);

    public SiteEntityBuilder WithSiteTypeDetails(SiteTypeDetails value) => SetProperty(x => x.SiteTypeDetails, value);

    public SiteEntityBuilder WithSiteUseDetails(SiteUseDetails value) => SetProperty(x => x.SiteUseDetails, value);

    public SiteEntityBuilder WithRuralClassification(SiteRuralClassification value) => SetProperty(x => x.RuralClassification, value);

    public SiteEntityBuilder WithEnvironmentalImpact(EnvironmentalImpact? value) => SetProperty(x => x.EnvironmentalImpact, value);

    public SiteEntityBuilder WithModernMethodsOfConstruction(SiteModernMethodsOfConstruction value) => SetProperty(x => x.ModernMethodsOfConstruction, value);

    public SiteEntityBuilder WithProcurements(SiteProcurements value) => SetProperty(x => x.Procurements, value);
}
