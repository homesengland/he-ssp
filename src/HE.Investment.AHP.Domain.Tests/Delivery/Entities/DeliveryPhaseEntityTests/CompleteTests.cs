using FluentAssertions;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.DeliveryPhaseEntityTests;

public class CompleteTests
{
    [Fact]
    public void ShouldSetCompleted_WhenAllQuestionsAnsweredForRegisteredBody()
    {
        // given
        var testCandidate = CreateValidBuilder()
            .Build();

        // when
        testCandidate.Complete();

        // then
        testCandidate.Status.Should().Be(SectionStatus.Completed);
        testCandidate.IsModified.Should().BeTrue();
    }

    [Fact]
    public void ShouldSetCompleted_WhenAllQuestionsAnsweredForUnregisteredBody()
    {
        // given
        var testCandidate = CreateValidBuilder()
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
    public void ShouldSetCompleted_WhenReconfiguringExisting()
    {
        // given
        var testCandidate = CreateValidBuilder()
            .WithTypeOfHomes(TypeOfHomes.Rehab)
            .WithReconfiguringExisting()
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
        var testCandidate = CreateValidBuilder()
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
    public void ShouldThrowException_WhenDeliveryPhaseMilestonesNotAnswered()
    {
        // given
        var testCandidate = CreateValidBuilder()
            .WithDeliveryPhaseMilestones(new DeliveryPhaseMilestonesBuilder().WithoutAcquisitionMilestoneDetails().Build())
            .Build();

        // when
        var action = () => testCandidate.Complete();

        // then
        action.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void ShouldThrowException_WhenBuildActivityNotAnswered()
    {
        // given
        var testCandidate = CreateValidBuilder()
            .WithoutBuildActivity()
            .Build();

        // when
        var action = () => testCandidate.Complete();

        // then
        action.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void ShouldThrowException_WhenTypeOfHomeNotAnswered()
    {
        // given
        var testCandidate = CreateValidBuilder()
            .WithoutTypeOfHomes()
            .Build();

        // when
        var action = () => testCandidate.Complete();

        // then
        action.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void ShouldThrowException_WhenReconfiguringExistingNotAnswered()
    {
        // given
        var testCandidate = CreateValidBuilder()
            .WithTypeOfHomes(TypeOfHomes.Rehab)
            .Build();

        // when
        var action = () => testCandidate.Complete();

        // then
        action.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void ShouldThrowException_WhenHomesToDeliverMissing()
    {
        // given
        var testCandidate = CreateValidBuilder()
            .WithoutHomesToDeliver()
            .Build();

        // when
        var action = () => testCandidate.Complete();

        // then
        action.Should().Throw<DomainValidationException>();
    }

    private DeliveryPhaseEntityBuilder CreateValidBuilder()
    {
        return new DeliveryPhaseEntityBuilder().WithHomesToBeDelivered();
    }
}
