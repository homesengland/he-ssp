using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Loans.Contract.CompanyStructure;
using Stateless;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure;

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

    public CompanyStructureState CurrentState(CompanyStructureState targetState)
    {
        if (_model.IsReadOnly())
        {
            return CompanyStructureState.CheckAnswers;
        }

        if (targetState != CompanyStructureState.StartCompanyStructure || _model.State == SectionStatus.NotStarted)
        {
            return targetState;
        }

        return _model switch
        {
            { Purpose: var x } when x.IsNotProvided() => CompanyStructureState.Purpose,
            { OrganisationMoreInformation: var x } when x.IsNotProvided() => CompanyStructureState.ExistingCompany,
            { HomesBuilt: var x } when x.IsNotProvided() => CompanyStructureState.HomesBuilt,
            _ => CompanyStructureState.CheckAnswers,
        };
    }

    private void ConfigureTransitions()
    {
        _machine.Configure(CompanyStructureState.StartCompanyStructure)
            .Permit(Trigger.Continue, CompanyStructureState.Purpose);

        _machine.Configure(CompanyStructureState.Purpose)
          .Permit(Trigger.Continue, CompanyStructureState.ExistingCompany)
          .Permit(Trigger.Change, CompanyStructureState.CheckAnswers)
          .Permit(Trigger.Back, CompanyStructureState.StartCompanyStructure);

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
