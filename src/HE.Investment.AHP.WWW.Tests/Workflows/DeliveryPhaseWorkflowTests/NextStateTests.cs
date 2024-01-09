using FluentAssertions;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Common.WWW.Routing;

namespace HE.Investment.AHP.WWW.Tests.Workflows.DeliveryPhaseWorkflowTests;

public class NextStateTests
{
    [Theory]
    [InlineData(DeliveryPhaseWorkflowState.Summary, DeliveryPhaseWorkflowState.AcquisitionMilestone)]
    [InlineData(DeliveryPhaseWorkflowState.AcquisitionMilestone, DeliveryPhaseWorkflowState.StartOnSiteMilestone)]
    [InlineData(DeliveryPhaseWorkflowState.StartOnSiteMilestone, DeliveryPhaseWorkflowState.PracticalCompletionMilestone)]
    [InlineData(DeliveryPhaseWorkflowState.PracticalCompletionMilestone, DeliveryPhaseWorkflowState.CheckAnswers)]
    [InlineData(DeliveryPhaseWorkflowState.Create, DeliveryPhaseWorkflowState.Details)]
    [InlineData(DeliveryPhaseWorkflowState.Name, DeliveryPhaseWorkflowState.Details)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecutedAsUnregisteredBody(DeliveryPhaseWorkflowState current, DeliveryPhaseWorkflowState expectedNext)
    {
        // given
        var workflow = BuildWorkflow(current, false);

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(DeliveryPhaseWorkflowState.Summary, DeliveryPhaseWorkflowState.PracticalCompletionMilestone)]
    [InlineData(DeliveryPhaseWorkflowState.PracticalCompletionMilestone, DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp)]
    [InlineData(DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp, DeliveryPhaseWorkflowState.CheckAnswers)]
    [InlineData(DeliveryPhaseWorkflowState.Create, DeliveryPhaseWorkflowState.Details)]
    [InlineData(DeliveryPhaseWorkflowState.Name, DeliveryPhaseWorkflowState.Details)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecutedAsRegisteredBody(DeliveryPhaseWorkflowState current, DeliveryPhaseWorkflowState expectedNext)
    {
        // given
        var workflow = BuildWorkflow(current, true);

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(DeliveryPhaseWorkflowState.CheckAnswers, DeliveryPhaseWorkflowState.PracticalCompletionMilestone)]
    [InlineData(DeliveryPhaseWorkflowState.PracticalCompletionMilestone, DeliveryPhaseWorkflowState.StartOnSiteMilestone)]
    [InlineData(DeliveryPhaseWorkflowState.StartOnSiteMilestone, DeliveryPhaseWorkflowState.AcquisitionMilestone)]
    [InlineData(DeliveryPhaseWorkflowState.AcquisitionMilestone, DeliveryPhaseWorkflowState.Summary)]
    public async Task ShouldReturnNextState_WhenBackTriggerExecutedAsUnregisteredBody(DeliveryPhaseWorkflowState current, DeliveryPhaseWorkflowState expectedNext)
    {
        // given
        var workflow = BuildWorkflow(current, false);

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(DeliveryPhaseWorkflowState.CheckAnswers, DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp)]
    [InlineData(DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp, DeliveryPhaseWorkflowState.PracticalCompletionMilestone)]
    [InlineData(DeliveryPhaseWorkflowState.PracticalCompletionMilestone, DeliveryPhaseWorkflowState.Summary)]
    public async Task ShouldReturnNextState_WhenBackTriggerExecutedAsRegisteredBody(DeliveryPhaseWorkflowState current, DeliveryPhaseWorkflowState expectedNext)
    {
        // given
        var workflow = BuildWorkflow(current, true);

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(expectedNext);
    }

    private static DeliveryPhaseWorkflow BuildWorkflow(DeliveryPhaseWorkflowState currentSiteWorkflowState, bool isUnregisteredBody)
    {
        return new DeliveryPhaseWorkflow(currentSiteWorkflowState, isUnregisteredBody);
    }
}
