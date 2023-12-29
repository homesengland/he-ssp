using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Routing;
using Stateless;

namespace HE.Investment.AHP.Contract.Delivery;

public class DeliveriesWorkflow : IStateRouting<DeliveriesWorkflowState>
{
    private readonly Delivery _model;

    private readonly StateMachine<DeliveriesWorkflowState, Trigger> _machine;

    public DeliveriesWorkflow(
        DeliveriesWorkflowState currentDeliveriesWorkflowState,
        Delivery model,
        bool isReadOnly)
    {
        _model = model;
        _machine = new StateMachine<DeliveriesWorkflowState, Trigger>(currentDeliveriesWorkflowState);
        ConfigureTransitions();
    }

    public async Task<DeliveriesWorkflowState> NextState(Trigger trigger)
    {
        await _machine.FireAsync(trigger);
        return _machine.State;
    }

    public DeliveriesWorkflowState CurrentState(DeliveriesWorkflowState targetState)
    {
        if (targetState != DeliveriesWorkflowState.Index || _model.SectionStatus == SectionStatus.NotStarted)
        {
            return targetState;
        }

        return _model switch
        {
            _ => DeliveriesWorkflowState.Index,
        };
    }

    public Task<bool> StateCanBeAccessed(DeliveriesWorkflowState nextState)
    {
        return Task.FromResult(true);
    }

    private void ConfigureTransitions()
    {
    }
}
