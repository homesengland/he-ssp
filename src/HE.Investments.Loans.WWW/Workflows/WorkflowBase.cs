using HE.Investments.Common.Contract;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Loans.Contract.ViewModels;
using Stateless;

namespace HE.Investments.Loans.WWW.Workflows;

public abstract class WorkflowBase<TState, TModel> : IStateRouting<TState>
    where TState : Enum
    where TModel : ISectionViewModel
{
    protected StateMachine<TState, Trigger> Machine { get; init; }

    protected TModel Model { get; init; }

    public async Task<TState> NextState(Trigger trigger)
    {
        await Machine.FireAsync(trigger);
        return Machine.State;
    }

    public virtual Task<bool> StateCanBeAccessed(TState nextState)
    {
        return Task.FromResult(true);
    }

    public abstract TState CurrentState(TState targetState);

    protected bool IsSectionStarted() => Model.Status != SectionStatus.NotStarted;
}
