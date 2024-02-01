using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Routing;
using Stateless;

namespace HE.Investment.AHP.WWW.Workflows;

public class SiteWorkflow : IStateRouting<SiteWorkflowState>
{
    private readonly StateMachine<SiteWorkflowState, Trigger> _machine;

    private readonly SiteModel? _siteModel;

    public SiteWorkflow(SiteWorkflowState currentSiteWorkflowState, SiteModel? siteModel)
    {
        _machine = new StateMachine<SiteWorkflowState, Trigger>(currentSiteWorkflowState);
        _siteModel = siteModel;
        ConfigureTransitions();
    }

    public async Task<SiteWorkflowState> NextState(Trigger trigger)
    {
        await _machine.FireAsync(trigger);
        return _machine.State;
    }

    public Task<bool> StateCanBeAccessed(SiteWorkflowState nextState)
    {
        return Task.FromResult(CanBeAccessed(nextState));
    }

    public SiteWorkflowState CurrentState(SiteWorkflowState targetState)
    {
        if (targetState != SiteWorkflowState.Start)
        {
            return targetState;
        }

        return _siteModel switch
        {
            { Name: var x } when x.IsNotProvided() => SiteWorkflowState.Name,

            // TODO: #89874  add support for Section106 pages
            // TODO #89873: add support for Local Authority pages
            { PlanningDetails: var x } when x.PlanningStatus.IsNotProvided() => SiteWorkflowState.PlanningStatus,
            { PlanningDetails.ArePlanningDetailsProvided: false } => SiteWorkflowState.PlanningDetails,
            { PlanningDetails: var x } when !IsLandRegistryProvided(x) => SiteWorkflowState.LandRegistry,
            _ => SiteWorkflowState.CheckAnswers,
        };
    }

    private bool CanBeAccessed(SiteWorkflowState state)
    {
        return state switch
        {
            SiteWorkflowState.Index => true,
            SiteWorkflowState.Start => true,
            SiteWorkflowState.Name => true,

            // TODO: #89874  add support for Section106 pages
            // TODO: #89873 add support for Local Authority pages
            SiteWorkflowState.PlanningStatus => true,
            SiteWorkflowState.PlanningDetails => true,
            SiteWorkflowState.LandRegistry => IsLandTitleRegistered(),
            _ => false,
        };
    }

