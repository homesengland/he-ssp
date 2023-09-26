using System.Diagnostics.CodeAnalysis;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Routing;
using HE.InvestmentLoans.Contract.Application.Enums;
using MediatR;
using Stateless;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;

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
        LoanPurpose,
        TaskList,
        CheckApplication,
        ApplicationSubmitted,
        Ineligible,
    }

    private readonly StateMachine<State, Trigger> _machine;
    private readonly IMediator _mediator;
    private readonly LoanApplicationViewModel _model;

    private readonly Func<Task<LoanApplicationViewModel>> _modelFactory;

    public LoanApplicationWorkflow(LoanApplicationViewModel model, IMediator mediator)
    {
        _model = model;
        _machine = new StateMachine<State, Trigger>(model.State);
        _mediator = mediator;
        ConfigureTransitions();
    }

    public LoanApplicationWorkflow(State currentState, Func<Task<LoanApplicationViewModel>> modelFactory)
    {
        _model = new LoanApplicationViewModel { GoodChangeMode = true };
        _machine = new StateMachine<State, Trigger>(currentState);

        ConfigureTransitions();
        _modelFactory = modelFactory;
    }

    public async Task<State> NextState(Trigger trigger)
    {
        await _machine.FireAsync(trigger);
        _model.State = _machine.State;
        return _machine.State;
    }

    public string GetName()
    {
        return Enum.GetName(typeof(State), _model.State) ?? string.Empty;
    }

    public bool IsFilled()
    {
        return _model.Company.IsCompleted()
            && (_model.Security.State == SectionStatus.Completed || _model.Security.IsFlowCompleted)
            && (_model.Funding.IsCompleted() || _model.Funding.IsFlowCompleted)
            && (_model.Sites.All(x => x.State == SiteWorkflow.State.Complete) || _model.Sites.All(x => x.IsFlowCompleted))
            && _model.Sites.Count > 0;
    }

    public bool IsBeingChecked()
    {
        return _model.State == State.CheckApplication;
    }

    public bool IsFilled(LoanApplicationViewModel application)
    {
        return application.Company.IsCompleted()
            && (application.Security.State == SectionStatus.Completed || application.Security.IsFlowCompleted)
            && (application.Funding.IsCompleted() || application.Funding.IsFlowCompleted)
            && (application.Sites.All(x => x.State == SiteWorkflow.State.Complete) || application.Sites.All(x => x.IsFlowCompleted))
            && application.Sites.Count > 0;
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

        _machine.Configure(State.Ineligible)
            .Permit(Trigger.Back, State.LoanPurpose);

        _machine.Configure(State.TaskList)
            .Permit(Trigger.Continue, State.CheckApplication)
            .Permit(Trigger.Back, State.ApplicationDashboard);

        _machine.Configure(State.CheckApplication)
            .Permit(Trigger.Continue, State.ApplicationSubmitted)
            .Permit(Trigger.Back, State.TaskList);

        _machine.OnTransitionCompletedAsync(x =>
        {
            if (_model.GoodChangeMode)
            {
                return Task.CompletedTask;
            }

            _model.State = x.Destination;
            return _mediator.Send(new Commands.Update() { Model = _model });
        });
    }
}
