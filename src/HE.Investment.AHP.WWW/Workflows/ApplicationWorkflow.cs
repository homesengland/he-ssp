using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Helpers;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.WWW.Routing;
using Stateless;

namespace HE.Investment.AHP.WWW.Workflows;

public class ApplicationWorkflow : IStateRouting<ApplicationWorkflowState>
{
    private readonly StateMachine<ApplicationWorkflowState, Trigger> _machine;

    private readonly Func<Task<Application>> _modelFactory;
    private readonly Func<Task<bool>> _isApplicationExist;
    private readonly bool _isReadOnly;

    public ApplicationWorkflow(ApplicationWorkflowState currentWorkflowState, Func<Task<Application>> modelFactory, Func<Task<bool>> isApplicationExist, bool isReadOnly)
    {
        _isApplicationExist = isApplicationExist;
        _modelFactory = modelFactory;
        _isReadOnly = isReadOnly;
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
        return CanBeAccessed(nextState);
    }

    public async Task<bool> CanBeAccessed(ApplicationWorkflowState state)
    {
        return state switch
        {
            ApplicationWorkflowState.Start => true,
            ApplicationWorkflowState.ApplicationsList => true,
            ApplicationWorkflowState.ApplicationName => true,
            ApplicationWorkflowState.ApplicationTenure => true,
            ApplicationWorkflowState.TaskList => true,
            ApplicationWorkflowState.OnHold => await CanBePutOnHold(),
            ApplicationWorkflowState.Withdraw => await CanBeWithdrawn(),
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null),
        };
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
            .Permit(Trigger.Continue, ApplicationWorkflowState.TaskList)
            .Permit(Trigger.Back, ApplicationWorkflowState.TaskList);

        _machine.Configure(ApplicationWorkflowState.Withdraw)
            .PermitIf(Trigger.Continue, ApplicationWorkflowState.TaskList, () => _isApplicationExist().Result)
            .PermitIf(Trigger.Continue, ApplicationWorkflowState.ApplicationsList, () => !_isApplicationExist().Result)
            .Permit(Trigger.Back, ApplicationWorkflowState.TaskList);
    }

    private async Task<bool> CanBePutOnHold()
    {
        var statusesAllowedForPutOnHold = ApplicationStatusDivision.GetAllStatusesAllowedForPutOnHold();
        var model = await _modelFactory();
        return statusesAllowedForPutOnHold.Contains(model.Status);
    }

    private async Task<bool> CanBeWithdrawn()
    {
        var statusesAllowedForWithdraw = ApplicationStatusDivision.GetAllStatusesAllowedForWithdraw();
        var model = await _modelFactory();
        return statusesAllowedForWithdraw.Contains(model.Status);
    }
}
