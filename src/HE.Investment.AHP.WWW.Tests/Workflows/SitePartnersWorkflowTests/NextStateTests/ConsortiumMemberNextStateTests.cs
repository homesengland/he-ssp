using FluentAssertions;
using HE.Investment.AHP.Contract.SitePartners;
using HE.Investments.Common.WWW.Routing;

namespace HE.Investment.AHP.WWW.Tests.Workflows.SitePartnersWorkflowTests.NextStateTests;

public class ConsortiumMemberNextStateTests
{
    [Theory]
    [InlineData(SitePartnersWorkflowState.FlowStarted, SitePartnersWorkflowState.DevelopingPartner)]
    [InlineData(SitePartnersWorkflowState.DevelopingPartner, SitePartnersWorkflowState.DevelopingPartnerConfirm)]
    [InlineData(SitePartnersWorkflowState.DevelopingPartnerConfirm, SitePartnersWorkflowState.OwnerOfTheLand)]
    [InlineData(SitePartnersWorkflowState.OwnerOfTheLand, SitePartnersWorkflowState.OwnerOfTheLandConfirm)]
    [InlineData(SitePartnersWorkflowState.OwnerOfTheLandConfirm, SitePartnersWorkflowState.OwnerOfTheHomes)]
    [InlineData(SitePartnersWorkflowState.OwnerOfTheHomes, SitePartnersWorkflowState.OwnerOfTheHomesConfirm)]
    [InlineData(SitePartnersWorkflowState.OwnerOfTheHomesConfirm, SitePartnersWorkflowState.FlowFinished)]
    public async Task ShouldReturnNextState_WhenContinueTriggerIsExecuted(SitePartnersWorkflowState currentState, SitePartnersWorkflowState expectedState)
    {
        // given
        var testCandidate = new SitePartnersWorkflowBuilder(currentState).WithIsConsortiumMember().Build();

        // when
        var result = await testCandidate.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedState);
    }

    [Theory]
    [InlineData(SitePartnersWorkflowState.FlowFinished, SitePartnersWorkflowState.OwnerOfTheHomes)]
    [InlineData(SitePartnersWorkflowState.OwnerOfTheHomesConfirm, SitePartnersWorkflowState.OwnerOfTheHomes)]
    [InlineData(SitePartnersWorkflowState.OwnerOfTheHomes, SitePartnersWorkflowState.OwnerOfTheLand)]
    [InlineData(SitePartnersWorkflowState.OwnerOfTheLandConfirm, SitePartnersWorkflowState.OwnerOfTheLand)]
    [InlineData(SitePartnersWorkflowState.OwnerOfTheLand, SitePartnersWorkflowState.DevelopingPartner)]
    [InlineData(SitePartnersWorkflowState.DevelopingPartnerConfirm, SitePartnersWorkflowState.DevelopingPartner)]
    [InlineData(SitePartnersWorkflowState.DevelopingPartner, SitePartnersWorkflowState.FlowStarted)]
    public async Task ShouldReturnNextState_WhenBackTriggerIsExecuted(SitePartnersWorkflowState currentState, SitePartnersWorkflowState expectedState)
    {
        // given
        var testCandidate = new SitePartnersWorkflowBuilder(currentState).WithIsConsortiumMember().Build();

        // when
        var result = await testCandidate.NextState(Trigger.Back);

        // then
        result.Should().Be(expectedState);
    }
}
