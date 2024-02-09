using HE.Investment.AHP.Contract.Consortium;
using HE.Investments.Common.WWW.Routing;
using Stateless;

namespace HE.Investment.AHP.WWW.Workflows;

public class ConsortiumWorkflow : IStateRouting<ConsortiumWorkflowState>
{
    private readonly StateMachine<ConsortiumWorkflowState, Trigger> _machine;

    public ConsortiumWorkflow(ConsortiumWorkflowState currentWorkflowState)
    {
        _machine = new StateMachine<ConsortiumWorkflowState, Trigger>(currentWorkflowState);
        ConfigureTransitions();
    }

    public async Task<ConsortiumWorkflowState> NextState(Trigger trigger)
    {
        await _machine.FireAsync(trigger);
        return _machine.State;
    }

    public async Task<bool> StateCanBeAccessed(ConsortiumWorkflowState nextState)
    {
        return await Task.FromResult(nextState switch
        {
            ConsortiumWorkflowState.Index => true,
            ConsortiumWorkflowState.Start => true,
            _ => false,
        });
    }

    public ConsortiumWorkflowState CurrentState(ConsortiumWorkflowState targetState)
    {
        return targetState;
    }

    private void ConfigureTransitions()
    {
        _machine.Configure(ConsortiumWorkflowState.Start)
            .Permit(Trigger.Continue, ConsortiumWorkflowState.Index)
            .Permit(Trigger.Back, ConsortiumWorkflowState.Index);
    }
}
