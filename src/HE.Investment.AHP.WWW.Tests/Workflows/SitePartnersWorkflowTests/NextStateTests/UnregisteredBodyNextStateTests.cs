using FluentAssertions;
using HE.Investment.AHP.Contract.SitePartners;
using HE.Investments.Common.WWW.Routing;

namespace HE.Investment.AHP.WWW.Tests.Workflows.SitePartnersWorkflowTests.NextStateTests;

public class UnregisteredBodyNextStateTests
{
    [Theory]
    [InlineData(SitePartnersWorkflowState.FlowStarted, SitePartnersWorkflowState.UnregisteredBodySearch)]
    [InlineData(SitePartnersWorkflowState.UnregisteredBodySearch, SitePartnersWorkflowState.UnregisteredBodySearchResult)]
    [InlineData(SitePartnersWorkflowState.UnregisteredBodySearchResult, SitePartnersWorkflowState.UnregisteredBodyConfirm)]
    [InlineData(SitePartnersWorkflowState.UnregisteredBodyCreateManual, SitePartnersWorkflowState.UnregisteredBodyConfirm)]
    [InlineData(SitePartnersWorkflowState.UnregisteredBodyConfirm, SitePartnersWorkflowState.FlowFinished)]
    public async Task ShouldReturnNextState_WhenContinueTriggerIsExecuted(SitePartnersWorkflowState currentState, SitePartnersWorkflowState expectedState)
    {
        // given
        var testCandidate = new SitePartnersWorkflowBuilder(currentState).WithIsUnregisteredBody().Build();

        // when
        var result = await testCandidate.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedState);
    }

    [Theory]
    [InlineData(SitePartnersWorkflowState.FlowFinished, SitePartnersWorkflowState.UnregisteredBodyConfirm)]
    [InlineData(SitePartnersWorkflowState.UnregisteredBodyConfirm, SitePartnersWorkflowState.UnregisteredBodySearch)]
    [InlineData(SitePartnersWorkflowState.UnregisteredBodyCreateManual, SitePartnersWorkflowState.UnregisteredBodySearch)]
    [InlineData(SitePartnersWorkflowState.UnregisteredBodySearchNoResults, SitePartnersWorkflowState.UnregisteredBodySearch)]
    [InlineData(SitePartnersWorkflowState.UnregisteredBodySearchResult, SitePartnersWorkflowState.UnregisteredBodySearch)]
    [InlineData(SitePartnersWorkflowState.UnregisteredBodySearch, SitePartnersWorkflowState.FlowStarted)]
    public async Task ShouldReturnNextState_WhenBackTriggerIsExecuted(SitePartnersWorkflowState currentState, SitePartnersWorkflowState expectedState)
    {
        // given
        var testCandidate = new SitePartnersWorkflowBuilder(currentState).WithIsUnregisteredBody().Build();

        // when
        var result = await testCandidate.NextState(Trigger.Back);

        // then
        result.Should().Be(expectedState);
    }
}
