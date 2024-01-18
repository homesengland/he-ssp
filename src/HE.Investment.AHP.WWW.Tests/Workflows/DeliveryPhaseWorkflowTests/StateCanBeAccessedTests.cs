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
    [InlineData(DeliveryPhaseWorkflowState.Create)]
    [InlineData(DeliveryPhaseWorkflowState.Name)]
    [InlineData(DeliveryPhaseWorkflowState.TypeOfHomes)]
    [InlineData(DeliveryPhaseWorkflowState.BuildActivityType)]
    [InlineData(DeliveryPhaseWorkflowState.SummaryOfDelivery)]
    [InlineData(DeliveryPhaseWorkflowState.PracticalCompletionMilestone)]
    [InlineData(DeliveryPhaseWorkflowState.CheckAnswers)]
    public void ShouldReturnTrue_WhenTryToAccessPageAsUnregisteredBody(DeliveryPhaseWorkflowState nextState)
    {
        // given
        var workflow = BuildWorkflow(DeliveryPhaseWorkflowState.Create, true);

        // when
        var result = workflow.CanBeAccessed(nextState);

        // then
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(DeliveryPhaseWorkflowState.Create)]
    [InlineData(DeliveryPhaseWorkflowState.Name)]
    [InlineData(DeliveryPhaseWorkflowState.TypeOfHomes)]
    [InlineData(DeliveryPhaseWorkflowState.BuildActivityType)]
    [InlineData(DeliveryPhaseWorkflowState.SummaryOfDelivery)]
    [InlineData(DeliveryPhaseWorkflowState.PracticalCompletionMilestone)]
    [InlineData(DeliveryPhaseWorkflowState.CheckAnswers)]
    public void ShouldReturnTrue_WhenTryToAccessPageAsRegisteredBody(DeliveryPhaseWorkflowState nextState)
    {
        // given
        var workflow = BuildWorkflow(DeliveryPhaseWorkflowState.Create, false);

        // when
        var result = workflow.CanBeAccessed(nextState);

        // then
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(DeliveryPhaseWorkflowState.AcquisitionMilestone)]
    [InlineData(DeliveryPhaseWorkflowState.StartOnSiteMilestone)]
    public void ShouldReturnFalse_WhenTryToAccessPageNotAvailableForIsOnlyCompletionMilestone(DeliveryPhaseWorkflowState nextState)
    {
        // given
        var workflow = BuildWorkflow(DeliveryPhaseWorkflowState.Create, false, true);

        // when
        var result = workflow.CanBeAccessed(nextState);

        // then
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(DeliveryPhaseWorkflowState.Create)]
    [InlineData(DeliveryPhaseWorkflowState.Name)]
    [InlineData(DeliveryPhaseWorkflowState.TypeOfHomes)]
    [InlineData(DeliveryPhaseWorkflowState.BuildActivityType)]
    [InlineData(DeliveryPhaseWorkflowState.SummaryOfDelivery)]
    [InlineData(DeliveryPhaseWorkflowState.PracticalCompletionMilestone)]
    [InlineData(DeliveryPhaseWorkflowState.CheckAnswers)]
    public void ShouldReturnTrue_WhenTryToAccessPageForIsOnlyCompletionMilestone(DeliveryPhaseWorkflowState nextState)
    {
        // given
        var workflow = BuildWorkflow(DeliveryPhaseWorkflowState.Create, false, true);

        // when
        var result = workflow.CanBeAccessed(nextState);

        // then
        result.Should().BeTrue();
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

    private static DeliveryPhaseWorkflow BuildWorkflow(
        DeliveryPhaseWorkflowState currentSiteWorkflowState,
        bool isUnregisteredBody,
        bool isOnlyCompletionMilestone = false)
    {
        return new DeliveryPhaseWorkflow(
            currentSiteWorkflowState,
            DeliveryPhaseDetailsTestData.WithNames with { IsUnregisteredBody = isUnregisteredBody, IsOnlyCompletionMilestone = isOnlyCompletionMilestone });
    }
}
