using HE.Investments.Common.Workflow;
using Stateless;

namespace HE.Investments.Common.WWW.Routing;

public abstract class EncodedStateRouting<TState> : IStateRouting<TState>
    where TState : struct, Enum
{
    private readonly bool _isLocked;

    protected EncodedStateRouting(TState currentWorkflowState, bool isLocked)
    {
        _isLocked = isLocked;
        Machine = new StateMachine<TState, Trigger>(currentWorkflowState);
    }

    protected StateMachine<TState, Trigger> Machine { get; }

    public abstract bool CanBeAccessed(TState state, bool? isReadOnlyMode = null);

    public async Task<TState> NextState(Trigger trigger)
    {
        if (_isLocked)
        {
            return Machine.State;
        }

        await Machine.FireAsync(trigger);
        return Machine.State;
    }

    public Task<bool> StateCanBeAccessed(TState nextState) => Task.FromResult(CanBeAccessed(nextState));

    public EncodedWorkflow<TState> GetEncodedWorkflow() => new(x => CanBeAccessed(x));
}
