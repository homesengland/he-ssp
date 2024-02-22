using FluentAssertions;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.WWW.Workflows;

namespace HE.Investment.AHP.WWW.Tests.Workflows.DeliveryPhaseWorkflowTests;

public class StateCanBeAccessedTests
{
    [Fact]
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
    public void ShouldReturnFalse_WhenTryToAccessPageAndIsOnlyCompletionMilestone(DeliveryPhaseWorkflowState nextState)
    {
        // given
        var workflow = BuildWorkflow(DeliveryPhaseWorkflowState.Create, isOnlyCompletionMilestone: true);

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
    [InlineData(DeliveryPhaseWorkflowState.NewBuildActivityType)]
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
    [InlineData(DeliveryPhaseWorkflowState.NewBuildActivityType)]
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
    [InlineData(DeliveryPhaseWorkflowState.NewBuildActivityType)]
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

    [Theory]
    [InlineData(DeliveryPhaseWorkflowState.RehabBuildActivityType)]
    [InlineData(DeliveryPhaseWorkflowState.ReconfiguringExisting)]
    public void ShouldReturnFalse_WhenReconfigureExistingIsNotNeeded(DeliveryPhaseWorkflowState nextState)
    {
        // given
        var workflow = BuildWorkflow(DeliveryPhaseWorkflowState.Create, isUnregisteredBody: true, isReconfiguringExistingNeeded: false);

        // when
        var result = workflow.CanBeAccessed(nextState);

        // then
        result.Should().BeFalse();
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
        var workflow = BuildWorkflow(DeliveryPhaseWorkflowState.Create, numberOfHomes: 0, isUnregisteredBody: isUnregisteredBody);

        // when
        var result = workflow.CanBeAccessed(nextState);

        // then
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(DeliveryPhaseWorkflowState.Name)]
    [InlineData(DeliveryPhaseWorkflowState.TypeOfHomes)]
    [InlineData(DeliveryPhaseWorkflowState.RehabBuildActivityType)]
    [InlineData(DeliveryPhaseWorkflowState.ReconfiguringExisting)]
    [InlineData(DeliveryPhaseWorkflowState.AddHomes)]
    [InlineData(DeliveryPhaseWorkflowState.CheckAnswers)]
    public void ShouldReturnTrue_WhenNumberOfHomesIsZero(DeliveryPhaseWorkflowState nextState)
    {
        // given
        var workflow = BuildWorkflow(
            DeliveryPhaseWorkflowState.Create,
            isUnregisteredBody: true,
            numberOfHomes: 0,
            isReconfiguringExistingNeeded: true,
            typeOfHomes: TypeOfHomes.Rehab);

        // when
        var result = workflow.CanBeAccessed(nextState);

        // then
        result.Should().BeTrue();
    }

    private static DeliveryPhaseWorkflow BuildWorkflow(
        DeliveryPhaseWorkflowState currentSiteWorkflowState,
        int numberOfHomes = 1,
        TypeOfHomes typeOfHomes = TypeOfHomes.NewBuild,
        bool isUnregisteredBody = false,
        bool isOnlyCompletionMilestone = false,
        bool isReconfiguringExistingNeeded = false)
    {
        return new DeliveryPhaseWorkflow(
            currentSiteWorkflowState,
            DeliveryPhaseDetailsTestData.WithNames with
            {
                TypeOfHomes = typeOfHomes,
                NumberOfHomes = numberOfHomes,
                IsUnregisteredBody = isUnregisteredBody,
                IsOnlyCompletionMilestone = isOnlyCompletionMilestone,
                IsReconfiguringExistingNeeded = isReconfiguringExistingNeeded,
            },
            false);
    }
}
