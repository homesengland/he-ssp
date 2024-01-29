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
        return Task.FromResult(true);
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
            .PermitIf(Trigger.Continue, SiteWorkflowState.Section106LocalAuthorityConfirmation, () => _siteModel is { Section106CapitalFundingEligibility: false, Section106AdditionalAffordableHousing: true })
            .PermitIf(Trigger.Continue, SiteWorkflowState.LocalAuthoritySearch, () => _siteModel is { Section106CapitalFundingEligibility: false, Section106AdditionalAffordableHousing: false })
            .PermitIf(Trigger.Back, SiteWorkflowState.Section106AdditionalAffordableHousing, () => _siteModel?.Section106OnlyAffordableHousing == false)
            .PermitIf(Trigger.Back, SiteWorkflowState.Section106OnlyAffordableHousing, () => _siteModel?.Section106OnlyAffordableHousing == true)
            .PermitIf(Trigger.Back, SiteWorkflowState.Section106AffordableHousing, () => _siteModel?.Section106OnlyAffordableHousing == null);

        _machine.Configure(SiteWorkflowState.Section106LocalAuthorityConfirmation)
           .Permit(Trigger.Continue, SiteWorkflowState.LocalAuthoritySearch)
           .Permit(Trigger.Back, SiteWorkflowState.Section106CapitalFundingEligibility);

        _machine.Configure(SiteWorkflowState.LocalAuthoritySearch)
            .Permit(Trigger.Continue, SiteWorkflowState.LocalAuthorityResult)
            .PermitIf(Trigger.Back, SiteWorkflowState.Section106LocalAuthorityConfirmation, () => _siteModel?.Section106AdditionalAffordableHousing != false)
            .PermitIf(Trigger.Back, SiteWorkflowState.Section106CapitalFundingEligibility, () => _siteModel?.Section106AdditionalAffordableHousing == false);

        _machine.Configure(SiteWorkflowState.LocalAuthorityResult)
            .Permit(Trigger.Continue, SiteWorkflowState.LocalAuthorityConfirm)
            .Permit(Trigger.Back, SiteWorkflowState.LocalAuthoritySearch);

        _machine.Configure(SiteWorkflowState.LocalAuthorityConfirm)
            .Permit(Trigger.Continue, SiteWorkflowState.PlanningStatus);

        _machine.Configure(SiteWorkflowState.LocalAuthorityReset)
            .Permit(Trigger.Continue, SiteWorkflowState.PlanningStatus)
            .Permit(Trigger.Back, SiteWorkflowState.LocalAuthoritySearch);

        _machine.Configure(SiteWorkflowState.PlanningStatus)
            .Permit(Trigger.Continue, SiteWorkflowState.Index)
            .PermitIf(Trigger.Back, SiteWorkflowState.LocalAuthorityConfirm, IsLocalAuthority)
            .PermitIf(Trigger.Back, SiteWorkflowState.LocalAuthoritySearch, () => !IsLocalAuthority());
    }

    private bool IsLocalAuthority() => _siteModel?.LocalAuthority?.Name.IsProvided() ?? false;
}