    private void ConfigureTransitions()
    {
        _machine.Configure(SiteWorkflowState.Index)
            .Permit(Trigger.Continue, SiteWorkflowState.Start);

        _machine.Configure(SiteWorkflowState.Start)
            .Permit(Trigger.Continue, SiteWorkflowState.Name)
            .Permit(Trigger.Back, SiteWorkflowState.Index);

        _machine.Configure(SiteWorkflowState.Name)
            .Permit(Trigger.Continue, SiteWorkflowState.Section106GeneralAgreement)
            .Permit(Trigger.Back, SiteWorkflowState.Start);

        _machine.Configure(SiteWorkflowState.Section106GeneralAgreement)
            .PermitIf(Trigger.Continue, SiteWorkflowState.Section106AffordableHousing, () => _siteModel?.Section106GeneralAgreement == true)
            .PermitIf(Trigger.Continue, SiteWorkflowState.LocalAuthoritySearch, () => _siteModel?.Section106GeneralAgreement == false)
            .Permit(Trigger.Back, SiteWorkflowState.Name);

        _machine.Configure(SiteWorkflowState.Section106AffordableHousing)
            .PermitIf(Trigger.Continue, SiteWorkflowState.Section106OnlyAffordableHousing, () => _siteModel?.Section106AffordableHousing == true)
            .PermitIf(Trigger.Continue, SiteWorkflowState.Section106CapitalFundingEligibility, () => _siteModel?.Section106AffordableHousing == false)
            .Permit(Trigger.Back, SiteWorkflowState.Section106GeneralAgreement);

        _machine.Configure(SiteWorkflowState.Section106OnlyAffordableHousing)
            .PermitIf(Trigger.Continue, SiteWorkflowState.Section106CapitalFundingEligibility, () => _siteModel?.Section106OnlyAffordableHousing == true)
            .PermitIf(Trigger.Continue, SiteWorkflowState.Section106AdditionalAffordableHousing, () => _siteModel?.Section106OnlyAffordableHousing == false)
            .Permit(Trigger.Back, SiteWorkflowState.Section106AffordableHousing);

        _machine.Configure(SiteWorkflowState.Section106AdditionalAffordableHousing)
            .Permit(Trigger.Continue, SiteWorkflowState.Section106CapitalFundingEligibility)
            .Permit(Trigger.Back, SiteWorkflowState.Section106AffordableHousing);

        _machine.Configure(SiteWorkflowState.Section106CapitalFundingEligibility)
            .PermitIf(Trigger.Continue, SiteWorkflowState.Section106Ineligible, () => _siteModel?.IsIneligible == true)
            .PermitIf(
                Trigger.Continue,
                SiteWorkflowState.Section106LocalAuthorityConfirmation,
                () => _siteModel is { Section106CapitalFundingEligibility: false, Section106AdditionalAffordableHousing: true })
            .PermitIf(
                Trigger.Continue,
                SiteWorkflowState.LocalAuthoritySearch,
                () => _siteModel is { Section106CapitalFundingEligibility: false, Section106AdditionalAffordableHousing: false })
            .PermitIf(Trigger.Back, SiteWorkflowState.Section106AdditionalAffordableHousing, () => _siteModel?.Section106OnlyAffordableHousing == false)
            .PermitIf(Trigger.Back, SiteWorkflowState.Section106OnlyAffordableHousing, () => _siteModel?.Section106OnlyAffordableHousing == true)
            .PermitIf(Trigger.Back, SiteWorkflowState.Section106AffordableHousing, () => _siteModel?.Section106OnlyAffordableHousing == null);

        _machine.Configure(SiteWorkflowState.Section106LocalAuthorityConfirmation)
            .Permit(Trigger.Continue, SiteWorkflowState.LocalAuthoritySearch)
            .Permit(Trigger.Back, SiteWorkflowState.Section106CapitalFundingEligibility);

        _machine.Configure(SiteWorkflowState.LocalAuthoritySearch)
            .Permit(Trigger.Continue, SiteWorkflowState.LocalAuthorityResult)
            .PermitIf(Trigger.Back, SiteWorkflowState.Section106GeneralAgreement, () => _siteModel?.Section106GeneralAgreement == false)
            .PermitIf(
                Trigger.Back,
                SiteWorkflowState.Section106LocalAuthorityConfirmation,
                () => _siteModel?.Section106AdditionalAffordableHousing != false && _siteModel?.Section106GeneralAgreement != false)
            .PermitIf(
                Trigger.Back,
                SiteWorkflowState.Section106CapitalFundingEligibility,
                () => _siteModel?.Section106AdditionalAffordableHousing == false && _siteModel?.Section106GeneralAgreement != false);

        _machine.Configure(SiteWorkflowState.LocalAuthorityResult)
            .Permit(Trigger.Continue, SiteWorkflowState.LocalAuthorityConfirm)
            .Permit(Trigger.Back, SiteWorkflowState.LocalAuthoritySearch);

        _machine.Configure(SiteWorkflowState.LocalAuthorityConfirm)
            .Permit(Trigger.Continue, SiteWorkflowState.PlanningStatus);

        _machine.Configure(SiteWorkflowState.LocalAuthorityReset)
            .Permit(Trigger.Continue, SiteWorkflowState.PlanningStatus)
            .Permit(Trigger.Back, SiteWorkflowState.LocalAuthoritySearch);

        _machine.Configure(SiteWorkflowState.PlanningStatus)
            .Permit(Trigger.Continue, SiteWorkflowState.PlanningDetails)
            .PermitIf(Trigger.Back, SiteWorkflowState.LocalAuthorityConfirm, IsLocalAuthority)
            .PermitIf(Trigger.Back, SiteWorkflowState.LocalAuthoritySearch, () => !IsLocalAuthority());

        _machine.Configure(SiteWorkflowState.PlanningDetails)
            .PermitIf(Trigger.Continue, SiteWorkflowState.LandRegistry, IsLandTitleRegistered)
            .PermitIf(Trigger.Continue, SiteWorkflowState.NationalDesignGuide, () => !IsLandTitleRegistered())
            .Permit(Trigger.Back, SiteWorkflowState.PlanningStatus);

        _machine.Configure(SiteWorkflowState.LandRegistry)
            .Permit(Trigger.Continue, SiteWorkflowState.NationalDesignGuide)
            .Permit(Trigger.Back, SiteWorkflowState.PlanningDetails);

        _machine.Configure(SiteWorkflowState.NationalDesignGuide)
            .Permit(Trigger.Continue, SiteWorkflowState.BuildingForHealthyLife)
            .Permit(Trigger.Back, SiteWorkflowState.PlanningDetails);

        _machine.Configure(SiteWorkflowState.BuildingForHealthyLife)
            .Permit(Trigger.Continue, SiteWorkflowState.CheckAnswers)
            .Permit(Trigger.Back, SiteWorkflowState.NationalDesignGuide);
    }

    private bool IsLocalAuthority() => _siteModel?.LocalAuthority?.Name.IsProvided() ?? false;

    private bool IsLandTitleRegistered() => _siteModel?.PlanningDetails.IsLandRegistryTitleNumberRegistered ?? false;

    private bool IsLandRegistryProvided(SitePlanningDetails planningDetails) => IsLandTitleRegistered() &&
                                                                                planningDetails.LandRegistryTitleNumber.IsProvided() &&
                                                                                planningDetails.IsGrantFundingForAllHomesCoveredByTitleNumber.IsProvided();
}
