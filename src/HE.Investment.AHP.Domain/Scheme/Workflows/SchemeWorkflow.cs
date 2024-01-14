using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Routing;
using Stateless;

namespace HE.Investment.AHP.Domain.Scheme.Workflows;

public class SchemeWorkflow : IStateRouting<SchemeWorkflowState>
{
    private readonly Contract.Scheme.Scheme _scheme;

    private readonly StateMachine<SchemeWorkflowState, Trigger> _machine;

    private readonly bool _isReadOnly;

    public SchemeWorkflow(SchemeWorkflowState currentWorkflowState, Contract.Scheme.Scheme scheme, bool isReadOnly)
    {
        _scheme = scheme;
        _machine = new StateMachine<SchemeWorkflowState, Trigger>(currentWorkflowState);
        _isReadOnly = isReadOnly;
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

    public SchemeWorkflowState CurrentState(SchemeWorkflowState targetState)
    {
        if (_isReadOnly)
        {
            return SchemeWorkflowState.CheckAnswers;
        }

        if (targetState != SchemeWorkflowState.Start || _scheme.Status == SectionStatus.NotStarted)
        {
            return targetState;
        }

#pragma warning disable S2589 // Boolean expressions should not be gratuitous
        return _scheme switch
        {
            { RequiredFunding: var x } when x.IsNotProvided() => SchemeWorkflowState.Funding,
            { HousesToDeliver: var x } when x.IsNotProvided() => SchemeWorkflowState.Funding,
            { AffordabilityEvidence: var x } when x.IsNotProvided() => SchemeWorkflowState.Affordability,
            { SalesRisk: var x } when x.IsNotProvided() => SchemeWorkflowState.SalesRisk,
            { MeetingLocalPriorities: var x } when x.IsNotProvided() => SchemeWorkflowState.HousingNeeds,
            { MeetingLocalHousingNeed: var x } when x.IsNotProvided() => SchemeWorkflowState.HousingNeeds,
            { StakeholderDiscussionsReport: var x } when x.IsNotProvided() => SchemeWorkflowState.StakeholderDiscussions,
            _ => SchemeWorkflowState.CheckAnswers,
        };
#pragma warning restore S2589 // Boolean expressions should not be gratuitous
    }

    private void ConfigureTransitions()
    {
        _machine.Configure(SchemeWorkflowState.Start)
            .Permit(Trigger.Continue, SchemeWorkflowState.Funding);

        ConfigureStep(SchemeWorkflowState.Funding, SchemeWorkflowState.Affordability, SchemeWorkflowState.Start);
        ConfigureStep(SchemeWorkflowState.Affordability, SchemeWorkflowState.SalesRisk, SchemeWorkflowState.Funding);
        ConfigureStep(SchemeWorkflowState.SalesRisk, SchemeWorkflowState.HousingNeeds, SchemeWorkflowState.Affordability);
        ConfigureStep(SchemeWorkflowState.HousingNeeds, SchemeWorkflowState.StakeholderDiscussions, SchemeWorkflowState.SalesRisk);
        ConfigureStep(SchemeWorkflowState.StakeholderDiscussions, SchemeWorkflowState.CheckAnswers, SchemeWorkflowState.HousingNeeds);

        _machine.Configure(SchemeWorkflowState.CheckAnswers)
            .Permit(Trigger.Back, SchemeWorkflowState.StakeholderDiscussions);
    }

    private void ConfigureStep(SchemeWorkflowState current, SchemeWorkflowState next, SchemeWorkflowState previous)
    {
        _machine.Configure(current)
            .PermitIf(Trigger.Continue, next)
            .PermitIf(Trigger.Back, previous)
            .Permit(Trigger.Change, SchemeWorkflowState.CheckAnswers);
    }
}
