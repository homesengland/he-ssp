using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.WWW.Routing;
using Stateless;

namespace HE.Investment.AHP.Domain.Application.Workflows;

public class ApplicationWorkflow : IStateRouting<ApplicationWorkflowState>
{
    private readonly StateMachine<ApplicationWorkflowState, Trigger> _machine;

    private readonly bool _isReadOnly;

    public ApplicationWorkflow(ApplicationWorkflowState currentWorkflowState, bool isReadOnly)
    {
        _machine = new StateMachine<ApplicationWorkflowState, Trigger>(currentWorkflowState);
        _isReadOnly = isReadOnly;
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

    public ApplicationWorkflowState CurrentState(ApplicationWorkflowState targetState)
    {
        if (_isReadOnly)
        {
            throw new NotFoundException("State could not be accessed when application is read only.");
        }

        return targetState;
    }

    private void ConfigureTransitions()
    {
        _machine.Configure(ApplicationWorkflowState.ApplicationName)
            .Permit(Trigger.Continue, ApplicationWorkflowState.ApplicationTenure);

        _machine.Configure(ApplicationWorkflowState.ApplicationTenure)
            .Permit(Trigger.Back, ApplicationWorkflowState.ApplicationName);

        _machine.Configure(ApplicationWorkflowState.OnHold)
            .Permit(Trigger.Continue, ApplicationWorkflowState.TaskList);

        _machine.Configure(ApplicationWorkflowState.Withdraw)
            .Permit(Trigger.Continue, ApplicationWorkflowState.TaskList);
    }
}
