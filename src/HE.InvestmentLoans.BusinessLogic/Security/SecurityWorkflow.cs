using System.Diagnostics.CodeAnalysis;
using HE.InvestmentLoans.BusinessLogic.LoanApplication;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Routing;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.Security;
using MediatR;
using Stateless;

namespace HE.InvestmentLoans.BusinessLogic.Security;

public class SecurityWorkflow : IStateRouting<SecurityState>
{
    private readonly LoanApplicationViewModel _model;
    private readonly StateMachine<SecurityState, Trigger> _machine;
    private readonly SecurityViewModel _model2;

    public SecurityWorkflow(LoanApplicationViewModel model)
    {
        _model = model;
        _machine = new StateMachine<SecurityState, Trigger>(SecurityState.Index);

        ConfigureTransitions();
    }

    public SecurityWorkflow(SecurityState currentState, SecurityViewModel model)
    {
        _model = new LoanApplicationViewModel { GoodChangeMode = true };
        _model2 = model;
        _machine = new StateMachine<SecurityState, Trigger>(currentState);

        ConfigureTransitions();
    }

    public string GetName()
    {
        return Enum.GetName(typeof(SecurityState), _model.Security.State) ?? string.Empty;
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

    private void ConfigureTransitions()
    {
        _machine.Configure(SecurityState.Index)
          .Permit(Trigger.Continue, SecurityState.ChargesDebtCompany);

        _machine.Configure(SecurityState.ChargesDebtCompany)
            .Permit(Trigger.Continue, SecurityState.DirLoans)
            .Permit(Trigger.Back, SecurityState.Index)
            .Permit(Trigger.Change, SecurityState.CheckAnswers);

        _machine.Configure(SecurityState.DirLoans)
            .PermitIf(Trigger.Continue, SecurityState.DirLoansSub, () => _model2.DirLoans == CommonResponse.Yes)
            .PermitIf(Trigger.Continue, SecurityState.CheckAnswers, () => _model2.DirLoans != CommonResponse.Yes)
            .PermitIf(Trigger.Change, SecurityState.DirLoansSub, () => _model2.DirLoans == CommonResponse.Yes)
            .PermitIf(Trigger.Change, SecurityState.CheckAnswers, () => _model2.DirLoans != CommonResponse.Yes)
            .Permit(Trigger.Back, SecurityState.ChargesDebtCompany);

        _machine.Configure(SecurityState.DirLoansSub)
            .Permit(Trigger.Continue, SecurityState.CheckAnswers)
            .Permit(Trigger.Back, SecurityState.DirLoans)
            .Permit(Trigger.Change, SecurityState.CheckAnswers);

        _machine.Configure(SecurityState.CheckAnswers)
           .Permit(Trigger.Continue, SecurityState.Complete)
           .PermitIf(Trigger.Back, SecurityState.Complete, () => _model2.IsReadOnly())
           .PermitIf(Trigger.Back, SecurityState.DirLoansSub, () => _model2.IsEditable() && _model2.DirLoans == CommonResponse.Yes)
           .PermitIf(Trigger.Back, SecurityState.DirLoans, () => _model2.IsEditable() && _model2.DirLoans != CommonResponse.Yes);

        _machine.Configure(SecurityState.Complete)
          .Permit(Trigger.Back, SecurityState.CheckAnswers);
    }
}
