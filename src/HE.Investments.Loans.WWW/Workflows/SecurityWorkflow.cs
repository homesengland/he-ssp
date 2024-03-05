using HE.Investments.Common.Contract.Constants;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Loans.Contract.Security;
using Stateless;

namespace HE.Investments.Loans.WWW.Workflows;

public class SecurityWorkflow : WorkflowBase<SecurityState, SecurityViewModel>
{
    public SecurityWorkflow(SecurityState currentState, SecurityViewModel model)
    {
        Model = model;
        Machine = new StateMachine<SecurityState, Trigger>(currentState);

        ConfigureTransitions();
    }

    public override SecurityState CurrentState(SecurityState targetState)
    {
        if (Model.IsReadOnly())
        {
            return SecurityState.CheckAnswers;
        }

        if (targetState != SecurityState.Index || !IsSectionStarted())
        {
            return targetState;
        }

        return Model switch
        {
            { ChargesDebtCompany: var x } when x.IsNotProvided() => SecurityState.ChargesDebtCompany,
            { DirLoans: var x } when x.IsNotProvided() => SecurityState.DirLoans,
            { DirLoans: CommonResponse.Yes, DirLoansSub: var dirLoansSub } when dirLoansSub.IsNotProvided() => SecurityState.DirLoansSub,
            _ => SecurityState.CheckAnswers,
        };
    }

    private void ConfigureTransitions()
    {
        Machine.Configure(SecurityState.Index)
          .Permit(Trigger.Continue, SecurityState.ChargesDebtCompany);

        Machine.Configure(SecurityState.ChargesDebtCompany)
            .Permit(Trigger.Continue, SecurityState.DirLoans)
            .PermitIf(Trigger.Back, SecurityState.TaskList, IsSectionStarted)
            .PermitIf(Trigger.Back, SecurityState.Index, () => !IsSectionStarted())
            .Permit(Trigger.Change, SecurityState.CheckAnswers);

        Machine.Configure(SecurityState.DirLoans)
            .PermitIf(Trigger.Continue, SecurityState.DirLoansSub, () => Model.DirLoans == CommonResponse.Yes)
            .PermitIf(Trigger.Continue, SecurityState.CheckAnswers, () => Model.DirLoans != CommonResponse.Yes)
            .PermitIf(Trigger.Change, SecurityState.DirLoansSub, () => Model.DirLoans == CommonResponse.Yes)
            .PermitIf(Trigger.Change, SecurityState.CheckAnswers, () => Model.DirLoans != CommonResponse.Yes)
            .Permit(Trigger.Back, SecurityState.ChargesDebtCompany);

        Machine.Configure(SecurityState.DirLoansSub)
            .Permit(Trigger.Continue, SecurityState.CheckAnswers)
            .Permit(Trigger.Back, SecurityState.DirLoans)
            .Permit(Trigger.Change, SecurityState.CheckAnswers);

        Machine.Configure(SecurityState.CheckAnswers)
           .Permit(Trigger.Continue, SecurityState.Complete)
           .PermitIf(Trigger.Back, SecurityState.Complete, () => Model.IsReadOnly())
           .PermitIf(Trigger.Back, SecurityState.DirLoansSub, () => Model.IsEditable() && Model.DirLoans == CommonResponse.Yes)
           .PermitIf(Trigger.Back, SecurityState.DirLoans, () => Model.IsEditable() && Model.DirLoans != CommonResponse.Yes);

        Machine.Configure(SecurityState.Complete)
          .Permit(Trigger.Back, SecurityState.CheckAnswers);
    }
}
