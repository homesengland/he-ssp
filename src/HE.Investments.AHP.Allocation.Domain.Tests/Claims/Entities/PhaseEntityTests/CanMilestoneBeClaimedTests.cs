using FluentAssertions;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Tests.TestObjectBuilders;
using Xunit;

namespace HE.Investments.AHP.Allocation.Domain.Tests.Claims.Entities.PhaseEntityTests;

public class CanMilestoneBeClaimedTests
{
    [Fact]
    public void ShouldAcquisitionMilestoneReturnTrue_WhenItHasNotBeenSubmitted()
    {
        // given
        var acquisitionMilestone = MilestoneClaimTestBuilder.Draft().Build();
        var testCandidate = PhaseEntityTestBuilder.New().WithAcquisitionMilestone(acquisitionMilestone).Build();

        // when
        var result = testCandidate.CanMilestoneBeClaimed(MilestoneType.Acquisition);

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldAcquisitionMilestoneReturnFalse_WhenItHasBeenSubmitted()
    {
        // given
        var acquisitionMilestone = MilestoneClaimTestBuilder.Draft().Submitted().Build();
        var testCandidate = PhaseEntityTestBuilder.New().WithAcquisitionMilestone(acquisitionMilestone).Build();

        // when
        var result = testCandidate.CanMilestoneBeClaimed(MilestoneType.Acquisition);

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldStartOnSiteMilestoneReturnTrue_WhenAcquisitionMilestoneHasBeenSubmittedAndStartOnSiteNot()
    {
        // given
        var acquisitionMilestone = MilestoneClaimTestBuilder.Draft().Submitted().Build();
        var startOnSiteMilestone = MilestoneClaimTestBuilder.Draft().Build();
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
    public void ShouldStartOnSiteMilestoneReturnFalse_WhenAcquisitionMilestoneHasNotBeenSubmitted()
    {
        // given
        var acquisitionMilestone = MilestoneClaimTestBuilder.Draft().Build();
        var startOnSiteMilestone = MilestoneClaimTestBuilder.Draft().Build();
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
    public void ShouldStartOnSiteMilestoneReturnFalse_WhenAcquisitionAndStartOnSiteMilestonesHasBeenSubmitted()
    {
        // given
        var acquisitionMilestone = MilestoneClaimTestBuilder.Draft().Submitted().Build();
        var startOnSiteMilestone = MilestoneClaimTestBuilder.Draft().Submitted().Build();
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
    public void ShouldCompletionMilestoneReturnTrue_WhenStartOnSiteMilestoneHasBeenSubmittedAndCompletionNot()
    {
        // given
        var startOnSiteMilestone = MilestoneClaimTestBuilder.Draft().Submitted().Build();
        var completionMilestone = MilestoneClaimTestBuilder.Draft().Build();
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
    public void ShouldCompletionMilestoneReturnTrue_WhenStartOnSiteDoesNotExistAndCompletionMilestoneHasNotBeenSubmitted()
    {
        // given
        var completionMilestone = MilestoneClaimTestBuilder.Draft().Build();
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
    public void ShouldCompletionMilestoneReturnFalse_WhenStartOnSiteAndCompletionMilestoneHasBeenSubmitted()
    {
        // given
        var startOnSiteMilestone = MilestoneClaimTestBuilder.Draft().Submitted().Build();
        var completionMilestone = MilestoneClaimTestBuilder.Draft().Submitted().Build();
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
    public void ShouldCompletionMilestoneReturnFalse_WhenCompletionMilestoneHasNotBeenSubmitted()
    {
        // given
        var startOnSiteMilestone = MilestoneClaimTestBuilder.Draft().Build();
        var completionMilestone = MilestoneClaimTestBuilder.Draft().Build();
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
    public void ShouldCompletionMilestoneReturnFalse_WhenStartOnSiteDoesNotExistAndCompletionMilestoneHasBeenSubmitted()
    {
        // given
        var completionMilestone = MilestoneClaimTestBuilder.Draft().Submitted().Build();
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
