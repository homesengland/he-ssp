using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.FrontDoor.Contract.Site;
using Stateless;

namespace HE.Investments.FrontDoor.WWW.Workflows;

public class SiteWorkflow : IStateRouting<SiteWorkflowState>
{
    private readonly StateMachine<SiteWorkflowState, Trigger> _machine;

    private readonly SiteDetails _model;

    public SiteWorkflow(SiteWorkflowState currentWorkflowState)
    {
        _machine = new StateMachine<SiteWorkflowState, Trigger>(currentWorkflowState);
        _model = new SiteDetails();
        ConfigureTransitions();
    }

    public SiteWorkflow(SiteWorkflowState currentWorkflowState, SiteDetails siteDetails)
    {
        _machine = new StateMachine<SiteWorkflowState, Trigger>(currentWorkflowState);
        _model = siteDetails;
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
        _machine.Configure(SiteWorkflowState.Name)
            .Permit(Trigger.Continue, SiteWorkflowState.HomesNumber);

        _machine.Configure(SiteWorkflowState.HomesNumber)
            .PermitIf(Trigger.Continue, SiteWorkflowState.LocalAuthoritySearch, () => _model.LocalAuthorityCode.IsNotProvided())
            .PermitIf(Trigger.Continue, SiteWorkflowState.LocalAuthorityConfirm, () => _model.LocalAuthorityCode.IsProvided())
            .Permit(Trigger.Back, SiteWorkflowState.Name);

        _machine.Configure(SiteWorkflowState.LocalAuthoritySearch)
            .Permit(Trigger.Back, SiteWorkflowState.HomesNumber);

        _machine.Configure(SiteWorkflowState.LocalAuthorityConfirm)
            .Permit(Trigger.Continue, SiteWorkflowState.PlanningStatus)
            .Permit(Trigger.Back, SiteWorkflowState.LocalAuthoritySearch);

        _machine.Configure(SiteWorkflowState.PlanningStatus)
            .Permit(Trigger.Continue, SiteWorkflowState.AddAnotherSite)
            .PermitIf(Trigger.Back, SiteWorkflowState.LocalAuthoritySearch, () => _model.LocalAuthorityCode.IsNotProvided())
            .PermitIf(Trigger.Back, SiteWorkflowState.LocalAuthorityConfirm, () => _model.LocalAuthorityCode.IsProvided());

        _machine.Configure(SiteWorkflowState.AddAnotherSite)
            .Permit(Trigger.Back, SiteWorkflowState.PlanningStatus);

        _machine.Configure(SiteWorkflowState.RemoveSite)
            .Permit(Trigger.Continue, SiteWorkflowState.CheckAnswers)
            .Permit(Trigger.Back, SiteWorkflowState.CheckAnswers);
    }
}
