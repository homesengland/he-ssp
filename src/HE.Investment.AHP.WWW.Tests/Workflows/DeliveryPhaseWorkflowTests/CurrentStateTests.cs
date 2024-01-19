using FluentAssertions;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.WWW.Tests.Workflows.DeliveryPhaseWorkflowTests;

public class CurrentStateTests
{
    [Theory]
    [InlineData(DeliveryPhaseWorkflowState.Create)]
    [InlineData(DeliveryPhaseWorkflowState.Name)]
    [InlineData(DeliveryPhaseWorkflowState.TypeOfHomes)]
    [InlineData(DeliveryPhaseWorkflowState.BuildActivityType)]
    [InlineData(DeliveryPhaseWorkflowState.AddHomes)]
    [InlineData(DeliveryPhaseWorkflowState.SummaryOfDelivery)]
    [InlineData(DeliveryPhaseWorkflowState.AcquisitionMilestone)]
    [InlineData(DeliveryPhaseWorkflowState.StartOnSiteMilestone)]
    [InlineData(DeliveryPhaseWorkflowState.PracticalCompletionMilestone)]
    [InlineData(DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp)]
    public void ShouldReturnCheckAnswers_WhenIsReadonly(DeliveryPhaseWorkflowState current)
    {
        // given
        var workflow = BuildWorkflow(current, DeliveryPhaseDetailsTestData.WithNames, true);

        // when
        var result = workflow.CurrentState(current);

        // then
        result.Should().Be(DeliveryPhaseWorkflowState.CheckAnswers);
    }

    [Theory]
    [InlineData(DeliveryPhaseWorkflowState.Create)]
    [InlineData(DeliveryPhaseWorkflowState.Name)]
    [InlineData(DeliveryPhaseWorkflowState.TypeOfHomes)]
    [InlineData(DeliveryPhaseWorkflowState.BuildActivityType)]
    [InlineData(DeliveryPhaseWorkflowState.AddHomes)]
    [InlineData(DeliveryPhaseWorkflowState.SummaryOfDelivery)]
    [InlineData(DeliveryPhaseWorkflowState.AcquisitionMilestone)]
    [InlineData(DeliveryPhaseWorkflowState.StartOnSiteMilestone)]
    [InlineData(DeliveryPhaseWorkflowState.PracticalCompletionMilestone)]
    [InlineData(DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp)]
    public void ShouldReturnCurrentState_WhenStatusNotStarted(DeliveryPhaseWorkflowState current)
    {
        // given
        var workflow = BuildWorkflow(current, DeliveryPhaseDetailsTestData.WithNames with { Status = SectionStatus.NotStarted });

        // when
        var result = workflow.CurrentState(current);

        // then
        result.Should().Be(current);
    }

    [Theory]
    [InlineData(DeliveryPhaseWorkflowState.Name)]
    [InlineData(DeliveryPhaseWorkflowState.TypeOfHomes)]
    [InlineData(DeliveryPhaseWorkflowState.BuildActivityType)]
    [InlineData(DeliveryPhaseWorkflowState.AddHomes)]
    [InlineData(DeliveryPhaseWorkflowState.SummaryOfDelivery)]
    [InlineData(DeliveryPhaseWorkflowState.AcquisitionMilestone)]
    [InlineData(DeliveryPhaseWorkflowState.StartOnSiteMilestone)]
    [InlineData(DeliveryPhaseWorkflowState.PracticalCompletionMilestone)]
    [InlineData(DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp)]
    public void ShouldReturnCurrentState_WhenStateIsNotCreate(DeliveryPhaseWorkflowState current)
    {
        // given
        var workflow = BuildWorkflow(current, DeliveryPhaseDetailsTestData.WithNames);

        // when
        var result = workflow.CurrentState(current);

        // then
        result.Should().Be(current);
    }

    [Fact]
    public void ShouldReturnTypeOfHomes_WhenTypeOfHomesNotProvided()
    {
        // given
        var state = DeliveryPhaseWorkflowState.Create;
        var workflow = BuildWorkflow(state, DeliveryPhaseDetailsTestData.WithNames with { TypeOfHomes = null });

        // when
        var result = workflow.CurrentState(state);

        // then
        result.Should().Be(DeliveryPhaseWorkflowState.TypeOfHomes);
    }

    [Fact]
    public void ShouldReturnBuildActivityType_WhenBuildActivityTypeNotProvided()
    {
        // given
        var state = DeliveryPhaseWorkflowState.Create;
        var workflow = BuildWorkflow(state, DeliveryPhaseDetailsTestData.WithNames with { TypeOfHomes = TypeOfHomes.NewBuild, BuildActivityType = null });

        // when
        var result = workflow.CurrentState(state);

        // then
        result.Should().Be(DeliveryPhaseWorkflowState.BuildActivityType);
    }

