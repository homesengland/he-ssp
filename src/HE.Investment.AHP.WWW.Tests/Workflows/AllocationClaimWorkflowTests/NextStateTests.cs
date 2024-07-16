using FluentAssertions;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.Common.WWW.Routing;

namespace HE.Investment.AHP.WWW.Tests.Workflows.AllocationClaimWorkflowTests;

public class NextStateTests
{
    [Fact]
    public async Task ShouldReturnNextState_WhenContinueTriggerIsExecuted()
    {
        // given
        var workflow = AllocationClaimWorkflowFactory.Create(
            MilestoneType.Acquisition,
            true,
            AllocationClaimWorkflowState.CostsIncurred);

        // when
        var result = new[]
        {
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
        };

        // then
        result[0].Should().Be(AllocationClaimWorkflowState.MilestoneDate);
        result[1].Should().Be(AllocationClaimWorkflowState.Confirmation);
        result[2].Should().Be(AllocationClaimWorkflowState.CheckAnswers);
    }

    [Fact]
    public async Task ShouldReturnNextState_WhenBackTriggerIsExecuted()
    {
        // given
        var workflow = AllocationClaimWorkflowFactory.Create(MilestoneType.Acquisition, true);

        // when
        var result = new[]
        {
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
        };

        // then
        result[0].Should().Be(AllocationClaimWorkflowState.Confirmation);
        result[1].Should().Be(AllocationClaimWorkflowState.MilestoneDate);
        result[2].Should().Be(AllocationClaimWorkflowState.CostsIncurred);
    }
}
