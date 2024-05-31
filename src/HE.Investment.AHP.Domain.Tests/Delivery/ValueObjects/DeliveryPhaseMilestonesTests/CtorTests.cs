using FluentAssertions;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Delivery.ValueObjects.DeliveryPhaseMilestonesTests;

public class CtorTests
{
    [Fact]
    public void ShouldThrowException_WhenAcquisitionMilestoneDetailsProvidedAndIsOnlyCompletionMilestone()
    {
        // given
        var acquisitionMilestone = new AcquisitionMilestoneDetailsBuilder().Build();
        var completionMilestone = new CompletionMilestoneDetailsBuilder().Build();

        // when
        var create = () => new DeliveryPhaseMilestones(true, acquisitionMilestone, null, completionMilestone);

        // then
        create.Should().Throw<DomainValidationException>().WithMessage("Cannot provide Acquisition Milestone details");
    }

    [Fact]
    public void ShouldThrowException_WhenStartOnSiteMilestoneDetailsProvidedAndIsOnlyCompletionMilestone()
    {
        // given
        var startOnSiteMilestone = new StartOnSiteMilestoneDetailsBuilder().Build();
        var completionMilestone = new CompletionMilestoneDetailsBuilder().Build();

        // when
        var create = () => new DeliveryPhaseMilestones(true, null, startOnSiteMilestone, completionMilestone);

        // then
        create.Should().Throw<DomainValidationException>().WithMessage("Cannot provide Start On Site Milestone details");
    }

    [Fact]
    public void ShouldCreateDeliveryPhaseMilestones_WhenIsOnlyCompletionMilestoneAndOnlyCompletionMilestoneIsProvided()
    {
        // given
        var completionMilestone = new CompletionMilestoneDetailsBuilder().Build();

        // when
        var result = new DeliveryPhaseMilestones(true, null, null, completionMilestone);

        // then
        result.IsOnlyCompletionMilestone.Should().BeTrue();
        result.AcquisitionMilestone.Should().BeNull();
        result.StartOnSiteMilestone.Should().BeNull();
        result.CompletionMilestone.Should().Be(completionMilestone);
    }

    [Fact]
    public void ShouldCreateDeliveryPhaseMilestones_WhenIsNotOnlyCompletionMilestoneAndAllMilestonesAreProvided()
    {
        // given
        var acquisitionMilestone = new AcquisitionMilestoneDetailsBuilder().Build();
        var startOnSiteMilestone = new StartOnSiteMilestoneDetailsBuilder().Build();
        var completionMilestone = new CompletionMilestoneDetailsBuilder().Build();

        // when
        var result = new DeliveryPhaseMilestones(false, acquisitionMilestone, startOnSiteMilestone, completionMilestone);

        // then
        result.IsOnlyCompletionMilestone.Should().BeFalse();
        result.AcquisitionMilestone.Should().Be(acquisitionMilestone);
        result.StartOnSiteMilestone.Should().Be(startOnSiteMilestone);
        result.CompletionMilestone.Should().Be(completionMilestone);
    }

    [Fact]
    public void ShouldCreateDeliveryPhaseMilestones_WhenIsNotOnlyCompletionMilestoneAndNoMilestonesAreProvided()
    {
        // given & when
        var result = new DeliveryPhaseMilestones(false);

        // then
        result.IsOnlyCompletionMilestone.Should().BeFalse();
        result.AcquisitionMilestone.Should().BeNull();
        result.StartOnSiteMilestone.Should().BeNull();
        result.CompletionMilestone.Should().BeNull();
    }
}
