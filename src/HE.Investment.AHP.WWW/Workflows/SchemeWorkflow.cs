using HE.Investment.AHP.Contract.Scheme;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Routing;
using Stateless;

namespace HE.Investment.AHP.WWW.Workflows;

public class SchemeWorkflow : IStateRouting<SchemeWorkflowState>
{
    private readonly Scheme _scheme;

    private readonly StateMachine<SchemeWorkflowState, Trigger> _machine;

    public SchemeWorkflow(SchemeWorkflowState currentWorkflowState, Scheme scheme)
    {
        _scheme = scheme;
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
        if (nextState == SchemeWorkflowState.PartnerDetails && !_scheme.IsConsortiumMember)
        {
            return Task.FromResult(false);
        }

        return Task.FromResult(true);
    }

    public SchemeWorkflowState CurrentState(SchemeWorkflowState targetState)
    {
        if (_scheme.Application.IsReadOnly)
        {
            return SchemeWorkflowState.CheckAnswers;
        }

        if (targetState != SchemeWorkflowState.Start || _scheme.Status == SectionStatus.NotStarted)
        {
            return targetState;
        }

        return _scheme switch
        {
            { RequiredFunding: var x } when x.IsNotProvided() => SchemeWorkflowState.Funding,
            { HousesToDeliver: var x } when x.IsNotProvided() => SchemeWorkflowState.Funding,
            { IsConsortiumMember: true, ArePartnersConfirmed: var arePartnersConfirmed } when arePartnersConfirmed != true =>
                SchemeWorkflowState.PartnerDetails,
            { AffordabilityEvidence: var x } when x.IsNotProvided() => SchemeWorkflowState.Affordability,
            { SalesRisk: var x } when x.IsNotProvided() => SchemeWorkflowState.SalesRisk,
            { MeetingLocalPriorities: var x } when x.IsNotProvided() => SchemeWorkflowState.HousingNeeds,
            { MeetingLocalHousingNeed: var x } when x.IsNotProvided() => SchemeWorkflowState.HousingNeeds,
            { StakeholderDiscussionsReport: var x } when x.IsNotProvided() => SchemeWorkflowState.StakeholderDiscussions,
            _ => SchemeWorkflowState.CheckAnswers,
        };
    }

    private void ConfigureTransitions()
    {
        _machine.Configure(SchemeWorkflowState.Start)
            .Permit(Trigger.Continue, SchemeWorkflowState.Funding);

        _machine.Configure(SchemeWorkflowState.Funding)
            .PermitIf(Trigger.Continue, SchemeWorkflowState.Affordability, () => !_scheme.IsConsortiumMember)
            .PermitIf(Trigger.Continue, SchemeWorkflowState.PartnerDetails, () => _scheme.IsConsortiumMember)
            .Permit(Trigger.Back, SchemeWorkflowState.Start);

        _machine.Configure(SchemeWorkflowState.PartnerDetails)
            .Permit(Trigger.Continue, SchemeWorkflowState.Affordability)
            .Permit(Trigger.Back, SchemeWorkflowState.Funding);

        _machine.Configure(SchemeWorkflowState.Affordability)
            .Permit(Trigger.Continue, SchemeWorkflowState.SalesRisk)
            .PermitIf(Trigger.Back, SchemeWorkflowState.Funding, () => !_scheme.IsConsortiumMember)
            .PermitIf(Trigger.Back, SchemeWorkflowState.PartnerDetails, () => _scheme.IsConsortiumMember);

        _machine.Configure(SchemeWorkflowState.SalesRisk)
            .Permit(Trigger.Continue, SchemeWorkflowState.HousingNeeds)
            .Permit(Trigger.Back, SchemeWorkflowState.Affordability);

        _machine.Configure(SchemeWorkflowState.HousingNeeds)
            .Permit(Trigger.Continue, SchemeWorkflowState.StakeholderDiscussions)
            .Permit(Trigger.Back, SchemeWorkflowState.SalesRisk);

        _machine.Configure(SchemeWorkflowState.StakeholderDiscussions)
            .Permit(Trigger.Continue, SchemeWorkflowState.CheckAnswers)
            .Permit(Trigger.Back, SchemeWorkflowState.HousingNeeds);

        _machine.Configure(SchemeWorkflowState.CheckAnswers)
            .Permit(Trigger.Back, SchemeWorkflowState.StakeholderDiscussions);
    }
}
