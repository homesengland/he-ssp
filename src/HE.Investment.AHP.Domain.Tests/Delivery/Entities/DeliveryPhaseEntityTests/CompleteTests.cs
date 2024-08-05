using FluentAssertions;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Common.TestData;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Utils;
using Moq;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.DeliveryPhaseEntityTests;

public class CompleteTests
{
    private readonly IDateTimeProvider _dateTimeProvider = Mock.Of<IDateTimeProvider>();

    [Fact]
    public void ShouldSetCompleted_WhenAllQuestionsAnsweredForRegisteredBody()
    {
        // given
        var programme = new ProgrammeBuilder().Build();
        var testCandidate = CreateValidBuilder()
            .Build();

        // when
        testCandidate.Complete(programme, IsSectionCompleted.Yes, _dateTimeProvider);

        // then
        testCandidate.Status.Should().Be(SectionStatus.Completed);
        testCandidate.IsModified.Should().BeTrue();
    }

    [Fact]
    public void ShouldSetCompleted_WhenAllQuestionsAnsweredForUnregisteredBody()
    {
        // given
        var programme = new ProgrammeBuilder().Build();
        var testCandidate = CreateValidBuilder()
            .WithUnregisteredBody()
            .WithAdditionalPaymentRequested(new IsAdditionalPaymentRequested(true))
            .WithCompletionMilestone(new CompletionMilestoneDetailsBuilder().Build())
            .WithoutAcquisitionMilestone()
            .WithoutStartOnSiteMilestone()
            .WithIsOnlyCompletionMilestonePolicy(true)
            .Build();

        // when
        testCandidate.Complete(programme, IsSectionCompleted.Yes, _dateTimeProvider);

        // then
        testCandidate.Status.Should().Be(SectionStatus.Completed);
        testCandidate.IsModified.Should().BeTrue();
    }

    [Fact]
    public void ShouldSetCompleted_WhenReconfiguringExisting()
    {
        // given
        var programme = new ProgrammeBuilder().Build();
        var testCandidate = CreateValidBuilder()
            .WithTypeOfHomes(TypeOfHomes.Rehab)
            .WithReconfiguringExisting()
            .Build();

        // when
        testCandidate.Complete(programme, IsSectionCompleted.Yes, _dateTimeProvider);

        // then
        testCandidate.Status.Should().Be(SectionStatus.Completed);
        testCandidate.IsModified.Should().BeTrue();
    }

    [Fact]
    public void ShouldThrowException_WhenIsAdditionalPaymentRequestedMissing()
    {
        // given
        var programme = new ProgrammeBuilder().Build();
        var testCandidate = CreateValidBuilder()
            .WithUnregisteredBody()
            .WithCompletionMilestone(new CompletionMilestoneDetailsBuilder().Build())
            .WithoutStartOnSiteMilestone()
            .WithoutAcquisitionMilestone()
            .Build();

        // when
        var action = () => testCandidate.Complete(programme, IsSectionCompleted.Yes, _dateTimeProvider);

        // then
        action.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void ShouldThrowException_WhenDeliveryPhaseMilestonesNotAnswered()
    {
        // given
        var programme = new ProgrammeBuilder().Build();
        var testCandidate = CreateValidBuilder()
            .WithoutAcquisitionMilestone()
            .Build();

        // when
        var action = () => testCandidate.Complete(programme, IsSectionCompleted.Yes, _dateTimeProvider);

        // then
        action.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void ShouldThrowException_WhenBuildActivityNotAnswered()
    {
        // given
        var programme = new ProgrammeBuilder().Build();
        var testCandidate = CreateValidBuilder()
            .WithoutBuildActivity()
            .Build();

        // when
        var action = () => testCandidate.Complete(programme, IsSectionCompleted.Yes, _dateTimeProvider);

        // then
        action.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void ShouldThrowException_WhenTypeOfHomeNotAnswered()
    {
        // given
        var programme = new ProgrammeBuilder().Build();
        var testCandidate = CreateValidBuilder()
            .WithoutTypeOfHomes()
            .Build();

        // when
        var action = () => testCandidate.Complete(programme, IsSectionCompleted.Yes, _dateTimeProvider);

        // then
        action.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void ShouldThrowException_WhenReconfiguringExistingNotAnswered()
    {
        // given
        var programme = new ProgrammeBuilder().Build();
        var testCandidate = CreateValidBuilder()
            .WithTypeOfHomes(TypeOfHomes.Rehab)
            .Build();

        // when
        var action = () => testCandidate.Complete(programme, IsSectionCompleted.Yes, _dateTimeProvider);

        // then
        action.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void ShouldThrowException_WhenHomesToDeliverMissing()
    {
        // given
        var programme = new ProgrammeBuilder().Build();
        var testCandidate = CreateValidBuilder()
            .WithoutHomesToDeliver()
            .Build();

        // when
        var action = () => testCandidate.Complete(programme, IsSectionCompleted.Yes, _dateTimeProvider);

        // then
        action.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void ShouldThrowException_WhenIsSectionCompletedIsNotProvided()
    {
        // given
        var programme = new ProgrammeBuilder().Build();
        var testCandidate = CreateValidBuilder().Build();

        // when
        var action = () => testCandidate.Complete(programme, null, _dateTimeProvider);

        // then
        action.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void ShouldSetInProgress_WhenIsSectionCompletedIsNo()
    {
        // given
        var programme = new ProgrammeBuilder().Build();
        var testCandidate = CreateValidBuilder().WithStatus(SectionStatus.Completed).Build();

        // when
        testCandidate.Complete(programme, IsSectionCompleted.No, _dateTimeProvider);

        // then
        testCandidate.Status.Should().Be(SectionStatus.InProgress);
        testCandidate.IsModified.Should().BeTrue();
    }

    [Fact]
    public void ShouldThrowException_WhenMilestoneTranchesAreNotValid()
    {
        // given
        var fundingEndDate = new DateTime(2026, 01, 01, 0, 0, 0, 0, DateTimeKind.Local);
        var programme = new ProgrammeBuilder().WithFundingEndDate(fundingEndDate).Build();
        var testCandidate = CreateValidBuilder()
            .WithCompletionMilestone(new CompletionMilestoneDetailsBuilder().WithPaymentDate(fundingEndDate.AddDays(1)).Build())
            .Build();

        // when
        var action = () => testCandidate.Complete(programme, IsSectionCompleted.Yes, _dateTimeProvider);

        // then
        action.Should().Throw<DomainValidationException>().WithMessage("Dates fall outside of the programme requirements. Check your dates against the published funding requirements\nThe forecast start on site claim date must be before, or the same as, the forecast completion claim date");
    }

    private DeliveryPhaseEntityBuilder CreateValidBuilder()
    {
        return new DeliveryPhaseEntityBuilder().WithHomesToBeDelivered();
    }
}
