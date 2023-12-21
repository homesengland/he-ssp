using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Loans.Common.Routing;
using Stateless;

namespace HE.Investment.AHP.Contract.Delivery;

public class DeliveryWorkflow : IStateRouting<DeliveryWorkflowState>
{
    private readonly Delivery _model;

    private readonly StateMachine<DeliveryWorkflowState, Trigger> _machine;

    private readonly bool _isReadOnly;

    public DeliveryWorkflow(
        DeliveryWorkflowState currentDeliveryWorkflowState,
        Delivery model,
        bool isReadOnly)
    {
        _model = model;
        _machine = new StateMachine<DeliveryWorkflowState, Trigger>(currentDeliveryWorkflowState);
        _isReadOnly = isReadOnly;
        ConfigureTransitions();
    }

    public async Task<DeliveryWorkflowState> NextState(Trigger trigger)
    {
        await _machine.FireAsync(trigger);
        return _machine.State;
    }

    public DeliveryWorkflowState CurrentState(DeliveryWorkflowState targetState)
    {
        if (_isReadOnly)
        {
            return DeliveryWorkflowState.CheckAnswers;
        }

        if (targetState != DeliveryWorkflowState.Index || _model.SectionStatus == SectionStatus.NotStarted || _model.SectionStatus == SectionStatus.Undefined)
        {
            return targetState;
        }

        return _model switch
        {
            _ => DeliveryWorkflowState.CheckAnswers,
        };
    }

    public Task<bool> StateCanBeAccessed(DeliveryWorkflowState nextState)
    {
        return Task.FromResult(true);
    }

    private void ConfigureTransitions()
    {
        _machine.Configure(DeliveryWorkflowState.Index)
            .Permit(Trigger.Continue, DeliveryWorkflowState.CheckAnswers);
    }
}
