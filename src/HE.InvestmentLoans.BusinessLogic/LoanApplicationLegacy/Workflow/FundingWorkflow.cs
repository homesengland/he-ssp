using System.Diagnostics.CodeAnalysis;
using System.Security;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Routing;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.Funding;
using HE.InvestmentLoans.Contract.Funding.Enums;
using MediatR;
using Stateless;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;

[SuppressMessage("Ordering Rules", "SA1201", Justification = "Need to refactored in the fure")]
public class FundingWorkflow : IStateRouting<FundingState>
{
    private readonly LoanApplicationViewModel _model;
    private readonly StateMachine<FundingState, Trigger> _machine;
    private readonly IMediator _mediator;

    public FundingWorkflow(LoanApplicationViewModel model, IMediator mediator)
    {
        _model = model;
        _machine = new StateMachine<FundingState, Trigger>(FundingState.Index);
        _mediator = mediator;

        ConfigureTransitions();
    }

    public FundingWorkflow(LoanApplicationId applicationId, IMediator mediator, FundingState currentState)
    {
        _mediator = mediator;
        _model = new LoanApplicationViewModel { GoodChangeMode = true };

        _machine = new StateMachine<FundingState, Trigger>(currentState);

        ConfigureTransitions();
    }

    public bool IsStateComplete()
    {
        return _model.Funding.State == SectionStatus.Completed;
    }

    public bool IsCompleted()
    {
        return IsStateComplete() || _model.Funding.IsFlowCompleted;
    }

    public bool IsStarted()
    {
        return !string.IsNullOrEmpty(_model.Funding.GrossDevelopmentValue)
            || !string.IsNullOrEmpty(_model.Funding.TotalCosts)
            || !string.IsNullOrEmpty(_model.Funding.AbnormalCosts)
            || !string.IsNullOrEmpty(_model.Funding.PrivateSectorFunding)
            || !string.IsNullOrEmpty(_model.Funding.Refinance)
            || !string.IsNullOrEmpty(_model.Funding.AdditionalProjects);
    }

    public string GetName()
    {
        return Enum.GetName(typeof(FundingState), _model.Funding.State) ?? string.Empty;
    }

    public async void ChangeState(FundingState state)
    {
        _model.Funding.StateChanged = true;
        await _mediator.Send(new Commands.Update() { Model = _model });
    }

    public async Task<FundingState> NextState(Trigger trigger)
    {
        await _machine.FireAsync(trigger);

        return _machine.State;
    }

    public Task<bool> StateCanBeAccessed(FundingState nextState)
    {
        return Task.FromResult(true);
    }

    private void ConfigureTransitions()
    {
        _machine.Configure(FundingState.Index)
          .Permit(Trigger.Continue, FundingState.GDV);

        _machine.Configure(FundingState.GDV)
            .Permit(Trigger.Continue, FundingState.TotalCosts)
            .Permit(Trigger.Back, FundingState.Index)
            .Permit(Trigger.Change, FundingState.CheckAnswers);

        _machine.Configure(FundingState.TotalCosts)
            .Permit(Trigger.Continue, FundingState.AbnormalCosts)
            .Permit(Trigger.Back, FundingState.GDV)
            .Permit(Trigger.Change, FundingState.CheckAnswers);

        _machine.Configure(FundingState.AbnormalCosts)
            .Permit(Trigger.Continue, FundingState.PrivateSectorFunding)
            .Permit(Trigger.Back, FundingState.TotalCosts)
            .Permit(Trigger.Change, FundingState.CheckAnswers);

        _machine.Configure(FundingState.PrivateSectorFunding)
            .Permit(Trigger.Continue, FundingState.Refinance)
            .Permit(Trigger.Back, FundingState.AbnormalCosts)
            .Permit(Trigger.Change, FundingState.CheckAnswers);

        _machine.Configure(FundingState.Refinance)
            .Permit(Trigger.Continue, FundingState.AdditionalProjects)
            .Permit(Trigger.Back, FundingState.PrivateSectorFunding)
            .Permit(Trigger.Change, FundingState.CheckAnswers);

        _machine.Configure(FundingState.AdditionalProjects)
           .Permit(Trigger.Continue, FundingState.CheckAnswers)
            .Permit(Trigger.Back, FundingState.Refinance)
            .Permit(Trigger.Change, FundingState.CheckAnswers);

        _machine.Configure(FundingState.CheckAnswers)
           .PermitIf(Trigger.Continue, FundingState.Complete, () => _model.Funding.CheckAnswers == CommonResponse.Yes)
           .IgnoreIf(Trigger.Continue, () => _model.Funding.CheckAnswers != CommonResponse.Yes)
           .Permit(Trigger.Back, FundingState.AdditionalProjects)
           .OnExit(() =>
           {
               if (_model.Funding.CheckAnswers == CommonResponse.Yes)
               {
                   _model.Funding.SetFlowCompletion(true);
               }
           });

        _machine.Configure(FundingState.Complete)
            .Permit(Trigger.Back, FundingState.CheckAnswers);

        _machine.OnTransitionCompletedAsync(x =>
        {
            if (_model.GoodChangeMode)
            {
                return Task.CompletedTask;
            }

            _model.Funding.RemoveAlternativeRoutesData();

            return _mediator.Send(new Commands.Update() { Model = _model });
        });
    }
}
