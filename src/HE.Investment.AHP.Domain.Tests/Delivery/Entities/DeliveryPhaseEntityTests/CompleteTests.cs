using FluentAssertions;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.DeliveryPhaseEntityTests;

public class CompleteTests
{
    [Fact]
    public void ShouldSetCompleted_WhenAllQuestionsAnsweredForUnregisteredBody()
    {
        // given
        var testCandidate = new DeliveryPhaseEntityBuilder()
            .WithUnregisteredBody()
            .WithAdditionalPaymentRequested(new IsAdditionalPaymentRequested(true))
            .WithDeliveryPhaseMilestones(new DeliveryPhaseMilestonesBuilder()
                .WithUnregisteredBody()
                .WithoutAcquisitionMilestoneDetails()
                .WithoutStartOnSiteMilestoneDetails()
                .Build())
            .Build();

        // when
        testCandidate.Complete();

        // then
        testCandidate.Status.Should().Be(SectionStatus.Completed);
        testCandidate.IsModified.Should().BeTrue();
    }

    [Fact]
    public void ShouldThrowException_WhenIsAdditionalPaymentRequestedMissing()
    {
        // given
        var testCandidate = new DeliveryPhaseEntityBuilder()
            .WithUnregisteredBody()
            .WithDeliveryPhaseMilestones(new DeliveryPhaseMilestonesBuilder()
                .WithUnregisteredBody()
                .WithoutAcquisitionMilestoneDetails()
                .WithoutStartOnSiteMilestoneDetails()
                .Build())
            .Build();

        // when
        var action = () => testCandidate.Complete();

        // then
        action.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void ShouldSetCompleted_WhenAllQuestionsAnsweredForRegisteredBody()
    {
        // given
        var testCandidate = new DeliveryPhaseEntityBuilder()
            .WithDeliveryPhaseMilestones(new DeliveryPhaseMilestonesBuilder().Build())
            .Build();

        // when
        testCandidate.Complete();

        // then
        testCandidate.Status.Should().Be(SectionStatus.Completed);
        testCandidate.IsModified.Should().BeTrue();
    }

    [Fact]
    public void ShouldThrowException_WhenDeliveryPhaseMilestonesNotAnswered()
    {
        // given
        var testCandidate = new DeliveryPhaseEntityBuilder()
            .WithDeliveryPhaseMilestones(new DeliveryPhaseMilestonesBuilder().WithoutAcquisitionMilestoneDetails().Build())
            .Build();

        // when
        var action = () => testCandidate.Complete();

        // then
        action.Should().Throw<DomainValidationException>();
    }
}
