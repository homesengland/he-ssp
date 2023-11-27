using HE.Investment.AHP.Contract.Scheme;
using HE.Investments.Loans.Common.Routing;
using Stateless;

namespace HE.Investment.AHP.Domain.Scheme.Workflows;

public class SchemeWorkflow : IStateRouting<SchemeWorkflowState>
{
    private readonly bool _isCheckAnswersMode;

    private readonly StateMachine<SchemeWorkflowState, Trigger> _machine;

    public SchemeWorkflow(SchemeWorkflowState currentWorkflowState, bool isCheckAnswersMode = false)
    {
        _isCheckAnswersMode = isCheckAnswersMode;
        _machine = new StateMachine<SchemeWorkflowState, Trigger>(currentWorkflowState);
        ConfigureTransitions();
    }

    public async Task<SchemeWorkflowState> NextState(Trigger trigger)
    {
        await _machine.FireAsync(trigger);
        return _machine.State;
    }

    public Task<bool> StateCanBeAccessed(SchemeWorkflowState nextState)
    {
        return Task.FromResult(true);
    }

    private void ConfigureTransitions()
    {
        _machine.Configure(SchemeWorkflowState.Index)
            .Permit(Trigger.Continue, SchemeWorkflowState.Funding);

        ConfigureStep(SchemeWorkflowState.Funding, SchemeWorkflowState.Affordability, SchemeWorkflowState.Index);
        ConfigureStep(SchemeWorkflowState.Affordability, SchemeWorkflowState.SalesRisk, SchemeWorkflowState.Funding);
        ConfigureStep(SchemeWorkflowState.SalesRisk, SchemeWorkflowState.HousingNeeds, SchemeWorkflowState.Affordability);
        ConfigureStep(SchemeWorkflowState.HousingNeeds, SchemeWorkflowState.StakeholderDiscussions, SchemeWorkflowState.SalesRisk);

        _machine.Configure(SchemeWorkflowState.StakeholderDiscussions)
            .Permit(Trigger.Continue, SchemeWorkflowState.CheckAnswers)
            .Permit(Trigger.Back, SchemeWorkflowState.HousingNeeds);

        _machine.Configure(SchemeWorkflowState.CheckAnswers)
            .Permit(Trigger.Back, SchemeWorkflowState.StakeholderDiscussions);
    }

    private void ConfigureStep(SchemeWorkflowState current, SchemeWorkflowState next, SchemeWorkflowState previous)
    {
        _machine.Configure(current)
            .PermitIf(Trigger.Continue, next, () => !_isCheckAnswersMode)
            .PermitIf(Trigger.Continue, SchemeWorkflowState.CheckAnswers, () => _isCheckAnswersMode)
            .PermitIf(Trigger.Back, previous, () => !_isCheckAnswersMode)
            .PermitIf(Trigger.Back, SchemeWorkflowState.CheckAnswers, () => _isCheckAnswersMode);
    }
}
