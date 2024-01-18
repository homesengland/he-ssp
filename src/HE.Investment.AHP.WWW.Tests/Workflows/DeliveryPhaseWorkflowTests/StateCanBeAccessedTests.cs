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
        var workflow = BuildWorkflow(DeliveryPhaseWorkflowState.Create, isUnregisteredBody: true);
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
        var workflow = BuildWorkflow(DeliveryPhaseWorkflowState.Create, isUnregisteredBody: true);

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
        var workflow = BuildWorkflow(DeliveryPhaseWorkflowState.Create, isUnregisteredBody: false);

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
        var workflow = BuildWorkflow(DeliveryPhaseWorkflowState.Create, isUnregisteredBody: true);

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
        var workflow = BuildWorkflow(DeliveryPhaseWorkflowState.Create, isUnregisteredBody: false);

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
        var workflow = BuildWorkflow(DeliveryPhaseWorkflowState.Create, isUnregisteredBody: false, isOnlyCompletionMilestone: true);

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
        var workflow = BuildWorkflow(DeliveryPhaseWorkflowState.Create, isUnregisteredBody: false, isOnlyCompletionMilestone: true);

        // when
        var result = workflow.CanBeAccessed(nextState);

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnFalseForPageReconfigureExisting_WhenTryReconfigureExistingIsNotNeeded()
    {
        // given
        var workflow = BuildWorkflow(DeliveryPhaseWorkflowState.BuildActivityType, isUnregisteredBody: true, isReconfiguringExistingNeeded: false);

        // when
        var result = workflow.CanBeAccessed(DeliveryPhaseWorkflowState.ReconfiguringExisting);

        // then
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(DeliveryPhaseWorkflowState.Name)]
    [InlineData(DeliveryPhaseWorkflowState.TypeOfHomes)]
    [InlineData(DeliveryPhaseWorkflowState.BuildActivityType)]
    [InlineData(DeliveryPhaseWorkflowState.ReconfiguringExisting)]
    [InlineData(DeliveryPhaseWorkflowState.AddHomes)]
    [InlineData(DeliveryPhaseWorkflowState.CheckAnswers)]
    public void ShouldReturnTrue_WhenNumberOfHomesIsZero(DeliveryPhaseWorkflowState nextState)
    {
        // given
        var workflow = BuildWorkflow(DeliveryPhaseWorkflowState.Create, isUnregisteredBody: true, numberOfHomes: 0, isReconfiguringExistingNeeded: true);

        // when
        var result = workflow.CanBeAccessed(nextState);

        // then
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(DeliveryPhaseWorkflowState.SummaryOfDelivery, true)]
    [InlineData(DeliveryPhaseWorkflowState.AcquisitionMilestone, false)]
    [InlineData(DeliveryPhaseWorkflowState.StartOnSiteMilestone, false)]
    [InlineData(DeliveryPhaseWorkflowState.PracticalCompletionMilestone, true)]
    [InlineData(DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp, true)]
    public void ShouldReturnFalse_WhenNumberOfHomesIsZero(DeliveryPhaseWorkflowState nextState, bool isUnregisteredBody)
    {
        // given
        var workflow = BuildWorkflow(DeliveryPhaseWorkflowState.Create, numberOfHomes: 0, isUnregisteredBody);

        // when
        var result = workflow.CanBeAccessed(nextState);

        // then
        result.Should().BeFalse();
    }

    private static DeliveryPhaseWorkflow BuildWorkflow(
        DeliveryPhaseWorkflowState currentSiteWorkflowState,
        int numberOfHomes = 1,
        bool isUnregisteredBody = false,
        bool isOnlyCompletionMilestone = false,
        bool isReconfiguringExistingNeeded = false)
    {
        return new DeliveryPhaseWorkflow(
            currentSiteWorkflowState,
            DeliveryPhaseDetailsTestData.WithNames with
            {
                NumberOfHomes = numberOfHomes,
                IsUnregisteredBody = isUnregisteredBody,
                IsOnlyCompletionMilestone = isOnlyCompletionMilestone,
                IsReconfiguringExistingNeeded = isReconfiguringExistingNeeded,
            });
    }
}
