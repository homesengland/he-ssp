using FluentAssertions;
using HE.Investment.AHP.Contract.Consortium;
using HE.Investment.AHP.WWW.Workflows;

namespace HE.Investment.AHP.WWW.Tests.Workflows.ConsortiumWorkflowTests;

public class CurrentStateTests
{
    [Fact]
    public void ShouldReturnStart_WhenProcessStarted()
    {
        Test(ConsortiumWorkflowState.Start);
    }

    private void Test(ConsortiumWorkflowState expected)
    {
        // given
        var workflow = new ConsortiumWorkflow(ConsortiumWorkflowState.Index);

        // when
        var result = workflow.CurrentState(ConsortiumWorkflowState.Start);

        // then
        result.Should().Be(expected);
    }
}
