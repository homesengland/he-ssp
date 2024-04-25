using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.Common.WWW.Routing;
using Stateless;

namespace HE.Investment.AHP.WWW.Workflows;

public class ConsortiumMemberWorkflow : IStateRouting<ConsortiumMemberWorkflowState>
{
    private readonly StateMachine<ConsortiumMemberWorkflowState, Trigger> _machine;

    public ConsortiumMemberWorkflow(ConsortiumMemberWorkflowState currentWorkflowState)
    {
        _machine = new StateMachine<ConsortiumMemberWorkflowState, Trigger>(currentWorkflowState);
        ConfigureTransitions();
    }

    public async Task<ConsortiumMemberWorkflowState> NextState(Trigger trigger)
    {
        await _machine.FireAsync(trigger);
        return _machine.State;
    }

    public async Task<bool> StateCanBeAccessed(ConsortiumMemberWorkflowState nextState)
    {
        return await Task.FromResult(nextState switch
        {
            ConsortiumMemberWorkflowState.Index => true,
            ConsortiumMemberWorkflowState.SearchOrganisation => true,
            ConsortiumMemberWorkflowState.SearchResult => true,
            ConsortiumMemberWorkflowState.SearchNoResults => true,
            _ => false,
        });
    }

    public ConsortiumMemberWorkflowState CurrentState(ConsortiumMemberWorkflowState targetState)
    {
        return targetState;
    }

    private void ConfigureTransitions()
    {
        _machine.Configure(ConsortiumMemberWorkflowState.SearchOrganisation)
            .Permit(Trigger.Continue, ConsortiumMemberWorkflowState.SearchResult)
            .Permit(Trigger.Back, ConsortiumMemberWorkflowState.Index);

        _machine.Configure(ConsortiumMemberWorkflowState.SearchResult)
            .Permit(Trigger.Continue, ConsortiumMemberWorkflowState.Index)
            .Permit(Trigger.Back, ConsortiumMemberWorkflowState.SearchOrganisation);

        _machine.Configure(ConsortiumMemberWorkflowState.SearchNoResults)
            .Permit(Trigger.Back, ConsortiumMemberWorkflowState.SearchOrganisation);
    }
}
