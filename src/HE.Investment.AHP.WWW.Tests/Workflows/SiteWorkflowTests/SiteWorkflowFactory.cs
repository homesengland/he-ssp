using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Common.Contract.Enum;
using ContractSiteTypeDetails = HE.Investment.AHP.Contract.Site.SiteTypeDetails;
using SiteRuralClassification = HE.Investment.AHP.Contract.Site.SiteRuralClassification;
using SiteUseDetails = HE.Investment.AHP.Contract.Site.SiteUseDetails;

namespace HE.Investment.AHP.WWW.Tests.Workflows.SiteWorkflowTests;

public static class SiteWorkflowFactory
{
    public static SiteWorkflow BuildWorkflow(
        SiteWorkflowState currentSiteWorkflowState,
        Section106Dto? section106 = null,
        LocalAuthority? localAuthority = null,
        SitePlanningDetails? planningDetails = null,
        BuildingForHealthyLifeType buildingForHealthyLife = BuildingForHealthyLifeType.Undefined,
        NumberOfGreenLights? numberOfGreenLights = null,
        IList<NationalDesignGuidePriority>? nationalDesignGuidePriorities = null,
        SiteLandAcquisitionStatus? landAcquisitionStatus = null,
        SiteTenderingStatusDetails? tenderingStatusDetails = null,
        StrategicSite? strategicSite = null,
        ContractSiteTypeDetails? siteTypeDetails = null,
        SiteUseDetails? siteUseDetails = null,
        IList<SiteProcurement>? procurements = null,
        SiteRuralClassification? ruralClassification = null,
        EnvironmentalImpact? environmentalImpact = null,
        SiteModernMethodsOfConstruction? modernMethodsOfConstruction = null)
    {
        var site = new SiteModel
        {
            Name = "some site",
            Section106 = section106,
            LocalAuthority = localAuthority,
            PlanningDetails = planningDetails ?? new SitePlanningDetails(SitePlanningStatus.Undefined),
            TenderingStatusDetails = tenderingStatusDetails ?? new SiteTenderingStatusDetails(null, null, null, null),
            NationalDesignGuidePriorities = nationalDesignGuidePriorities ?? new List<NationalDesignGuidePriority>(),
            BuildingForHealthyLife = buildingForHealthyLife,
            NumberOfGreenLights = numberOfGreenLights?.ToString(),
            LandAcquisitionStatus = landAcquisitionStatus,
            StrategicSiteDetails = strategicSite ?? new StrategicSite(false, null),
            SiteTypeDetails = siteTypeDetails ?? new ContractSiteTypeDetails(null, null, null, true),
            SiteUseDetails = siteUseDetails ?? new SiteUseDetails(null, null, TravellerPitchSiteType.Undefined),
            SiteProcurements = procurements ?? new List<SiteProcurement>(),
            RuralClassification = ruralClassification ?? new SiteRuralClassification(null, null),
            EnvironmentalImpact = environmentalImpact?.Value,
            ModernMethodsOfConstruction = modernMethodsOfConstruction ?? new SiteModernMethodsOfConstruction(),
        };

        return new SiteWorkflow(currentSiteWorkflowState, site);
    }
}
