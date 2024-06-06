using FluentAssertions;
using HE.Investment.AHP.Contract.SitePartners;
using HE.Investments.Common.WWW.Routing;

namespace HE.Investment.AHP.WWW.Tests.Workflows.SitePartnersWorkflowTests.NextStateTests;

public class StandardOrganisationNextStateTests
{
    [Fact]
    public async Task ShouldReturnNextState_WhenContinueTriggerIsExecuted()
    {
        // given
        var testCandidate = new SitePartnersWorkflowBuilder().WithIsUnregisteredBody(false).WithIsConsortiumMember(false).Build();

        // when
        var result = await testCandidate.NextState(Trigger.Continue);

        // then
        result.Should().Be(SitePartnersWorkflowState.FlowFinished);
    }

    [Fact]
    public async Task ShouldReturnNextState_WhenBackTriggerIsExecuted()
    {
        // given
        var testCandidate = new SitePartnersWorkflowBuilder(SitePartnersWorkflowState.FlowFinished).WithIsUnregisteredBody(false).WithIsConsortiumMember(false).Build();

        // when
        var result = await testCandidate.NextState(Trigger.Back);

        // then
        result.Should().Be(SitePartnersWorkflowState.FlowStarted);
    }
}
