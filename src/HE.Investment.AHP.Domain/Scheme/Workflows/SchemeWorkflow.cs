using HE.InvestmentLoans.Common.Routing;
using Stateless;

namespace HE.Investment.AHP.Domain.Scheme.Workflows;

public class SchemeWorkflow : IStateRouting<SchemeWorkflowState>
{
    private readonly StateMachine<SchemeWorkflowState, Trigger> _machine;

    public SchemeWorkflow(SchemeWorkflowState currentWorkflowState)
    {
        _machine = new StateMachine<SchemeWorkflowState, Trigger>(currentWorkflowState);
        ConfigureTransitions();
    }

    public async Task<SchemeWorkflowState> NextState(Trigger trigger)
    {
        await _machine.FireAsync(trigger);
        return _machine.State;
    }

    public Task<bool> StateCanBeAccessed(SchemeWorkflowState nextState)
    {
        return Task.FromResult(true);
    }

    private void ConfigureTransitions()
    {
        _machine.Configure(SchemeWorkflowState.Funding)
            .Permit(Trigger.Continue, SchemeWorkflowState.Affordability);

        _machine.Configure(SchemeWorkflowState.Affordability)
            .Permit(Trigger.Continue, SchemeWorkflowState.SalesRisk)
            .Permit(Trigger.Back, SchemeWorkflowState.Funding);

        _machine.Configure(SchemeWorkflowState.SalesRisk)
            .Permit(Trigger.Continue, SchemeWorkflowState.HousingNeeds)
            .Permit(Trigger.Back, SchemeWorkflowState.Affordability);

        _machine.Configure(SchemeWorkflowState.HousingNeeds)
            .Permit(Trigger.Continue, SchemeWorkflowState.StakeholderDiscussions)
            .Permit(Trigger.Back, SchemeWorkflowState.SalesRisk);

        _machine.Configure(SchemeWorkflowState.StakeholderDiscussions)
            .Permit(Trigger.Continue, SchemeWorkflowState.Summary)
            .Permit(Trigger.Back, SchemeWorkflowState.HousingNeeds);

        _machine.Configure(SchemeWorkflowState.Summary)
            .Permit(Trigger.Back, SchemeWorkflowState.StakeholderDiscussions);
    }
}
