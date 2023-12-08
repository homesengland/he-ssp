using FluentAssertions;
using HE.Investments.Common.Workflow;
using Xunit;

namespace HE.Investments.Common.Tests.Workflow.EncodedWorkflowTests;

public class ValueTests
{
    [Fact]
    public void ShouldEncodeWorkflow_WhenAllStatesCanBeAccessed()
    {
        // given
        var workflow = new EncodedWorkflow<TestWorkflowEnum>(_ => true);

        // when
        var result = workflow.Value;

        // then
        result.Should().Be("abcde");
    }

    [Fact]
    public void ShouldEncodeWorkflow_WhenNoStatesCanBeAccessed()
    {
        // given
        var workflow = new EncodedWorkflow<TestWorkflowEnum>(_ => false);

        // when
        var result = workflow.Value;

        // then
        result.Should().Be(string.Empty);
    }

    [Fact]
    public void ShouldEncodeWorkflow_WhenSomeStatesCanBeAccessed()
    {
        // given
        var workflow = new EncodedWorkflow<TestWorkflowEnum>(x => x is TestWorkflowEnum.State2 or TestWorkflowEnum.State3);

        // when
        var result = workflow.Value;

        // then
        result.Should().Be("bc");
    }

    [Fact]
    public void ShouldBuildProperWorkflow_WhenItIsAlreadyEncoded()
    {
        // given
        var workflow = new EncodedWorkflow<TestWorkflowEnum>("abcde");

        // when
        var getValue = () => workflow.Value;

        // then
        getValue.Should().NotThrow();
    }

    [Fact]
    public void ShouldThrowException_WhenEncodedWorkflowCannotBeMappedToState()
    {
        // given
        const string encodedWorkflow = "ef";

        // when
        var create = () => new EncodedWorkflow<TestWorkflowEnum>(encodedWorkflow);

        // then
        create.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ShouldThrowException_WhenEncodedWorkflowHasInvalidCharacters()
    {
        // given
        const string encodedWorkflow = "ab#";

        // when
        var create = () => new EncodedWorkflow<TestWorkflowEnum>(encodedWorkflow);

        // then
        create.Should().Throw<InvalidOperationException>();
    }
}
