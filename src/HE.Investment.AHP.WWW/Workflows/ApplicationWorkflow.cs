using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract;
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
            ApplicationWorkflowState.OnHold => await IsActionAllowed(AhpApplicationOperation.PutOnHold),
            ApplicationWorkflowState.Reactivate => await IsActionAllowed(AhpApplicationOperation.Reactivate),
            ApplicationWorkflowState.RequestToEdit => await IsActionAllowed(AhpApplicationOperation.RequestToEdit),
            ApplicationWorkflowState.Withdraw => await IsActionAllowed(AhpApplicationOperation.Withdraw),
            ApplicationWorkflowState.CheckAnswers => await CanBeSubmitted(),
            ApplicationWorkflowState.Submit => await CanBeSubmitted(),
            ApplicationWorkflowState.Completed => await IsSubmitted(),
            _ => false,
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

        _machine.Configure(ApplicationWorkflowState.Reactivate)
            .Permit(Trigger.Continue, ApplicationWorkflowState.TaskList);

        _machine.Configure(ApplicationWorkflowState.RequestToEdit)
            .Permit(Trigger.Continue, ApplicationWorkflowState.TaskList)
            .Permit(Trigger.Back, ApplicationWorkflowState.TaskList);

        _machine.Configure(ApplicationWorkflowState.Withdraw)
            .PermitIf(Trigger.Continue, ApplicationWorkflowState.TaskList, () => _isApplicationExist().Result)
            .PermitIf(Trigger.Continue, ApplicationWorkflowState.ApplicationsList, () => !_isApplicationExist().Result)
            .Permit(Trigger.Back, ApplicationWorkflowState.TaskList);

        _machine.Configure(ApplicationWorkflowState.CheckAnswers)
            .Permit(Trigger.Continue, ApplicationWorkflowState.Submit)
            .Permit(Trigger.Back, ApplicationWorkflowState.TaskList);

        _machine.Configure(ApplicationWorkflowState.Submit)
            .Permit(Trigger.Continue, ApplicationWorkflowState.Completed)
            .Permit(Trigger.Back, ApplicationWorkflowState.CheckAnswers);
    }

    private async Task<bool> IsActionAllowed(AhpApplicationOperation operation)
    {
        var model = await _modelFactory();
        return model.AllowedOperations.Contains(operation);
    }

    private async Task<bool> CanBeSubmitted()
    {
        var model = await _modelFactory();
        var allSectionsCompleted = model.Sections.All(x => x.SectionStatus == SectionStatus.Completed);
        return model.AllowedOperations.Contains(AhpApplicationOperation.Submit) && allSectionsCompleted;
    }

    private async Task<bool> IsSubmitted()
    {
        var model = await _modelFactory();
        var allSectionsCompleted = model.Sections.All(x => x.SectionStatus == SectionStatus.Submitted);
        return model.Status == ApplicationStatus.ApplicationSubmitted && allSectionsCompleted;
    }
}
