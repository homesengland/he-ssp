using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.WWW.Workflows;

namespace HE.Investment.AHP.WWW.Tests.Workflows.SiteWorkflowTests;

public static class SiteWorkflowFactory
{
    public static SiteWorkflow BuildWorkflow(
        SiteWorkflowState currentSiteWorkflowState,
        Section106? section106 = null,
        LocalAuthority? localAuthority = null,
        string? name = null,
        SitePlanningDetails? planningDetails = null,
        IList<NationalDesignGuidePriority>? nationalDesignGuidePriorities = null,
        SiteTenderingStatusDetails? tenderingStatusDetails = null)
    {
        var site = new SiteModel
        {
            Name = name,
            Section106 = section106,
            LocalAuthority = localAuthority,
            PlanningDetails = planningDetails ?? new SitePlanningDetails(SitePlanningStatus.Undefined),
            TenderingStatusDetails = tenderingStatusDetails ?? new SiteTenderingStatusDetails(null, null, null, null),
            NationalDesignGuidePriorities = nationalDesignGuidePriorities ?? new List<NationalDesignGuidePriority>(),
        };

        return new SiteWorkflow(currentSiteWorkflowState, site);
    }
}
