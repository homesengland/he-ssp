using System.Diagnostics.CodeAnalysis;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Routing;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.CompanyStructure;
using MediatR;
using Stateless;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;

[SuppressMessage("Ordering Rules", "SA1201", Justification = "Need to refactored in the fure")]
public class CompanyStructureWorkflow
{
    private readonly LoanApplicationViewModel _model;
    private readonly StateMachine<CompanyStructureState, Trigger> _machine;
    private readonly IMediator _mediator;

    public CompanyStructureWorkflow(LoanApplicationViewModel model, IMediator mediator)
    {
        _model = model;
        _machine = new StateMachine<CompanyStructureState, Trigger>(_model.Company.State);
        _mediator = mediator;

        ConfigureTransitions();
    }

    public async void NextState(Trigger trigger)
    {
        await _machine.FireAsync(trigger);
    }

    public bool IsStateComplete()
    {
        return _model.Company.State == CompanyStructureState.Complete;
    }

    public bool IsCompleted()
    {
        return IsStateComplete() || _model.Company.IsFlowCompleted;
    }

    public bool IsStarted()
    {
        return !string.IsNullOrEmpty(_model.Company.Purpose)
            || !string.IsNullOrEmpty(_model.Company.ExistingCompany)
            || !string.IsNullOrEmpty(_model.Company.HomesBuilt);
    }

    public string GetName()
    {
        return Enum.GetName(typeof(CompanyStructureState), _model.Company.State) ?? string.Empty;
    }

    public async void ChangeState(CompanyStructureState state)
    {
        _model.Company.State = state;
        _model.Company.StateChanged = true;
        await _mediator.Send(new Commands.Update() { Model = _model });
    }

    private void ConfigureTransitions()
    {
        _machine.Configure(CompanyStructureState.Index)
            .Permit(Trigger.Continue, CompanyStructureState.Purpose);

        _machine.Configure(CompanyStructureState.Purpose)
          .Permit(Trigger.Continue, CompanyStructureState.ExistingCompany)
          .Permit(Trigger.Change, CompanyStructureState.CheckAnswers)
          .Permit(Trigger.Back, CompanyStructureState.Index);

        _machine.Configure(CompanyStructureState.ExistingCompany)
            .Permit(Trigger.Continue, CompanyStructureState.HomesBuilt)
            .Permit(Trigger.Back, CompanyStructureState.Purpose)
            .Permit(Trigger.Change, CompanyStructureState.CheckAnswers);

        _machine.Configure(CompanyStructureState.HomesBuilt)
            .Permit(Trigger.Continue, CompanyStructureState.CheckAnswers)
            .Permit(Trigger.Back, CompanyStructureState.ExistingCompany)
            .Permit(Trigger.Change, CompanyStructureState.CheckAnswers);

        _machine.Configure(CompanyStructureState.CheckAnswers)
           .PermitIf(Trigger.Continue, CompanyStructureState.Complete, () => _model.Company.CheckAnswers == CommonResponse.Yes)
           .IgnoreIf(Trigger.Continue, () => _model.Company.CheckAnswers != CommonResponse.Yes)
           .PermitIf(Trigger.Back, CompanyStructureState.HomesBuilt)
           .OnExit(() =>
           {
               if (_model.Company.CheckAnswers == CommonResponse.Yes)
               {
                   _model.Company.SetFlowCompletion(true);
               }
           });

        _machine.Configure(CompanyStructureState.Complete)
           .Permit(Trigger.Back, CompanyStructureState.CheckAnswers);

        _machine.OnTransitionCompletedAsync(x =>
        {
            _model.Company.State = x.Destination;
            return _mediator.Send(new Commands.Update { Model = _model });
        });
    }
}
