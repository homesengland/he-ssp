using FluentAssertions;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;

namespace HE.Investment.AHP.WWW.Tests.Workflows.AllocationClaimWorkflowTests;

public class CanBeAccessedTests
{
    [Fact]
    public void ShouldReturnTrue_WhenStateIsCheckAnswersAndIsReadOnlyModeIsTrue()
    {
        // given
        const bool isReadOnlyMode = true;
        var workflow = AllocationClaimWorkflowFactory.Create(MilestoneType.Acquisition, true);

        // when
        var result = workflow.CanBeAccessed(AllocationClaimWorkflowState.CheckAnswers, isReadOnlyMode);

        // then
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(AllocationClaimWorkflowState.CostsIncurred)]
    [InlineData(AllocationClaimWorkflowState.MilestoneDate)]
    [InlineData(AllocationClaimWorkflowState.Confirmation)]
    [InlineData(AllocationClaimWorkflowState.CheckAnswers)]
    public void ShouldReturnFalse_WhenMilestoneCanNotBeClaimed(AllocationClaimWorkflowState state)
    {
        // given
        const bool isReadOnlyMode = false;
        var workflow = AllocationClaimWorkflowFactory.Create(MilestoneType.Acquisition, false);

        // when
        var result = workflow.CanBeAccessed(state, isReadOnlyMode);

        // then
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(AllocationClaimWorkflowState.CostsIncurred)]
    [InlineData(AllocationClaimWorkflowState.MilestoneDate)]
    [InlineData(AllocationClaimWorkflowState.Confirmation)]
    [InlineData(AllocationClaimWorkflowState.CheckAnswers)]
    public void ShouldReturnTrue_WhenMilestoneIsAcquisitionAndCanBeClaimed(AllocationClaimWorkflowState state)
    {
        // given
        const bool isReadOnlyMode = false;
        var workflow = AllocationClaimWorkflowFactory.Create(MilestoneType.Acquisition, true);

        // when
        var result = workflow.CanBeAccessed(state, isReadOnlyMode);

        // then
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(MilestoneType.StartOnSite)]
    [InlineData(MilestoneType.Completion)]
    public void ShouldReturnFalse_WhenStateIsCostsIncurredAndMilestoneIs(MilestoneType milestoneType)
    {
        // given
        const bool isReadOnlyMode = false;
        var workflow = AllocationClaimWorkflowFactory.Create(milestoneType, true);

        // when
        var result = workflow.CanBeAccessed(AllocationClaimWorkflowState.CostsIncurred, isReadOnlyMode);

        // then
        result.Should().BeFalse();
    }
}
