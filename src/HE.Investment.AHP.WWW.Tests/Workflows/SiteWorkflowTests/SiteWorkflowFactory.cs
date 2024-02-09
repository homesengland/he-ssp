using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investment.AHP.WWW.Workflows;
using ContractSiteTypeDetails = HE.Investment.AHP.Contract.Site.SiteTypeDetails;
using SiteUseDetails = HE.Investment.AHP.Contract.Site.SiteUseDetails;

namespace HE.Investment.AHP.WWW.Tests.Workflows.SiteWorkflowTests;

public static class SiteWorkflowFactory
{
    public static SiteWorkflow BuildWorkflow(
        SiteWorkflowState currentSiteWorkflowState,
        Section106Dto? section106 = null,
        LocalAuthority? localAuthority = null,
        string? name = null,
        SitePlanningDetails? planningDetails = null,
        BuildingForHealthyLifeType buildingForHealthyLife = BuildingForHealthyLifeType.Undefined,
        NumberOfGreenLights? numberOfGreenLights = null,
        IList<NationalDesignGuidePriority>? nationalDesignGuidePriorities = null,
        SiteTenderingStatusDetails? tenderingStatusDetails = null,
        StrategicSite? strategicSite = null,
        ContractSiteTypeDetails? siteTypeDetails = null,
        SiteUseDetails? siteUseDetails = null)
    {
        var site = new SiteModel
        {
            Name = name,
            Section106 = section106,
            LocalAuthority = localAuthority,
            PlanningDetails = planningDetails ?? new SitePlanningDetails(SitePlanningStatus.Undefined),
            TenderingStatusDetails = tenderingStatusDetails ?? new SiteTenderingStatusDetails(null, null, null, null),
            NationalDesignGuidePriorities = nationalDesignGuidePriorities ?? new List<NationalDesignGuidePriority>(),
            BuildingForHealthyLife = buildingForHealthyLife,
            NumberOfGreenLights = numberOfGreenLights?.ToString(),
            StrategicSiteDetails = strategicSite ?? new StrategicSite(false, null),
            SiteTypeDetails = siteTypeDetails ?? new ContractSiteTypeDetails(null, null, null, true),
            SiteUseDetails = siteUseDetails ?? new SiteUseDetails(null, null, TravellerPitchSiteType.Undefined),
        };

        return new SiteWorkflow(currentSiteWorkflowState, site);
    }
}
