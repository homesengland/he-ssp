using HE.InvestmentLoans.BusinessLogic.LoanApplication;
using HE.InvestmentLoans.Common.Routing;
using Stateless;

namespace HE.Investment.AHP.BusinessLogic.HomeTypes;

public class HomeTypesWorkflow : IStateRouting<HomeTypesWorkflow.State>
{
    public enum State
    {
        Index = 1,
        TypeOfHousing,
    }

    private readonly StateMachine<State, Trigger> _machine;

    public HomeTypesWorkflow(State currentState)
    {
        _machine = new StateMachine<State, Trigger>(currentState);
    }

    public async Task<State> NextState(Trigger trigger)
    {
        await _machine.FireAsync(trigger);
        return _machine.State;
    }

    public Task<bool> StateCanBeAccessed(State nextState)
    {
        throw new NotImplementedException();
    }
}
