using HE.Investment.AHP.Domain.HomeTypes;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.Domain.Tests.HomeTypes.HomeTypesWorkflowTests;

public class CanBeAccessedTests : TestBase<HomeTypesWorkflow>
{
    [Fact]
    public void ShouldNotThrowException_ForEachPossibleWorkflowState()
    {
        // given
        var workflowStates = Enum.GetValues<HomeTypesWorkflowState>();

        // when
        foreach (var workflowState in workflowStates)
        {
            TestCandidate.CanBeAccessed(workflowState);
        }

        // then - no Exception should be thrown
    }
}
