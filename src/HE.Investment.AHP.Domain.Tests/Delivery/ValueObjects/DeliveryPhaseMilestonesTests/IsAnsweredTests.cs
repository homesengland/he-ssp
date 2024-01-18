using FluentAssertions;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;

namespace HE.Investment.AHP.Domain.Tests.Delivery.ValueObjects.DeliveryPhaseMilestonesTests;

public class IsAnsweredTests
{
    [Fact]
    public void ShouldReturnTrue()
    {
        // given
        var milestones = new DeliveryPhaseMilestonesBuilder()
            .Build();

        // when
        var result = milestones.IsAnswered();

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnFalse_WhenCompletionMilestoneMissingForUnregisteredBody()
    {
        // given
        var milestones = new DeliveryPhaseMilestonesBuilder()
            .WithUnregisteredBody()
            .WithoutAcquisitionMilestoneDetails()
            .WithoutStartOnSiteMilestoneDetails()
            .WithoutCompletionMilestoneDetails()
            .Build();

        // when
        var result = milestones.IsAnswered();

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnFalse_WhenCompletionMilestoneMissingForExistingSatisfactory()
    {
        // given
        var milestones = new DeliveryPhaseMilestonesBuilder()
            .WithBuildActivityOnlyForCompletionMilestone()
            .WithoutAcquisitionMilestoneDetails()
            .WithoutStartOnSiteMilestoneDetails()
            .WithoutCompletionMilestoneDetails()
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
            .WithoutStartOnSiteMilestoneDetails()
            .WithoutCompletionMilestoneDetails()
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
            .WithoutAcquisitionMilestoneDetails()
            .WithoutCompletionMilestoneDetails()
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
            .WithoutAcquisitionMilestoneDetails()
            .WithoutStartOnSiteMilestoneDetails()
            .Build();

        // when
        var result = milestones.IsAnswered();

        // then
        result.Should().BeFalse();
    }
}
