using System.Diagnostics.CodeAnalysis;
using HE.Investments.Common.Contract;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Loans.BusinessLogic.ViewModel;
using Stateless;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication;

[SuppressMessage("Ordering Rules", "SA1201", Justification = "Need to refactored in the fure")]
public class LoanApplicationWorkflow : IStateRouting<LoanApplicationWorkflow.State>
{
    public enum State : int
    {
        Index = 1,
        ApplicationDashboard,
        UserDashboard,
        AboutLoan,
        CheckYourDetails,
        ApplicationName,
        LoanPurpose,
        TaskList,
        CheckApplication,
        ResubmitApplication,
        ApplicationSubmitted,
        Ineligible,
        Withdraw,
    }

    private readonly StateMachine<State, Trigger> _machine;
    private readonly LoanApplicationViewModel _model;

    private readonly Func<Task<LoanApplicationViewModel>> _modelFactory;

    private readonly Func<Task<bool>> _isLoanApplicationExist;

    public LoanApplicationWorkflow(LoanApplicationViewModel model)
    {
        _model = model;
        _machine = new StateMachine<State, Trigger>(model.State);
        ConfigureTransitions();
    }

    public LoanApplicationWorkflow(State currentState, Func<Task<LoanApplicationViewModel>> modelFactory, Func<Task<bool>> isLoanApplicationExist)
    {
        _model = new LoanApplicationViewModel();
        _machine = new StateMachine<State, Trigger>(currentState);
        _isLoanApplicationExist = isLoanApplicationExist;
        _modelFactory = modelFactory;
        ConfigureTransitions();
    }

    public async Task<State> NextState(Trigger trigger)
    {
        await _machine.FireAsync(trigger);
        _model.State = _machine.State;
        return _machine.State;
    }

    public bool IsFilled()
    {
        return _model.Company.IsCompleted()
               && (_model.Security.State == SectionStatus.Completed || _model.Security.IsFlowCompleted)
               && (_model.Funding.IsCompleted() || _model.Funding.IsFlowCompleted)
               && _model.Projects.All(x => x.Status == SectionStatus.Completed)
               && _model.Projects.Any();
    }

    public bool IsFilled(LoanApplicationViewModel application)
    {
        return application.Company.IsCompleted()
               && (application.Security.State == SectionStatus.Completed || application.Security.IsFlowCompleted)
               && (application.Funding.IsCompleted() || application.Funding.IsFlowCompleted)
               && application.Projects.All(x => x.Status == SectionStatus.Completed)
               && application.Projects.Any();
    }

    public async Task<bool> StateCanBeAccessed(State nextState)
    {
        return nextState switch
        {
            State.CheckApplication => IsFilled(await _modelFactory()),
            State.ApplicationSubmitted => IsFilled(await _modelFactory()),
            _ => true,
        };
    }

    private void ConfigureTransitions()
    {
        _machine.Configure(State.Index)
        .Permit(Trigger.Continue, State.AboutLoan);

        _machine.Configure(State.AboutLoan)
            .Permit(Trigger.Continue, State.CheckYourDetails)
            .Permit(Trigger.Back, State.Index);

        _machine.Configure(State.CheckYourDetails)
            .Permit(Trigger.Continue, State.LoanPurpose)
            .Permit(Trigger.Back, State.AboutLoan);

        _machine.Configure(State.LoanPurpose)
            .Permit(Trigger.Back, State.CheckYourDetails);

        _machine.Configure(State.ApplicationName)
            .Permit(Trigger.Back, State.LoanPurpose);

        _machine.Configure(State.Ineligible)
            .Permit(Trigger.Back, State.LoanPurpose);

        _machine.Configure(State.TaskList)
            .Permit(Trigger.Continue, State.CheckApplication)
            .Permit(Trigger.Back, State.ApplicationDashboard);

        _machine.Configure(State.CheckApplication)
            .Permit(Trigger.Continue, State.ApplicationSubmitted)
            .Permit(Trigger.Back, State.TaskList);

        _machine.Configure(State.ResubmitApplication)
            .Permit(Trigger.Continue, State.TaskList)
            .Permit(Trigger.Back, State.TaskList);

        _machine.Configure(State.ApplicationDashboard)
            .Permit(Trigger.Withdraw, State.Withdraw);

        _machine.Configure(State.Withdraw)
            .PermitIf(Trigger.Continue, State.ApplicationDashboard, () => _isLoanApplicationExist().Result)
            .PermitIf(Trigger.Continue, State.UserDashboard, () => _isLoanApplicationExist().Result is false)
            .Permit(Trigger.Back, State.ApplicationDashboard);
    }
}
