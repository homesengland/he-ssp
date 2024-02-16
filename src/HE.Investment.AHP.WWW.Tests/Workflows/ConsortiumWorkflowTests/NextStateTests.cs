using FluentAssertions;
using HE.Investment.AHP.Contract.Consortium;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Common.WWW.Routing;

namespace HE.Investment.AHP.WWW.Tests.Workflows.ConsortiumWorkflowTests;

public class NextStateTests
{
    [Theory]
    [InlineData(ConsortiumWorkflowState.Start, ConsortiumWorkflowState.Index)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecuted(ConsortiumWorkflowState current, ConsortiumWorkflowState expectedNext)
    {
        // given
        var workflow = new ConsortiumWorkflow(current);

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(ConsortiumWorkflowState.Start, ConsortiumWorkflowState.Index)]
    public async Task ShouldReturnNextState_WhenBackTriggerExecuted(ConsortiumWorkflowState current, ConsortiumWorkflowState expectedNext)
    {
        // given
        var workflow = new ConsortiumWorkflow(current);

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(expectedNext);
    }
}
