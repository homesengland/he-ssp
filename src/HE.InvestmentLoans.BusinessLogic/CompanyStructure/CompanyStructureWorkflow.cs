using HE.InvestmentLoans.BusinessLogic.LoanApplication;
using HE.InvestmentLoans.Common.Routing;
using HE.InvestmentLoans.Contract.CompanyStructure;
using Stateless;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure;

public class CompanyStructureWorkflow : IStateRouting<CompanyStructureState>
{
    private readonly StateMachine<CompanyStructureState, Trigger> _machine;

    private readonly CompanyStructureViewModel _model;

    public CompanyStructureWorkflow(CompanyStructureState state, CompanyStructureViewModel model)
    {
        _model = model;
        _machine = new StateMachine<CompanyStructureState, Trigger>(state);
        ConfigureTransitions();
    }

    public async Task<CompanyStructureState> NextState(Trigger trigger)
    {
        await _machine.FireAsync(trigger);
        return _machine.State;
    }

    public Task<bool> StateCanBeAccessed(CompanyStructureState nextState)
    {
        return Task.FromResult(true);
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
            .PermitIf(Trigger.Continue, CompanyStructureState.Complete)
            .PermitIf(Trigger.Back, CompanyStructureState.Complete, () => _model.IsReadOnly())
            .PermitIf(Trigger.Back, CompanyStructureState.HomesBuilt, () => _model.IsEditable());

        _machine.Configure(CompanyStructureState.Complete)
           .Permit(Trigger.Back, CompanyStructureState.CheckAnswers);
    }
}
