using FluentAssertions;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.DeliveryPhaseEntityTests;

public class CompleteTests
{
    private readonly MilestonePaymentDate _validPaymentDate = new("1", "5", "2000");

    [Fact]
    public void ShouldSetCompleted_WhenAllQuestionsAnsweredForUnregisteredBody()
    {
        // given
        var testCandidate = new DeliveryPhaseEntityBuilder()
            .WithUnregisteredBody()
            .WithAdditionalPaymentRequested(new IsAdditionalPaymentRequested(true))
            .WithCompletionMilestoneDetails(new CompletionMilestoneDetails(new CompletionDate("7", "3", "2023"), _validPaymentDate))
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
            .WithCompletionMilestoneDetails(new CompletionMilestoneDetails(new CompletionDate("7", "3", "2023"), _validPaymentDate))
            .Build();

        // when
        var action = () => testCandidate.Complete();

        // then
        action.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void ShouldThrowException_WhenCompletionDateIsMissing()
    {
        // given
        var testCandidate = new DeliveryPhaseEntityBuilder()
            .WithUnregisteredBody()
            .WithAdditionalPaymentRequested(new IsAdditionalPaymentRequested(true))
            .WithCompletionMilestoneDetails(new CompletionMilestoneDetails(null, _validPaymentDate))
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
            .WithAcquisitionMilestoneDetails(new AcquisitionMilestoneDetails(new AcquisitionDate("4", "7", "2012"), _validPaymentDate))
            .WithStartOnSiteMilestoneDetails(new StartOnSiteMilestoneDetails(new StartOnSiteDate("4", "7", "2012"), _validPaymentDate))
            .WithCompletionMilestoneDetails(new CompletionMilestoneDetails(new CompletionDate("7", "3", "2023"), _validPaymentDate))
            .Build();

        // when
        testCandidate.Complete();

        // then
        testCandidate.Status.Should().Be(SectionStatus.Completed);
        testCandidate.IsModified.Should().BeTrue();
    }

    [Fact]
    public void ShouldSetCompleted_WhenStartOnSiteMilestoneDetailsMissing()
    {
        // given
        var testCandidate = new DeliveryPhaseEntityBuilder()
            .WithAcquisitionMilestoneDetails(new AcquisitionMilestoneDetails(new AcquisitionDate("4", "7", "2012"), _validPaymentDate))
            .WithCompletionMilestoneDetails(new CompletionMilestoneDetails(new CompletionDate("7", "3", "2023"), _validPaymentDate))
            .Build();

        // when
        var action = () => testCandidate.Complete();

        // then
        action.Should().Throw<DomainValidationException>();
    }
}
