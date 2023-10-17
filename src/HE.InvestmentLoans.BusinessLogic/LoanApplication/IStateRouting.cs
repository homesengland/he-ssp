using HE.InvestmentLoans.Common.Routing;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication;

public interface IStateRouting<TState>
    where TState : Enum
{
    Task<TState> NextState(Trigger trigger);

    TState CurrentState(TState targetState) => targetState;

    Task<bool> StateCanBeAccessed(TState nextState);
}
