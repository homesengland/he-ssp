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
        var lastWorkflow = new EncodedWorkflow<TestWorkflowEnum>(lastEncodedWorkflow);
        var currentWorkflow = new EncodedWorkflow<TestWorkflowEnum>(currentEncodedWorkflow);

        // when
        var result = currentWorkflow.GetNextChangedWorkflowState(TestWorkflowEnum.State3, lastWorkflow);

        // then
        result.Should().Be(TestWorkflowEnum.State5);
    }

    [Theory]
    [InlineData("ade", "abc", TestWorkflowEnum.State2)]
    [InlineData("ab", "d", TestWorkflowEnum.State4)]
    public void ShouldReturnNextDifferentState_WhenThereAreSomeNewStatesAfterState1(
        string lastEncodedWorkflow,
        string currentEncodedWorkflow,
        TestWorkflowEnum expectedResult)
    {
        // given
        var lastWorkflow = new EncodedWorkflow<TestWorkflowEnum>(lastEncodedWorkflow);
        var currentWorkflow = new EncodedWorkflow<TestWorkflowEnum>(currentEncodedWorkflow);

        // when
        var result = currentWorkflow.GetNextChangedWorkflowState(TestWorkflowEnum.State1, lastWorkflow);

        // then
        result.Should().Be(expectedResult);
    }
}
