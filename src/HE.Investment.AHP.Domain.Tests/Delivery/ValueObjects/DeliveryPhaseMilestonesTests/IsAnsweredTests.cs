using FluentAssertions;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;

namespace HE.Investment.AHP.Domain.Tests.Delivery.ValueObjects.DeliveryPhaseMilestonesTests;

public class IsAnsweredTests
{
    [Fact]
    public void ShouldReturnTrue_WhenIsOnlyCompletionMilestoneAndCompletionMilestoneIsAnswered()
    {
        // given
        var milestones = new DeliveryPhaseMilestonesBuilder()
            .WithIsOnlyCompletionMilestone(true)
            .WithCompletionMilestone(CompletionMilestoneDetailsBuilder.Answered)
            .Build();

        // when
        var result = milestones.IsAnswered();

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnTrue_WhenAllMilestonesAreAnswered()
    {
        // given
        var milestones = new DeliveryPhaseMilestonesBuilder()
            .WithIsOnlyCompletionMilestone(false)
            .WithAcquisitionMilestone(AcquisitionMilestoneDetailsBuilder.Answered)
            .WithStartOnSiteMilestone(StartOnSiteMilestoneDetailsBuilder.Answered)
            .WithCompletionMilestone(CompletionMilestoneDetailsBuilder.Answered)
            .Build();

        // when
        var result = milestones.IsAnswered();

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnFalse_WhenCompletionMilestoneMissingAndIsOnlyCompletionMilestone()
    {
        // given
        var milestones = new DeliveryPhaseMilestonesBuilder()
            .WithIsOnlyCompletionMilestone(true)
            .Build();

        // when
        var result = milestones.IsAnswered();

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnFalse_WhenAcquisitionMilestoneMissing()
    {
        // given
        var milestones = new DeliveryPhaseMilestonesBuilder()
            .WithIsOnlyCompletionMilestone(false)
            .WithStartOnSiteMilestone(StartOnSiteMilestoneDetailsBuilder.Answered)
            .WithCompletionMilestone(CompletionMilestoneDetailsBuilder.Answered)
            .Build();

        // when
        var result = milestones.IsAnswered();

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnFalse_WhenStartOnSiteMilestoneMissing()
    {
        // given
        var milestones = new DeliveryPhaseMilestonesBuilder()
                .WithIsOnlyCompletionMilestone(false)
                .WithAcquisitionMilestone(AcquisitionMilestoneDetailsBuilder.Answered)
                .WithCompletionMilestone(CompletionMilestoneDetailsBuilder.Answered)
                .Build();

        // when
        var result = milestones.IsAnswered();

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnFalse_WhenCompletionMilestoneMissing()
    {
        // given
        var milestones = new DeliveryPhaseMilestonesBuilder()
            .WithIsOnlyCompletionMilestone(false)
            .WithAcquisitionMilestone(AcquisitionMilestoneDetailsBuilder.Answered)
            .WithStartOnSiteMilestone(StartOnSiteMilestoneDetailsBuilder.Answered)
            .Build();

        // when
        var result = milestones.IsAnswered();

        // then
        result.Should().BeFalse();
    }
}
