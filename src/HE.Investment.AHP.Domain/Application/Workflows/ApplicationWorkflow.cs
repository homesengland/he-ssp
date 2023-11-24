using HE.Investments.Loans.Common.Routing;
using Stateless;

namespace HE.Investment.AHP.Domain.Application.Workflows;

public class ApplicationWorkflow : IStateRouting<ApplicationWorkflowState>
{
    private readonly StateMachine<ApplicationWorkflowState, Trigger> _machine;

    public ApplicationWorkflow(ApplicationWorkflowState currentWorkflowState)
    {
        _machine = new StateMachine<ApplicationWorkflowState, Trigger>(currentWorkflowState);
        ConfigureTransitions();
    }

    public async Task<ApplicationWorkflowState> NextState(Trigger trigger)
    {
        await _machine.FireAsync(trigger);
        return _machine.State;
    }

    public Task<bool> StateCanBeAccessed(ApplicationWorkflowState nextState)
    {
        return Task.FromResult(true);
    }

    private void ConfigureTransitions()
    {
        _machine.Configure(ApplicationWorkflowState.ApplicationName)
            .Permit(Trigger.Continue, ApplicationWorkflowState.ApplicationTenure);

        _machine.Configure(ApplicationWorkflowState.ApplicationTenure)
            .Permit(Trigger.Back, ApplicationWorkflowState.ApplicationName);
    }
}
