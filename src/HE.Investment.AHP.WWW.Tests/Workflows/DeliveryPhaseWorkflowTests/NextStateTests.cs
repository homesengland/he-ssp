using FluentAssertions;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Common.WWW.Routing;

namespace HE.Investment.AHP.WWW.Tests.Workflows.DeliveryPhaseWorkflowTests;

public class NextStateTests
{
    [Theory]
    [InlineData(DeliveryPhaseWorkflowState.Create, DeliveryPhaseWorkflowState.TypeOfHomes)]
    [InlineData(DeliveryPhaseWorkflowState.Name, DeliveryPhaseWorkflowState.TypeOfHomes)]
    [InlineData(DeliveryPhaseWorkflowState.TypeOfHomes, DeliveryPhaseWorkflowState.BuildActivityType)]
    [InlineData(DeliveryPhaseWorkflowState.BuildActivityType, DeliveryPhaseWorkflowState.AddHomes)]
    [InlineData(DeliveryPhaseWorkflowState.AddHomes, DeliveryPhaseWorkflowState.SummaryOfDelivery)]
    [InlineData(DeliveryPhaseWorkflowState.SummaryOfDelivery, DeliveryPhaseWorkflowState.AcquisitionMilestone)]
    [InlineData(DeliveryPhaseWorkflowState.AcquisitionMilestone, DeliveryPhaseWorkflowState.StartOnSiteMilestone)]
    [InlineData(DeliveryPhaseWorkflowState.StartOnSiteMilestone, DeliveryPhaseWorkflowState.PracticalCompletionMilestone)]
    [InlineData(DeliveryPhaseWorkflowState.PracticalCompletionMilestone, DeliveryPhaseWorkflowState.CheckAnswers)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecutedAsRegisteredBody(
        DeliveryPhaseWorkflowState current,
        DeliveryPhaseWorkflowState expectedNext)
    {
        // given
        var workflow = BuildWorkflow(current, DeliveryPhaseDetailsTestData.WithNames with { IsUnregisteredBody = false });

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(DeliveryPhaseWorkflowState.Create, DeliveryPhaseWorkflowState.TypeOfHomes)]
    [InlineData(DeliveryPhaseWorkflowState.Name, DeliveryPhaseWorkflowState.TypeOfHomes)]
    [InlineData(DeliveryPhaseWorkflowState.TypeOfHomes, DeliveryPhaseWorkflowState.BuildActivityType)]
    [InlineData(DeliveryPhaseWorkflowState.BuildActivityType, DeliveryPhaseWorkflowState.AddHomes)]
    [InlineData(DeliveryPhaseWorkflowState.AddHomes, DeliveryPhaseWorkflowState.SummaryOfDelivery)]
    [InlineData(DeliveryPhaseWorkflowState.SummaryOfDelivery, DeliveryPhaseWorkflowState.PracticalCompletionMilestone)]
    [InlineData(DeliveryPhaseWorkflowState.PracticalCompletionMilestone, DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp)]
    [InlineData(DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp, DeliveryPhaseWorkflowState.CheckAnswers)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecutedAsUnregisteredBody(
        DeliveryPhaseWorkflowState current,
        DeliveryPhaseWorkflowState expectedNext)
    {
        // given
        var workflow = BuildWorkflow(current, DeliveryPhaseDetailsTestData.WithNames with { IsUnregisteredBody = true });

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(DeliveryPhaseWorkflowState.CheckAnswers, DeliveryPhaseWorkflowState.PracticalCompletionMilestone)]
    [InlineData(DeliveryPhaseWorkflowState.PracticalCompletionMilestone, DeliveryPhaseWorkflowState.StartOnSiteMilestone)]
    [InlineData(DeliveryPhaseWorkflowState.StartOnSiteMilestone, DeliveryPhaseWorkflowState.AcquisitionMilestone)]
    [InlineData(DeliveryPhaseWorkflowState.AcquisitionMilestone, DeliveryPhaseWorkflowState.SummaryOfDelivery)]
    [InlineData(DeliveryPhaseWorkflowState.SummaryOfDelivery, DeliveryPhaseWorkflowState.AddHomes)]
    [InlineData(DeliveryPhaseWorkflowState.AddHomes, DeliveryPhaseWorkflowState.BuildActivityType)]
    [InlineData(DeliveryPhaseWorkflowState.BuildActivityType, DeliveryPhaseWorkflowState.TypeOfHomes)]
    [InlineData(DeliveryPhaseWorkflowState.TypeOfHomes, DeliveryPhaseWorkflowState.Name)]
    public async Task ShouldReturnNextState_WhenBackTriggerExecutedAsRegisteredBody(
        DeliveryPhaseWorkflowState current,
        DeliveryPhaseWorkflowState expectedNext)
    {
        // given
        var workflow = BuildWorkflow(current, DeliveryPhaseDetailsTestData.WithNames with { IsUnregisteredBody = false });

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(DeliveryPhaseWorkflowState.CheckAnswers, DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp)]
    [InlineData(DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp, DeliveryPhaseWorkflowState.PracticalCompletionMilestone)]
    [InlineData(DeliveryPhaseWorkflowState.PracticalCompletionMilestone, DeliveryPhaseWorkflowState.SummaryOfDelivery)]
    [InlineData(DeliveryPhaseWorkflowState.SummaryOfDelivery, DeliveryPhaseWorkflowState.AddHomes)]
    [InlineData(DeliveryPhaseWorkflowState.AddHomes, DeliveryPhaseWorkflowState.BuildActivityType)]
    [InlineData(DeliveryPhaseWorkflowState.BuildActivityType, DeliveryPhaseWorkflowState.TypeOfHomes)]
    [InlineData(DeliveryPhaseWorkflowState.TypeOfHomes, DeliveryPhaseWorkflowState.Name)]
    public async Task ShouldReturnNextState_WhenBackTriggerExecutedAsUnregisteredBody(DeliveryPhaseWorkflowState current, DeliveryPhaseWorkflowState expectedNext)
    {
        // given
        var workflow = BuildWorkflow(current, DeliveryPhaseDetailsTestData.WithNames with { IsUnregisteredBody = true });

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(DeliveryPhaseWorkflowState.BuildActivityType, DeliveryPhaseWorkflowState.ReconfiguringExisting, true)]
    [InlineData(DeliveryPhaseWorkflowState.BuildActivityType, DeliveryPhaseWorkflowState.AddHomes, false)]
    public async Task ShouldReturnNextState_WhenBackTriggerExecutedForReconfiguringExisting(
        DeliveryPhaseWorkflowState current,
        DeliveryPhaseWorkflowState expectedNext,
        bool isReconfiguringExistingNeeded)
    {
        // given
        var workflow = BuildWorkflow(
            current,
            DeliveryPhaseDetailsTestData.WithNames with { IsReconfiguringExistingNeeded = isReconfiguringExistingNeeded });

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(DeliveryPhaseWorkflowState.Create, DeliveryPhaseWorkflowState.TypeOfHomes)]
    [InlineData(DeliveryPhaseWorkflowState.Name, DeliveryPhaseWorkflowState.TypeOfHomes)]
    [InlineData(DeliveryPhaseWorkflowState.TypeOfHomes, DeliveryPhaseWorkflowState.BuildActivityType)]
    [InlineData(DeliveryPhaseWorkflowState.BuildActivityType, DeliveryPhaseWorkflowState.AddHomes)]
    [InlineData(DeliveryPhaseWorkflowState.AddHomes, DeliveryPhaseWorkflowState.SummaryOfDelivery)]
    [InlineData(DeliveryPhaseWorkflowState.SummaryOfDelivery, DeliveryPhaseWorkflowState.PracticalCompletionMilestone)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecutedForIsOnlyCompletionMilestone(
        DeliveryPhaseWorkflowState current,
        DeliveryPhaseWorkflowState expectedNext)
    {
        // given
        var workflow = BuildWorkflow(current, DeliveryPhaseDetailsTestData.WithNames with { IsOnlyCompletionMilestone = true });

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(DeliveryPhaseWorkflowState.CheckAnswers, DeliveryPhaseWorkflowState.PracticalCompletionMilestone)]
    [InlineData(DeliveryPhaseWorkflowState.PracticalCompletionMilestone, DeliveryPhaseWorkflowState.SummaryOfDelivery)]
    [InlineData(DeliveryPhaseWorkflowState.SummaryOfDelivery, DeliveryPhaseWorkflowState.AddHomes)]
    [InlineData(DeliveryPhaseWorkflowState.AddHomes, DeliveryPhaseWorkflowState.BuildActivityType)]
    [InlineData(DeliveryPhaseWorkflowState.BuildActivityType, DeliveryPhaseWorkflowState.TypeOfHomes)]
    [InlineData(DeliveryPhaseWorkflowState.TypeOfHomes, DeliveryPhaseWorkflowState.Name)]
    public async Task ShouldReturnNextState_WhenBackTriggerExecutedForIsOnlyCompletionMilestone(DeliveryPhaseWorkflowState current, DeliveryPhaseWorkflowState expectedNext)
    {
        // given
        var workflow = BuildWorkflow(current, DeliveryPhaseDetailsTestData.WithNames with { IsOnlyCompletionMilestone = true });

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(expectedNext);
    }

    private static DeliveryPhaseWorkflow BuildWorkflow(DeliveryPhaseWorkflowState currentSiteWorkflowState, DeliveryPhaseDetails deliveryPhaseDetails)
    {
        return new DeliveryPhaseWorkflow(currentSiteWorkflowState, deliveryPhaseDetails, false);
    }
}
