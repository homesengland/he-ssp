using FluentAssertions;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.AHP.Consortium.Contract;

namespace HE.Investment.AHP.WWW.Tests.Workflows.ConsortiumWorkflowTests;

public class StateCanBeAccessedTests
{
    [Theory]
    [InlineData(ConsortiumWorkflowState.Index, true)]
    [InlineData(ConsortiumWorkflowState.Start, true)]
    public async Task ShouldReturnValue_WhenMethodCalledForDefaults(ConsortiumWorkflowState state, bool expectedResult)
    {
        // given
        var workflow = new ConsortiumWorkflow(ConsortiumWorkflowState.Index);

        // when
        var result = await workflow.StateCanBeAccessed(state);

        // then
        result.Should().Be(expectedResult);
    }
}
