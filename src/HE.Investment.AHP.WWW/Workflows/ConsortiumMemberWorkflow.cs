using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.Common.WWW.Routing;
using Stateless;

namespace HE.Investment.AHP.WWW.Workflows;

public class ConsortiumMemberWorkflow : IStateRouting<ConsortiumMemberWorkflowState>
{
    private readonly ConsortiumDetails _consortium;

    private readonly StateMachine<ConsortiumMemberWorkflowState, Trigger> _machine;

    public ConsortiumMemberWorkflow(ConsortiumDetails consortium, ConsortiumMemberWorkflowState currentWorkflowState)
    {
        _consortium = consortium;
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
            ConsortiumMemberWorkflowState.AddOrganisation => true,
            ConsortiumMemberWorkflowState.AddMembers => _consortium.IsDraft,
            ConsortiumMemberWorkflowState.RemoveMember => true,
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
            .PermitIf(Trigger.Back, ConsortiumMemberWorkflowState.AddMembers, () => _consortium.IsDraft)
            .PermitIf(Trigger.Back, ConsortiumMemberWorkflowState.Index, () => !_consortium.IsDraft);

        _machine.Configure(ConsortiumMemberWorkflowState.SearchResult)
            .PermitIf(Trigger.Continue, ConsortiumMemberWorkflowState.AddMembers, () => _consortium.IsDraft)
            .PermitIf(Trigger.Continue, ConsortiumMemberWorkflowState.Index, () => !_consortium.IsDraft)
            .Permit(Trigger.Back, ConsortiumMemberWorkflowState.SearchOrganisation);

        _machine.Configure(ConsortiumMemberWorkflowState.SearchNoResults)
            .Permit(Trigger.Back, ConsortiumMemberWorkflowState.SearchOrganisation);

        _machine.Configure(ConsortiumMemberWorkflowState.AddOrganisation)
            .PermitIf(Trigger.Continue, ConsortiumMemberWorkflowState.AddMembers, () => _consortium.IsDraft)
            .PermitIf(Trigger.Continue, ConsortiumMemberWorkflowState.Index, () => !_consortium.IsDraft)
            .Permit(Trigger.Back, ConsortiumMemberWorkflowState.SearchOrganisation);

        _machine.Configure(ConsortiumMemberWorkflowState.RemoveMember)
            .PermitIf(Trigger.Continue, ConsortiumMemberWorkflowState.AddMembers, () => _consortium.IsDraft)
            .PermitIf(Trigger.Continue, ConsortiumMemberWorkflowState.Index, () => !_consortium.IsDraft)
            .PermitIf(Trigger.Back, ConsortiumMemberWorkflowState.AddMembers, () => _consortium.IsDraft)
            .PermitIf(Trigger.Back, ConsortiumMemberWorkflowState.Index, () => !_consortium.IsDraft);
    }
}
