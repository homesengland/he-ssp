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

    [Fact]
    public void ShouldReturnFalseForPageReconfigureExisting_WhenTryReconfigureExistingIsNotNeeded()
    {
        // given
        var workflow = BuildWorkflow(DeliveryPhaseWorkflowState.BuildActivityType, true, isReconfiguringExistingNeeded: false);

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
        var workflow = BuildWorkflow(DeliveryPhaseWorkflowState.Create, isUnregisteredBody: false);

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
        var workflow = BuildWorkflow(DeliveryPhaseWorkflowState.Create, isUnregisteredBody, numberOfHomes: 0);

        // when
        var result = workflow.CanBeAccessed(nextState);

        // then
        result.Should().BeFalse();
    }

    private static DeliveryPhaseWorkflow BuildWorkflow(
        DeliveryPhaseWorkflowState currentSiteWorkflowState,
        bool isUnregisteredBody = false,
        int numberOfHomes = 1,
        bool isReconfiguringExistingNeeded = false)
    {
        return new DeliveryPhaseWorkflow(
            currentSiteWorkflowState,
            DeliveryPhaseDetailsTestData.WithNames with
            {
                IsUnregisteredBody = isUnregisteredBody,
                NumberOfHomes = numberOfHomes,
                IsReconfiguringExistingNeeded = isReconfiguringExistingNeeded,
            });
    }
}
