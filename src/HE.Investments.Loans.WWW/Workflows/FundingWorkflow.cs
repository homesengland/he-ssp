using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Loans.Contract.Funding;
using HE.Investments.Loans.Contract.Funding.Enums;
using Stateless;

namespace HE.Investments.Loans.WWW.Workflows;

public class FundingWorkflow : WorkflowBase<FundingState, FundingViewModel>
{
    public FundingWorkflow(FundingState currentState, FundingViewModel fundingViewModel)
    {
        Model = fundingViewModel;
        Machine = new StateMachine<FundingState, Trigger>(currentState);

        ConfigureTransitions();
    }

    public override FundingState CurrentState(FundingState targetState)
    {
        if (Model.IsReadOnly())
        {
            return FundingState.CheckAnswers;
        }

        if (targetState != FundingState.Index || !IsSectionStarted())
        {
            return targetState;
        }

        return Model switch
        {
            { GrossDevelopmentValue: var x } when x.IsNotProvided() => FundingState.GDV,
            { TotalCosts: var x } when x.IsNotProvided() => FundingState.TotalCosts,
            { AbnormalCosts: var x } when x.IsNotProvided() => FundingState.AbnormalCosts,
            { PrivateSectorFunding: var x } when x.IsNotProvided() => FundingState.PrivateSectorFunding,
            { Refinance: var x } when x.IsNotProvided() => FundingState.Refinance,
            { AdditionalProjects: var x } when x.IsNotProvided() => FundingState.AdditionalProjects,
            _ => FundingState.CheckAnswers,
        };
    }

    private void ConfigureTransitions()
    {
        Machine.Configure(FundingState.Index)
            .Permit(Trigger.Continue, FundingState.GDV);

        Machine.Configure(FundingState.GDV)
            .Permit(Trigger.Continue, FundingState.TotalCosts)
            .PermitIf(Trigger.Back, FundingState.TaskList, IsSectionStarted)
            .PermitIf(Trigger.Back, FundingState.Index, () => !IsSectionStarted())
            .Permit(Trigger.Change, FundingState.CheckAnswers);

        Machine.Configure(FundingState.TotalCosts)
            .Permit(Trigger.Continue, FundingState.AbnormalCosts)
            .Permit(Trigger.Back, FundingState.GDV)
            .Permit(Trigger.Change, FundingState.CheckAnswers);

        Machine.Configure(FundingState.AbnormalCosts)
            .Permit(Trigger.Continue, FundingState.PrivateSectorFunding)
            .Permit(Trigger.Back, FundingState.TotalCosts)
            .Permit(Trigger.Change, FundingState.CheckAnswers);

        Machine.Configure(FundingState.PrivateSectorFunding)
            .Permit(Trigger.Continue, FundingState.Refinance)
            .Permit(Trigger.Back, FundingState.AbnormalCosts)
            .Permit(Trigger.Change, FundingState.CheckAnswers);

        Machine.Configure(FundingState.Refinance)
            .Permit(Trigger.Continue, FundingState.AdditionalProjects)
            .Permit(Trigger.Back, FundingState.PrivateSectorFunding)
            .Permit(Trigger.Change, FundingState.CheckAnswers);

        Machine.Configure(FundingState.AdditionalProjects)
           .Permit(Trigger.Continue, FundingState.CheckAnswers)
            .Permit(Trigger.Back, FundingState.Refinance)
            .Permit(Trigger.Change, FundingState.CheckAnswers);

        Machine.Configure(FundingState.CheckAnswers)
           .Permit(Trigger.Continue, FundingState.Complete)
           .PermitIf(Trigger.Back, FundingState.AdditionalProjects, () => Model.IsEditable())
           .PermitIf(Trigger.Back, FundingState.Complete, () => Model.IsReadOnly());

        Machine.Configure(FundingState.Complete)
            .Permit(Trigger.Back, FundingState.CheckAnswers);
    }
}