    [Fact]
    public void ShouldReturnReconfiguringExistingNeeded_WhenReconfiguringExistingNeededNotProvided()
    {
        // given
        var state = DeliveryPhaseWorkflowState.Create;
        var workflow = BuildWorkflow(
            state,
            DeliveryPhaseDetailsTestData.WithNames with
            {
                TypeOfHomes = TypeOfHomes.NewBuild,
                BuildActivityType = BuildActivityType.Regeneration,
                IsReconfiguringExistingNeeded = true,
            });

        // when
        var result = workflow.CurrentState(state);

        // then
        result.Should().Be(DeliveryPhaseWorkflowState.ReconfiguringExisting);
    }

    [Fact]
    public void ShouldReturnAddHomes_WhenHomesNotProvided()
    {
        // given
        var state = DeliveryPhaseWorkflowState.Create;
        var workflow = BuildWorkflow(
            state,
            DeliveryPhaseDetailsTestData.WithNames with
            {
                TypeOfHomes = TypeOfHomes.NewBuild,
                BuildActivityType = BuildActivityType.Regeneration,
                NumberOfHomes = null,
            });

        // when
        var result = workflow.CurrentState(state);

        // then
        result.Should().Be(DeliveryPhaseWorkflowState.AddHomes);
    }

    [Fact]
    public void ShouldReturnAcquisitionMilestone_WhenAcquisitionPaymentDateNotProvided()
    {
        // given
        var state = DeliveryPhaseWorkflowState.Create;
        var workflow = BuildWorkflow(
            state,
            DeliveryPhaseDetailsTestData.WithNames with
            {
                TypeOfHomes = TypeOfHomes.NewBuild,
                BuildActivityType = BuildActivityType.Regeneration,
                NumberOfHomes = 2,
                AcquisitionDate = new DateDetails("1", "2", "2023"),
            });

        // when
        var result = workflow.CurrentState(state);

        // then
        result.Should().Be(DeliveryPhaseWorkflowState.AcquisitionMilestone);
    }

    [Fact]
    public void ShouldReturnAcquisitionMilestone_WhenAcquisitionDateNotProvided()
    {
        // given
        var state = DeliveryPhaseWorkflowState.Create;
        var workflow = BuildWorkflow(
            state,
            DeliveryPhaseDetailsTestData.WithNames with
            {
                TypeOfHomes = TypeOfHomes.NewBuild,
                BuildActivityType = BuildActivityType.Regeneration,
                NumberOfHomes = 2,
                AcquisitionPaymentDate = new DateDetails("1", "2", "2023"),
            });

        // when
        var result = workflow.CurrentState(state);

        // then
        result.Should().Be(DeliveryPhaseWorkflowState.AcquisitionMilestone);
    }

    [Fact]
    public void ShouldReturnPracticalCompletionMilestone_WhenAcquisitionNotAvailable()
    {
        // given
        var state = DeliveryPhaseWorkflowState.Create;
        var workflow = BuildWorkflow(
            state,
            DeliveryPhaseDetailsTestData.WithNames with
            {
                TypeOfHomes = TypeOfHomes.NewBuild,
                BuildActivityType = BuildActivityType.Regeneration,
                NumberOfHomes = 2,
                IsOnlyCompletionMilestone = true,
            });

        // when
        var result = workflow.CurrentState(state);

        // then
        result.Should().Be(DeliveryPhaseWorkflowState.PracticalCompletionMilestone);
    }

    [Fact]
    public void ShouldReturnUnregisteredBodyFollowUp_WhenUnregisteredBodyFollowUpNotProvided()
    {
        // given
        var state = DeliveryPhaseWorkflowState.Create;
        var workflow = BuildWorkflow(
            state,
            DeliveryPhaseDetailsTestData.WithNames with
            {
                TypeOfHomes = TypeOfHomes.NewBuild,
                BuildActivityType = BuildActivityType.Regeneration,
                NumberOfHomes = 2,
                IsOnlyCompletionMilestone = true,
                PracticalCompletionDate = new DateDetails("1", "2", "2023"),
                PracticalCompletionPaymentDate = new DateDetails("1", "2", "2023"),
                IsUnregisteredBody = true,
            });

        // when
        var result = workflow.CurrentState(state);

        // then
        result.Should().Be(DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp);
    }

    private static DeliveryPhaseWorkflow BuildWorkflow(
        DeliveryPhaseWorkflowState currentSiteWorkflowState,
        DeliveryPhaseDetails deliveryPhaseDetails,
        bool? isReadonly = false)
    {
        return new DeliveryPhaseWorkflow(currentSiteWorkflowState, deliveryPhaseDetails, isReadonly!.Value);
    }
}
