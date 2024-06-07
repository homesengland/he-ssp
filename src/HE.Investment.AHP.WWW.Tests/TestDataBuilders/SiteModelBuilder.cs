using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Organisation.Contract;
using SiteRuralClassification = HE.Investment.AHP.Contract.Site.SiteRuralClassification;
using SiteTypeDetails = HE.Investment.AHP.Contract.Site.SiteTypeDetails;
using SiteUseDetails = HE.Investment.AHP.Contract.Site.SiteUseDetails;

namespace HE.Investment.AHP.WWW.Tests.TestDataBuilders;

public static class SiteModelBuilder
{
    public static SiteModel Build(
        Section106Dto? section106 = null,
        LocalAuthority? localAuthority = null,
        SitePlanningDetails? planningDetails = null,
        BuildingForHealthyLifeType buildingForHealthyLife = BuildingForHealthyLifeType.Undefined,
        NumberOfGreenLights? numberOfGreenLights = null,
        IList<NationalDesignGuidePriority>? nationalDesignGuidePriorities = null,
        SiteLandAcquisitionStatus? landAcquisitionStatus = null,
        SiteTenderingStatusDetails? tenderingStatusDetails = null,
        StrategicSite? strategicSite = null,
        SiteTypeDetails? siteTypeDetails = null,
        SiteUseDetails? siteUseDetails = null,
        IList<SiteProcurement>? procurements = null,
        SiteRuralClassification? ruralClassification = null,
        EnvironmentalImpact? environmentalImpact = null,
        SiteModernMethodsOfConstruction? modernMethodsOfConstruction = null,
        bool isConsortiumMember = false,
        bool isUnregisteredBody = false,
        OrganisationDetails? developingPartner = null,
        OrganisationDetails? ownerOfTheLand = null,
        OrganisationDetails? ownerOfTheHomes = null,
        string? id = null)
    {
        return new SiteModel
        {
            Id = id,
            Name = "some site",
            Section106 = section106,
            LocalAuthority = localAuthority,
            PlanningDetails = planningDetails ?? new SitePlanningDetails(SitePlanningStatus.Undefined),
            TenderingStatusDetails = tenderingStatusDetails ?? new SiteTenderingStatusDetails(null, null, null, null),
            NationalDesignGuidePriorities = nationalDesignGuidePriorities ?? [],
            BuildingForHealthyLife = buildingForHealthyLife,
            NumberOfGreenLights = numberOfGreenLights?.ToString(),
            LandAcquisitionStatus = landAcquisitionStatus,
            StrategicSiteDetails = strategicSite ?? new StrategicSite(false, null),
            SiteTypeDetails = siteTypeDetails ?? new SiteTypeDetails(null, null, null, true),
            SiteUseDetails = siteUseDetails ?? new SiteUseDetails(null, null, TravellerPitchSiteType.Undefined),
            SiteProcurements = procurements ?? [],
            RuralClassification = ruralClassification ?? new SiteRuralClassification(null, null),
            EnvironmentalImpact = environmentalImpact?.Value,
            ModernMethodsOfConstruction = modernMethodsOfConstruction ?? new SiteModernMethodsOfConstruction(),
            IsConsortiumMember = isConsortiumMember,
            IsUnregisteredBody = isUnregisteredBody,
            DevelopingPartner = developingPartner,
            OwnerOfTheLand = ownerOfTheLand,
            OwnerOfTheHomes = ownerOfTheHomes,
        };
    }
}
