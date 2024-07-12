using FluentAssertions;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Tests.TestObjectBuilders;
using Xunit;

namespace HE.Investments.AHP.Allocation.Domain.Tests.Claims.Entities.PhaseEntityTests;

public class CanMilestoneBeClaimedTests
{
    [Fact]
    public void ShouldAcquisitionMilestoneReturnTrue_WhenItHasNotBeenClaimed()
    {
        // given
        var acquisitionMilestone = MilestoneClaimTestBuilder.New().NotClaimed().Build();
        var testCandidate = PhaseEntityTestBuilder.New().WithAcquisitionMilestone(acquisitionMilestone).Build();

        // when
        var result = testCandidate.CanMilestoneBeClaimed(MilestoneType.Acquisition);

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldAcquisitionMilestoneReturnFalse_WhenItHasBeenClaimed()
    {
        // given
        var acquisitionMilestone = MilestoneClaimTestBuilder.New().Claimed().Build();
        var testCandidate = PhaseEntityTestBuilder.New().WithAcquisitionMilestone(acquisitionMilestone).Build();

        // when
        var result = testCandidate.CanMilestoneBeClaimed(MilestoneType.Acquisition);

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldStartOnSiteMilestoneReturnTrue_WhenAcquisitionMilestoneHasBeenClaimedAndStartOnSiteNot()
    {
        // given
        var acquisitionMilestone = MilestoneClaimTestBuilder.New().Claimed().Build();
        var startOnSiteMilestone = MilestoneClaimTestBuilder.New().NotClaimed().Build();
        var testCandidate = PhaseEntityTestBuilder.New()
            .WithAcquisitionMilestone(acquisitionMilestone)
            .WithStartOnSiteMilestone(startOnSiteMilestone)
            .Build();

        // when
        var result = testCandidate.CanMilestoneBeClaimed(MilestoneType.StartOnSite);

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldStartOnSiteMilestoneReturnFalse_WhenAcquisitionMilestoneHasNotBeenClaimed()
    {
        // given
        var acquisitionMilestone = MilestoneClaimTestBuilder.New().NotClaimed().Build();
        var startOnSiteMilestone = MilestoneClaimTestBuilder.New().NotClaimed().Build();
        var testCandidate = PhaseEntityTestBuilder.New()
            .WithAcquisitionMilestone(acquisitionMilestone)
            .WithStartOnSiteMilestone(startOnSiteMilestone)
            .Build();

        // when
        var result = testCandidate.CanMilestoneBeClaimed(MilestoneType.StartOnSite);

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldStartOnSiteMilestoneReturnFalse_WhenAcquisitionAndStartOnSiteMilestonesHasBeenClaimed()
    {
        // given
        var acquisitionMilestone = MilestoneClaimTestBuilder.New().Claimed().Build();
        var startOnSiteMilestone = MilestoneClaimTestBuilder.New().Claimed().Build();
        var testCandidate = PhaseEntityTestBuilder.New()
            .WithAcquisitionMilestone(acquisitionMilestone)
            .WithStartOnSiteMilestone(startOnSiteMilestone)
            .Build();

        // when
        var result = testCandidate.CanMilestoneBeClaimed(MilestoneType.StartOnSite);

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldCompletionMilestoneReturnTrue_WhenStartOnSiteMilestoneHasBeenClaimedAndCompletionNot()
    {
        // given
        var startOnSiteMilestone = MilestoneClaimTestBuilder.New().Claimed().Build();
        var completionMilestone = MilestoneClaimTestBuilder.New().NotClaimed().Build();
        var testCandidate = PhaseEntityTestBuilder.New()
            .WithStartOnSiteMilestone(startOnSiteMilestone)
            .WithCompletionMilestone(completionMilestone)
            .Build();

        // when
        var result = testCandidate.CanMilestoneBeClaimed(MilestoneType.Completion);

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldCompletionMilestoneReturnTrue_WhenStartOnSiteDoesNotExistAndCompletionMilestoneHasNotBeenClaimed()
    {
        // given
        var completionMilestone = MilestoneClaimTestBuilder.New().NotClaimed().Build();
        var testCandidate = PhaseEntityTestBuilder.New()
            .WithAcquisitionMilestone(null)
            .WithStartOnSiteMilestone(null)
            .WithCompletionMilestone(completionMilestone)
            .Build();

        // when
        var result = testCandidate.CanMilestoneBeClaimed(MilestoneType.Completion);

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldCompletionMilestoneReturnFalse_WhenStartOnSiteAndCompletionMilestoneHasBeenClaimed()
    {
        // given
        var startOnSiteMilestone = MilestoneClaimTestBuilder.New().Claimed().Build();
        var completionMilestone = MilestoneClaimTestBuilder.New().Claimed().Build();
        var testCandidate = PhaseEntityTestBuilder.New()
            .WithStartOnSiteMilestone(startOnSiteMilestone)
            .WithCompletionMilestone(completionMilestone)
            .Build();

        // when
        var result = testCandidate.CanMilestoneBeClaimed(MilestoneType.Completion);

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldCompletionMilestoneReturnFalse_WhenCompletionMilestoneHasNotBeenClaimed()
    {
        // given
        var startOnSiteMilestone = MilestoneClaimTestBuilder.New().NotClaimed().Build();
        var completionMilestone = MilestoneClaimTestBuilder.New().NotClaimed().Build();
        var testCandidate = PhaseEntityTestBuilder.New()
            .WithStartOnSiteMilestone(startOnSiteMilestone)
            .WithCompletionMilestone(completionMilestone)
            .Build();

        // when
        var result = testCandidate.CanMilestoneBeClaimed(MilestoneType.Completion);

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldCompletionMilestoneReturnFalse_WhenStartOnSiteDoesNotExistAndCompletionMilestoneHasBeenClaimed()
    {
        // given
        var completionMilestone = MilestoneClaimTestBuilder.New().Claimed().Build();
        var testCandidate = PhaseEntityTestBuilder.New()
            .WithAcquisitionMilestone(null)
            .WithStartOnSiteMilestone(null)
            .WithCompletionMilestone(completionMilestone)
            .Build();

        // when
        var result = testCandidate.CanMilestoneBeClaimed(MilestoneType.Completion);

        // then
        result.Should().BeFalse();
    }
}
