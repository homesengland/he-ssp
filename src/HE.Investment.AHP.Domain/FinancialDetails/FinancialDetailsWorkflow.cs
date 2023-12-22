using HE.Investment.AHP.Domain.Scheme.Workflows;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Routing;
using Stateless;

namespace HE.Investment.AHP.Domain.FinancialDetails;

public class FinancialDetailsWorkflow : IStateRouting<FinancialDetailsWorkflowState>
{
    private readonly Contract.FinancialDetails.FinancialDetails _model;

    private readonly StateMachine<FinancialDetailsWorkflowState, Trigger> _machine;

    private readonly bool _isReadOnly;

    public FinancialDetailsWorkflow(
        FinancialDetailsWorkflowState currentFinancialDetailsWorkflowState,
        Contract.FinancialDetails.FinancialDetails model,
        bool isReadOnly)
    {
        _model = model;
        _machine = new StateMachine<FinancialDetailsWorkflowState, Trigger>(currentFinancialDetailsWorkflowState);
        _isReadOnly = isReadOnly;
        ConfigureTransitions();
    }

    public async Task<FinancialDetailsWorkflowState> NextState(Trigger trigger)
    {
        await _machine.FireAsync(trigger);
        return _machine.State;
    }

    public FinancialDetailsWorkflowState CurrentState(FinancialDetailsWorkflowState targetState)
    {
        if (_isReadOnly)
        {
            return FinancialDetailsWorkflowState.CheckAnswers;
        }

        if (targetState != FinancialDetailsWorkflowState.Index || _model.SectionStatus == SectionStatus.NotStarted)
        {
            return targetState;
        }

        return _model switch
        {
            { PurchasePrice: var x } when x.IsNotProvided() => FinancialDetailsWorkflowState.LandStatus,
            { IsSchemaOnPublicLand: var x } when x.IsNotProvided() => FinancialDetailsWorkflowState.LandValue,
            { LandValue: var x } when x.IsNotProvided() => FinancialDetailsWorkflowState.LandValue,
            { ExpectedOnCost: var x } when x.IsNotProvided() => FinancialDetailsWorkflowState.OtherApplicationCosts,
            { ExpectedWorkCost: var x } when x.IsNotProvided() => FinancialDetailsWorkflowState.OtherApplicationCosts,
            { RentalIncomeContribution: var x } when x.IsNotProvided() => FinancialDetailsWorkflowState.Contributions,
            { SubsidyFromSaleOnThisScheme: var x } when x.IsNotProvided() => FinancialDetailsWorkflowState.Contributions,
            { SubsidyFromSaleOnOtherSchemes: var x } when x.IsNotProvided() => FinancialDetailsWorkflowState.Contributions,
            { OwnResourcesContribution: var x } when x.IsNotProvided() => FinancialDetailsWorkflowState.Contributions,
            { RecycledCapitalGarntFundContribution: var x } when x.IsNotProvided() => FinancialDetailsWorkflowState.Contributions,
            { OtherCapitalContributions: var x } when x.IsNotProvided() => FinancialDetailsWorkflowState.Contributions,
            { TransferValueOfHomes: var x } when x.IsNotProvided() => FinancialDetailsWorkflowState.Contributions,
            { CountyCouncilGrants: var x } when x.IsNotProvided() => FinancialDetailsWorkflowState.Grants,
            { DHSCExtraCareGrants: var x } when x.IsNotProvided() => FinancialDetailsWorkflowState.Grants,
            { LocalAuthorityGrants: var x } when x.IsNotProvided() => FinancialDetailsWorkflowState.Grants,
            { SocialServicesGrants: var x } when x.IsNotProvided() => FinancialDetailsWorkflowState.Grants,
            { HealthRelatedGrants: var x } when x.IsNotProvided() => FinancialDetailsWorkflowState.Grants,
            { LotteryFunding: var x } when x.IsNotProvided() => FinancialDetailsWorkflowState.Grants,
            { OtherPublicGrants: var x } when x.IsNotProvided() => FinancialDetailsWorkflowState.Grants,
            _ => FinancialDetailsWorkflowState.CheckAnswers,
        };
    }

    public Task<bool> StateCanBeAccessed(FinancialDetailsWorkflowState nextState)
    {
        return Task.FromResult(true);
    }

    private void ConfigureTransitions()
    {
        _machine.Configure(FinancialDetailsWorkflowState.Index)
            .Permit(Trigger.Continue, FinancialDetailsWorkflowState.LandStatus);

        _machine.Configure(FinancialDetailsWorkflowState.LandStatus)
            .Permit(Trigger.Continue, FinancialDetailsWorkflowState.LandValue)
            .Permit(Trigger.Back, FinancialDetailsWorkflowState.Index);

        _machine.Configure(FinancialDetailsWorkflowState.LandValue)
            .Permit(Trigger.Continue, FinancialDetailsWorkflowState.OtherApplicationCosts)
            .Permit(Trigger.Back, FinancialDetailsWorkflowState.LandStatus);

        _machine.Configure(FinancialDetailsWorkflowState.OtherApplicationCosts)
           .Permit(Trigger.Continue, FinancialDetailsWorkflowState.Contributions)
           .Permit(Trigger.Back, FinancialDetailsWorkflowState.LandValue);

        _machine.Configure(FinancialDetailsWorkflowState.Contributions)
           .Permit(Trigger.Continue, FinancialDetailsWorkflowState.Grants)
           .Permit(Trigger.Back, FinancialDetailsWorkflowState.OtherApplicationCosts);

        _machine.Configure(FinancialDetailsWorkflowState.Grants)
           .Permit(Trigger.Continue, FinancialDetailsWorkflowState.CheckAnswers)
           .Permit(Trigger.Back, FinancialDetailsWorkflowState.Contributions);

        _machine.Configure(FinancialDetailsWorkflowState.CheckAnswers)
          .Permit(Trigger.Back, FinancialDetailsWorkflowState.Grants);
    }
}
