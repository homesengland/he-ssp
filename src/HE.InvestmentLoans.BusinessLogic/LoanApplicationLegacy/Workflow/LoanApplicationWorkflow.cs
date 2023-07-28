using System.Diagnostics.CodeAnalysis;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Routing;
using HE.InvestmentLoans.Contract.Application.Enums;
using MediatR;
using Stateless;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;

[SuppressMessage("Ordering Rules", "SA1201", Justification = "Need to refactored in the fure")]
public class LoanApplicationWorkflow
{
    public enum State : int
    {
        Index = 1,
        AboutLoan,
        CheckYourDetails,
        LoanPurpose,
        TaskList,
        CheckAnswers,
        ApplicationSubmitted,
        Ineligible,
    }

    private readonly StateMachine<State, Trigger> _machine;
    private readonly IMediator _mediator;
    private readonly LoanApplicationViewModel _model;

    public LoanApplicationWorkflow(LoanApplicationViewModel model, IMediator mediator)
    {
        _model = model;
        _machine = new StateMachine<State, Trigger>(_model.State);
        _mediator = mediator;
        ConfigureTransitions();
    }

    public async Task<State> NextState(Trigger trigger)
    {
        await _machine.FireAsync(trigger);
        _model.State = _machine.State;
        return _machine.State;
    }

    public async void ChangeState(State state)
    {
        _model.State = state;
        await _mediator.Send(new Commands.Update() { Model = _model });
    }

    public string GetName()
    {
        return Enum.GetName(typeof(State), _model.State) ?? string.Empty;
    }

    public bool IsFilled()
    {
        return (_model.Company.State == CompanyStructureWorkflow.State.Complete || _model.Company.IsFlowCompleted)
            && (_model.Security.State == SecurityWorkflow.State.Complete || _model.Security.IsFlowCompleted)
            && (_model.Funding.State == FundingWorkflow.State.Complete || _model.Funding.IsFlowCompleted)
            && (_model.Sites.All(x => x.State == SiteWorkflow.State.Complete) || _model.Sites.All(x => x.IsFlowCompleted))
            && _model.Sites.Count > 0;
    }

    public bool IsBeingChecked()
    {
        return _model.State == State.CheckAnswers;
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
            .PermitIf(Trigger.Continue, State.TaskList, () => _model.Purpose == FundingPurpose.BuildingNewHomes)
            .PermitIf(Trigger.Continue, State.Ineligible, () => _model.Purpose != FundingPurpose.BuildingNewHomes)
            .Permit(Trigger.Back, State.CheckYourDetails);

        _machine.Configure(State.Ineligible)
            .Permit(Trigger.Back, State.LoanPurpose);

        _machine.Configure(State.TaskList)
            .Permit(Trigger.Continue, State.CheckAnswers)
            .Permit(Trigger.Back, State.LoanPurpose);

        _machine.Configure(State.CheckAnswers)
            .Permit(Trigger.Continue, State.ApplicationSubmitted)
            .Permit(Trigger.Back, State.TaskList);

        _machine.Configure(State.ApplicationSubmitted).OnEntry(x => _mediator.Send(new SubmitApplicationCommand(_model)).GetAwaiter().GetResult());

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
