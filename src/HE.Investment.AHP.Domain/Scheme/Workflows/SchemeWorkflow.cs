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
            .Permit(Trigger.Back, SchemeWorkflowState.Funding);
    }
}
