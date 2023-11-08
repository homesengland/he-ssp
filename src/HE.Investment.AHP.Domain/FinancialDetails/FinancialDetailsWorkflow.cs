using HE.Investment.AHP.Contract.FinancialDetails.Models;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.InvestmentLoans.Common.Routing;
using Stateless;

namespace HE.Investment.AHP.Domain.FinancialDetails;

public class FinancialDetailsWorkflow : IStateRouting<FinancialDetailsWorkflowState>
{
    private readonly StateMachine<FinancialDetailsWorkflowState, Trigger> _machine;

    public FinancialDetailsWorkflow(FinancialDetailsWorkflowState currentFinancialDetailsWorkflowState)
    {
        _machine = new StateMachine<FinancialDetailsWorkflowState, Trigger>(currentFinancialDetailsWorkflowState);
        ConfigureTransitions();
    }

    public FinancialDetailsWorkflow()
        : this(FinancialDetailsWorkflowState.Index)
    {
    }

    public async Task<FinancialDetailsWorkflowState> NextState(Trigger trigger)
    {
        await _machine.FireAsync(trigger);
        return _machine.State;
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
            .Permit(Trigger.Continue, FinancialDetailsWorkflowState.OtherSchemeCost)
            .Permit(Trigger.Back, FinancialDetailsWorkflowState.LandStatus);

        _machine.Configure(FinancialDetailsWorkflowState.OtherSchemeCost)
           .Permit(Trigger.Continue, FinancialDetailsWorkflowState.ExpectedContributions)
           .Permit(Trigger.Back, FinancialDetailsWorkflowState.LandValue);

        _machine.Configure(FinancialDetailsWorkflowState.ExpectedContributions)
           .Permit(Trigger.Continue, FinancialDetailsWorkflowState.Grants)
           .Permit(Trigger.Back, FinancialDetailsWorkflowState.OtherSchemeCost);

        _machine.Configure(FinancialDetailsWorkflowState.Grants)
           .Permit(Trigger.Continue, FinancialDetailsWorkflowState.CheckFinancialDetails)
           .Permit(Trigger.Back, FinancialDetailsWorkflowState.ExpectedContributions);

        _machine.Configure(FinancialDetailsWorkflowState.CheckFinancialDetails)
          .Permit(Trigger.Back, FinancialDetailsWorkflowState.Grants);
    }
}
