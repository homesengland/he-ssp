using System.Diagnostics.CodeAnalysis;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Routing;
using MediatR;
using Stateless;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;

[SuppressMessage("Ordering Rules", "SA1201", Justification = "Need to refactored in the fure")]
public class FundingWorkflow
{
    public enum State : int
    {
        Index,
        GDV,
        TotalCosts,
        AbnormalCosts,
        PrivateSectorFunding,
        AdditionalProjects,
        Refinance,
        CheckAnswers,
        Complete,
    }

    private readonly LoanApplicationViewModel _model;
    private readonly StateMachine<State, Trigger> _machine;
    private readonly IMediator _mediator;

    public FundingWorkflow(LoanApplicationViewModel model, IMediator mediator)
    {
        _model = model;
        _machine = new StateMachine<State, Trigger>(_model.Funding.State);
        _mediator = mediator;

        ConfigureTransitions();
    }

    public async void NextState(Trigger trigger)
    {
        await _machine.FireAsync(trigger);
    }

    public bool IsStateComplete()
    {
        return _model.Funding.State == State.Complete;
    }

    public bool IsWorkflowCompleted()
    {
        return IsStateComplete() || _model.Funding.IsFlowCompleted;
    }

    public bool IsStarted()
    {
        return _model.Funding.GrossDevelopmentValue != null
            || _model.Funding.TotalCosts != null
            || _model.Funding.AbnormalCosts != null
            || _model.Funding.PrivateSectorFunding != null
            || _model.Funding.Refinance != null
            || _model.Funding.AdditionalProjects != null;
    }

    public string GetName()
    {
        return Enum.GetName(typeof(State), _model.Funding.State) ?? string.Empty;
    }

    public async void ChangeState(State state)
    {
        _model.Funding.State = state;
        _model.Funding.StateChanged = true;
        await _mediator.Send(new Commands.Update() { Model = _model });
    }

    private void ConfigureTransitions()
    {
        _machine.Configure(State.Index)
          .Permit(Trigger.Continue, State.GDV);

        _machine.Configure(State.GDV)
            .Permit(Trigger.Continue, State.TotalCosts)
            .Permit(Trigger.Back, State.Index)
            .Permit(Trigger.Change, State.CheckAnswers);

        _machine.Configure(State.TotalCosts)
            .Permit(Trigger.Continue, State.AbnormalCosts)
            .Permit(Trigger.Back, State.GDV)
            .Permit(Trigger.Change, State.CheckAnswers);

        _machine.Configure(State.AbnormalCosts)
            .Permit(Trigger.Continue, State.PrivateSectorFunding)
            .Permit(Trigger.Back, State.TotalCosts)
            .Permit(Trigger.Change, State.CheckAnswers);

        _machine.Configure(State.PrivateSectorFunding)
            .Permit(Trigger.Continue, State.Refinance)
            .Permit(Trigger.Back, State.AbnormalCosts)
            .Permit(Trigger.Change, State.CheckAnswers);

        _machine.Configure(State.Refinance)
            .Permit(Trigger.Continue, State.AdditionalProjects)
            .Permit(Trigger.Back, State.PrivateSectorFunding)
            .Permit(Trigger.Change, State.CheckAnswers);

        _machine.Configure(State.AdditionalProjects)
           .Permit(Trigger.Continue, State.CheckAnswers)
            .Permit(Trigger.Back, State.Refinance)
            .Permit(Trigger.Change, State.CheckAnswers);

        _machine.Configure(State.CheckAnswers)
           .PermitIf(Trigger.Continue, State.Complete, () => _model.Funding.CheckAnswers == "Yes")
           .IgnoreIf(Trigger.Continue, () => _model.Funding.CheckAnswers != "Yes")
           .Permit(Trigger.Back, State.AdditionalProjects)
           .OnExit(() =>
           {
               if (_model.Funding.CheckAnswers == "Yes")
               {
                   _model.Funding.SetFlowCompletion(true);
               }
           });

        _machine.Configure(State.Complete)
            .Permit(Trigger.Back, State.CheckAnswers);

        _machine.OnTransitionCompletedAsync(x =>
        {
            _model.Funding.State = x.Destination;

            _model.Funding.RemoveAlternativeRoutesData();

            return _mediator.Send(new Commands.Update() { Model = _model });
        });
    }
}
