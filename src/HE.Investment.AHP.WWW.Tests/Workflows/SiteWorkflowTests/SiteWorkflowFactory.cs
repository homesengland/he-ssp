using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Workflows;

namespace HE.Investment.AHP.WWW.Tests.Workflows.SiteWorkflowTests;

public static class SiteWorkflowFactory
{
    public static SiteWorkflow BuildWorkflow(
        SiteWorkflowState currentSiteWorkflowState,
        bool? section106GeneralAgreement = null,
        bool? section106AffordableHousing = null,
        bool? section106OnlyAffordableHousing = null,
        bool? section106AdditionalAffordableHousing = null,
        bool? section106CapitalFundingEligibility = null,
        string? section106LocalAuthorityConfirmation = null,
        bool? isIneligible = null,
        LocalAuthority? localAuthority = null,
        string? name = null,
        SitePlanningDetails? planningDetails = null,
        SiteTenderingStatusDetails? tenderingStatusDetails = null)
    {
        var site = new SiteModel
        {
            Name = name,
            Section106GeneralAgreement = section106GeneralAgreement,
            Section106AffordableHousing = section106AffordableHousing,
            Section106OnlyAffordableHousing = section106OnlyAffordableHousing,
            Section106AdditionalAffordableHousing = section106AdditionalAffordableHousing,
            Section106CapitalFundingEligibility = section106CapitalFundingEligibility,
            Section106LocalAuthorityConfirmation = section106LocalAuthorityConfirmation,
            IsIneligible = isIneligible,
            LocalAuthority = localAuthority,
            PlanningDetails = planningDetails ?? new SitePlanningDetails(SitePlanningStatus.Undefined),
            TenderingStatusDetails = tenderingStatusDetails ?? new SiteTenderingStatusDetails(null, null, null, null),
        };

        return new SiteWorkflow(currentSiteWorkflowState, site);
    }
}
