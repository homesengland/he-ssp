using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Routing;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Security;
using Stateless;

namespace HE.InvestmentLoans.BusinessLogic.Security;

public class SecurityWorkflow : IStateRouting<SecurityState>
{
    private readonly StateMachine<SecurityState, Trigger> _machine;
    private readonly SecurityViewModel _model;

    public SecurityWorkflow(SecurityState currentState, SecurityViewModel model)
    {
        _model = model;
        _machine = new StateMachine<SecurityState, Trigger>(currentState);

        ConfigureTransitions();
    }

    public async Task<SecurityState> NextState(Trigger trigger)
    {
        await _machine.FireAsync(trigger);

        return _machine.State;
    }

    public Task<bool> StateCanBeAccessed(SecurityState nextState)
    {
        return Task.FromResult(true);
    }

    public SecurityState CurrentState(SecurityState targetState)
    {
        if (_model.IsReadOnly())
        {
            return SecurityState.CheckAnswers;
        }

        if (targetState != SecurityState.Index || _model.State == SectionStatus.NotStarted)
        {
            return targetState;
        }

        return _model switch
        {
            { ChargesDebtCompany: var x } when x.IsNotProvided() => SecurityState.ChargesDebtCompany,
            { DirLoans: var x } when x.IsNotProvided() => SecurityState.DirLoans,
            { DirLoans: CommonResponse.Yes, DirLoansSub: var dirLoansSub } when dirLoansSub.IsNotProvided() => SecurityState.DirLoansSub,
            _ => SecurityState.CheckAnswers,
        };
    }

    private void ConfigureTransitions()
    {
        _machine.Configure(SecurityState.Index)
          .Permit(Trigger.Continue, SecurityState.ChargesDebtCompany);

        _machine.Configure(SecurityState.ChargesDebtCompany)
            .Permit(Trigger.Continue, SecurityState.DirLoans)
            .Permit(Trigger.Back, SecurityState.Index)
            .Permit(Trigger.Change, SecurityState.CheckAnswers);

        _machine.Configure(SecurityState.DirLoans)
            .PermitIf(Trigger.Continue, SecurityState.DirLoansSub, () => _model.DirLoans == CommonResponse.Yes)
            .PermitIf(Trigger.Continue, SecurityState.CheckAnswers, () => _model.DirLoans != CommonResponse.Yes)
            .PermitIf(Trigger.Change, SecurityState.DirLoansSub, () => _model.DirLoans == CommonResponse.Yes)
            .PermitIf(Trigger.Change, SecurityState.CheckAnswers, () => _model.DirLoans != CommonResponse.Yes)
            .Permit(Trigger.Back, SecurityState.ChargesDebtCompany);

        _machine.Configure(SecurityState.DirLoansSub)
            .Permit(Trigger.Continue, SecurityState.CheckAnswers)
            .Permit(Trigger.Back, SecurityState.DirLoans)
            .Permit(Trigger.Change, SecurityState.CheckAnswers);

        _machine.Configure(SecurityState.CheckAnswers)
           .Permit(Trigger.Continue, SecurityState.Complete)
           .PermitIf(Trigger.Back, SecurityState.Complete, () => _model.IsReadOnly())
           .PermitIf(Trigger.Back, SecurityState.DirLoansSub, () => _model.IsEditable() && _model.DirLoans == CommonResponse.Yes)
           .PermitIf(Trigger.Back, SecurityState.DirLoans, () => _model.IsEditable() && _model.DirLoans != CommonResponse.Yes);

        _machine.Configure(SecurityState.Complete)
          .Permit(Trigger.Back, SecurityState.CheckAnswers);
    }
}
