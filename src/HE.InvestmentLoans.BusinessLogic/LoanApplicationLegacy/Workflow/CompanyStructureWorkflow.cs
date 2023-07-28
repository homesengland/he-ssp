using System.Diagnostics.CodeAnalysis;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Routing;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using MediatR;
using Stateless;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;

[SuppressMessage("Ordering Rules", "SA1201", Justification = "Need to refactored in the fure")]
public class CompanyStructureWorkflow
{
    public enum State : int
    {
        Index = 1,
        Purpose,
        ExistingCompany,
        HomesBuilt,
        CheckAnswers,
        Complete,
    }

    private readonly LoanApplicationViewModel _model;
    private readonly StateMachine<State, Trigger> _machine;
    private readonly IMediator _mediator;

    public CompanyStructureWorkflow(LoanApplicationViewModel model, IMediator mediator)
    {
        _model = model;
        _machine = new StateMachine<State, Trigger>(_model.Company.State);
        _mediator = mediator;

        ConfigureTransitions();
    }

    public async void NextState(Trigger trigger)
    {
        await _machine.FireAsync(trigger);
    }

    public bool IsStateComplete()
    {
        return _model.Company.State == State.Complete;
    }

    public bool IsCompleted()
    {
        return IsStateComplete() || _model.Company.IsFlowCompleted;
    }

    public bool IsStarted()
    {
        return _model.Company.Purpose != null
            || _model.Company.ExistingCompany != null
            || _model.Company.HomesBuilt != null;
    }

    public string GetName()
    {
        return Enum.GetName(typeof(State), _model.Company.State) ?? string.Empty;
    }

    public async void ChangeState(State state)
    {
        _model.Company.State = state;
        _model.Company.StateChanged = true;
        await _mediator.Send(new Commands.Update() { Model = _model });
    }

    private void ConfigureTransitions()
    {
        _machine.Configure(State.Index)
            .Permit(Trigger.Continue, State.Purpose);

        _machine.Configure(State.Purpose)
          .Permit(Trigger.Continue, State.ExistingCompany)
          .Permit(Trigger.Change, State.CheckAnswers)
          .Permit(Trigger.Back, State.Index);

        _machine.Configure(State.ExistingCompany)
            .Permit(Trigger.Continue, State.HomesBuilt)
            .Permit(Trigger.Back, State.Purpose)
            .Permit(Trigger.Change, State.CheckAnswers);

        _machine.Configure(State.HomesBuilt)
            .Permit(Trigger.Continue, State.CheckAnswers)
            .Permit(Trigger.Back, State.ExistingCompany)
            .Permit(Trigger.Change, State.CheckAnswers);

        _machine.Configure(State.CheckAnswers)
           .PermitIf(Trigger.Continue, State.Complete, () => _model.Company.CheckAnswers == CommonResponse.Yes)
           .IgnoreIf(Trigger.Continue, () => _model.Company.CheckAnswers != CommonResponse.Yes)
           .PermitIf(Trigger.Back, State.HomesBuilt)
           .OnExit(() =>
           {
               if (_model.Company.CheckAnswers == CommonResponse.Yes)
               {
                   _model.Company.SetFlowCompletion(true);
               }
           });

        _machine.Configure(State.Complete)
         .Permit(Trigger.Back, State.CheckAnswers);

        _machine.OnTransitionCompletedAsync(x =>
        {
            _model.Company.State = x.Destination;
            return _mediator.Send(new Commands.Update() { Model = _model });
        });
    }
}
