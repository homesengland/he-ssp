using FluentAssertions;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Tests.TestObjectBuilders;
using Xunit;

namespace HE.Investments.AHP.Allocation.Domain.Tests.Claims.Entities.PhaseEntityTests;

public class GetMilestoneClaimTests
{
    [Theory]
    [InlineData(MilestoneType.Acquisition)]
    [InlineData(MilestoneType.StartOnSite)]
    public void ShouldReturnCorrectMilestone_WhenMilestoneTypeWasProvidedAndIsOnlyCompletionMilestoneIsFalse(MilestoneType milestoneType)
    {
        // given
        var milestone = MilestoneClaimTestBuilder.Draft().WithType(milestoneType).Build();
        var testCandidate = PhaseEntityTestBuilder.New()
            .WithAcquisitionMilestone(milestoneType == MilestoneType.Acquisition ? milestone : null)
            .WithStartOnSiteMilestone(milestoneType == MilestoneType.StartOnSite ? milestone : null)
            .Build();

        // when
        var result = testCandidate.GetMilestoneClaim(milestoneType);

        // then
        result.Should().Be(milestone);
    }

    [Theory]
    [InlineData(MilestoneType.Acquisition)]
    [InlineData(MilestoneType.StartOnSite)]
    public void ShouldReturnNull_WhenMilestoneTypeWasProvidedAndIsOnlyCompletionMilestoneIsTrue(MilestoneType milestoneType)
    {
        // given
        var completionMilestone = MilestoneClaimTestBuilder.Draft().WithType(MilestoneType.Completion).Build();
        var testCandidate = PhaseEntityTestBuilder.New(true)
            .WithCompletionMilestone(completionMilestone)
            .Build();

        // when
        var result = testCandidate.GetMilestoneClaim(milestoneType);

        // then
        result.Should().BeNull();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShouldReturnCompletionMilestone_WhenMilestoneTypeWasProvidedRegardlessTheIsOnlyCompletionValue(bool isOnlyCompletionMilestone)
    {
        // given
        var completionMilestone = MilestoneClaimTestBuilder.Draft().WithType(MilestoneType.Completion).Build();
        var testCandidate = PhaseEntityTestBuilder.New(isOnlyCompletionMilestone)
            .WithCompletionMilestone(completionMilestone)
            .Build();

        // when
        var result = testCandidate.GetMilestoneClaim(MilestoneType.Completion);

        // then
        result.Should().Be(completionMilestone);
    }
}
