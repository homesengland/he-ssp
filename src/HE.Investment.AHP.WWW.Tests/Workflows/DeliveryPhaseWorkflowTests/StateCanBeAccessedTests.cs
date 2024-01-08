using FluentAssertions;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.WWW.Workflows;

namespace HE.Investment.AHP.WWW.Tests.Workflows.DeliveryPhaseWorkflowTests;

public class StateCanBeAccessedTests
{
    [Fact]
    public void ShouldNotThrowException_ForEachPossibleWorkflowState()
    {
        // given
        var workflow = BuildWorkflow(DeliveryPhaseWorkflowState.New, true);
        var workflowStates = Enum.GetValues<DeliveryPhaseWorkflowState>();

        // when
        foreach (var workflowState in workflowStates)
        {
            workflow.CanBeAccessed(workflowState);
        }

        // then - no Exception should be thrown
    }

    [Theory]
    [InlineData(DeliveryPhaseWorkflowState.AcquisitionMilestone)]
    [InlineData(DeliveryPhaseWorkflowState.StartOnSiteMilestone)]
    public void ShouldReturnFalse_WhenTryToAccessPageAsUnregisteredBody(DeliveryPhaseWorkflowState nextState)
    {
        // given
        var workflow = BuildWorkflow(DeliveryPhaseWorkflowState.New, true);

        // when
        var result = workflow.CanBeAccessed(nextState);

        // then
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp)]
    public void ShouldReturnFalse_WhenTryToAccessPageAsRegisteredBody(DeliveryPhaseWorkflowState nextState)
    {
        // given
        var workflow = BuildWorkflow(DeliveryPhaseWorkflowState.New, false);

        // when
        var result = workflow.CanBeAccessed(nextState);

        // then
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(DeliveryPhaseWorkflowState.New, true)]
    [InlineData(DeliveryPhaseWorkflowState.Name, true)]
    [InlineData(DeliveryPhaseWorkflowState.Summary, true)]
    [InlineData(DeliveryPhaseWorkflowState.PracticalCompletionMilestone, true)]
    [InlineData(DeliveryPhaseWorkflowState.CheckAnswers, true)]
    [InlineData(DeliveryPhaseWorkflowState.New, false)]
    [InlineData(DeliveryPhaseWorkflowState.Name, false)]
    [InlineData(DeliveryPhaseWorkflowState.Summary, false)]
    [InlineData(DeliveryPhaseWorkflowState.PracticalCompletionMilestone, false)]
    [InlineData(DeliveryPhaseWorkflowState.CheckAnswers, false)]
    public void ShouldReturnTrue_WhenTryToAccessPage(DeliveryPhaseWorkflowState nextState, bool isUnregisteredBody)
    {
        // given
        var workflow = BuildWorkflow(DeliveryPhaseWorkflowState.New, isUnregisteredBody);

        // when
        var result = workflow.CanBeAccessed(nextState);

        // then
        result.Should().BeTrue();
    }

    private static DeliveryPhaseWorkflow BuildWorkflow(DeliveryPhaseWorkflowState currentSiteWorkflowState, bool isUnregisteredBody)
    {
        return new DeliveryPhaseWorkflow(currentSiteWorkflowState, isUnregisteredBody);
    }
}
