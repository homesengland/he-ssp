using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.WWW.Workflows;

namespace HE.Investment.AHP.WWW.Tests.Workflows.DeliveryPhaseWorkflowTests;

public class StateCanBeAccessedTests
{
    [Fact]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Error in the Sonarlint library when using AngleSharp when using AngleSharp")]
    public void ShouldNotThrowException_ForEachPossibleWorkflowState()
    {
        // given
        var workflow = BuildWorkflow(DeliveryPhaseWorkflowState.Create, true);
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
        var workflow = BuildWorkflow(DeliveryPhaseWorkflowState.Create, true);

        // when
        var result = workflow.CanBeAccessed(nextState);

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnFalseForPageReconfigureExisting_WhenTryReconfigureExistingIsNotNeeded()
    {
        // given
        var workflow = BuildWorkflow(DeliveryPhaseWorkflowState.BuildActivityType, true);

        // when
        var result = workflow.CanBeAccessed(DeliveryPhaseWorkflowState.ReconfiguringExisting);

        // then
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp)]
    public void ShouldReturnFalse_WhenTryToAccessPageAsRegisteredBody(DeliveryPhaseWorkflowState nextState)
    {
        // given
        var workflow = BuildWorkflow(DeliveryPhaseWorkflowState.Create, false);

        // when
        var result = workflow.CanBeAccessed(nextState);

        // then
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(DeliveryPhaseWorkflowState.Create, true)]
    [InlineData(DeliveryPhaseWorkflowState.Name, true)]
    [InlineData(DeliveryPhaseWorkflowState.TypeOfHomes, true)]
    [InlineData(DeliveryPhaseWorkflowState.BuildActivityType, true)]
    [InlineData(DeliveryPhaseWorkflowState.SummaryOfDelivery, true)]
    [InlineData(DeliveryPhaseWorkflowState.PracticalCompletionMilestone, true)]
    [InlineData(DeliveryPhaseWorkflowState.CheckAnswers, true)]
    [InlineData(DeliveryPhaseWorkflowState.Create, false)]
    [InlineData(DeliveryPhaseWorkflowState.Name, false)]
    [InlineData(DeliveryPhaseWorkflowState.SummaryOfDelivery, false)]
    [InlineData(DeliveryPhaseWorkflowState.PracticalCompletionMilestone, false)]
    [InlineData(DeliveryPhaseWorkflowState.CheckAnswers, false)]
    public void ShouldReturnTrue_WhenTryToAccessPage(DeliveryPhaseWorkflowState nextState, bool isUnregisteredBody)
    {
        // given
        var workflow = BuildWorkflow(DeliveryPhaseWorkflowState.Create, isUnregisteredBody);

        // when
        var result = workflow.CanBeAccessed(nextState);

        // then
        result.Should().BeTrue();
    }

    private static DeliveryPhaseWorkflow BuildWorkflow(DeliveryPhaseWorkflowState currentSiteWorkflowState, bool isUnregisteredBody)
    {
        return new DeliveryPhaseWorkflow(currentSiteWorkflowState, DeliveryPhaseDetailsTestData.WithNames with { IsUnregisteredBody = isUnregisteredBody });
    }
}
