using FluentAssertions;
using HE.Investments.Common.Workflow;
using Xunit;

namespace HE.Investments.Common.Tests.Workflow.EncodedWorkflowTests;

public class GetNextChangedWorkflowStateTests
{
    [Theory]
    [InlineData("abcde", "abcde")]
    [InlineData("abcde", "abc")]
    [InlineData("abcde", "cde")]
    [InlineData("acde", "abcde")]
    public void ShouldReturnLastState_WhenThereAreNoNewStatesAfterState3(string lastEncodedWorkflow, string currentEncodedWorkflow)
    {
        // given
        var lastWorkflow = new EncodedWorkflow<TestWorkflow>(lastEncodedWorkflow);
        var currentWorkflow = new EncodedWorkflow<TestWorkflow>(currentEncodedWorkflow);

        // when
        var result = currentWorkflow.GetNextChangedWorkflowState(TestWorkflow.State3, lastWorkflow);

        // then
        result.Should().Be(TestWorkflow.State5);
    }

    [Theory]
    [InlineData("ade", "abc", TestWorkflow.State2)]
    [InlineData("ab", "d", TestWorkflow.State4)]
    public void ShouldReturnNextDifferentState_WhenThereAreSomeNewStatesAfterState1(
        string lastEncodedWorkflow,
        string currentEncodedWorkflow,
        TestWorkflow expectedResult)
    {
        // given
        var lastWorkflow = new EncodedWorkflow<TestWorkflow>(lastEncodedWorkflow);
        var currentWorkflow = new EncodedWorkflow<TestWorkflow>(currentEncodedWorkflow);

        // when
        var result = currentWorkflow.GetNextChangedWorkflowState(TestWorkflow.State1, lastWorkflow);

        // then
        result.Should().Be(expectedResult);
    }
}
