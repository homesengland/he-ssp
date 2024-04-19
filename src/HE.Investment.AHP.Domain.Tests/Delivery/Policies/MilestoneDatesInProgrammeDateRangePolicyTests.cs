using FluentAssertions;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Delivery.Policies;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Programme;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;
using HE.Investments.Common.Contract.Exceptions;
using Moq;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Policies;

public class MilestoneDatesInProgrammeDateRangePolicyTests
{
    private readonly AhpProgramme _programme = new AhpProgrammeBuilder().Build();
    private readonly MilestoneDatesInProgrammeDateRangePolicy _testCandidate;

    public MilestoneDatesInProgrammeDateRangePolicyTests()
    {
        var mock = new Mock<IAhpProgrammeRepository>();
        mock.Setup(r => r.GetProgramme(CancellationToken.None))
            .ReturnsAsync(_programme);

        _testCandidate = new MilestoneDatesInProgrammeDateRangePolicy(mock.Object);
    }

    [Fact]
    public void ShouldDoNotThrowDomainValidationException_WhenAcquisitionPaymentDatesInProgrammeDates()
    {
        // given
        var milestones = new DeliveryPhaseMilestonesBuilder()
            .Build();

        // when
        var action = async () => await _testCandidate.Validate(milestones, CancellationToken.None);

        // then
        action
            .Should()
            .NotThrowAsync<DomainValidationException>();
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenAcquisitionDateBeforeProgrammeStartDate()
    {
        var acquisitionMilestoneDetails = new AcquisitionMilestoneDetailsBuilder()
            .WithAcquisitionDate(_programme.ProgrammeDates.ProgrammeEndDate.AddDays(1))
            .WithoutPaymentDate()
            .Build();

        // when && then
        ExecuteAndAssert(acquisitionMilestoneDetails, "The milestone date must be within the programme date");
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenAcquisitionDateAfterProgrammeEndDate()
    {
        // given
        var acquisitionMilestoneDetails = new AcquisitionMilestoneDetailsBuilder()
            .WithAcquisitionDate(_programme.ProgrammeDates.ProgrammeStartDate.AddDays(-1))
            .WithoutPaymentDate()
            .Build();

        // when && then
        ExecuteAndAssert(acquisitionMilestoneDetails, "The milestone date must be within the programme date");
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenAcquisitionPaymentDateBeforeProgrammeStartDate()
    {
        // given
        var acquisitionMilestoneDetails = new AcquisitionMilestoneDetailsBuilder()
            .WithoutAcquisitionDate()
            .WithPaymentDate(_programme.ProgrammeDates.ProgrammeStartDate.AddDays(-1))
            .Build();

        // when && then
        ExecuteAndAssert(acquisitionMilestoneDetails, "The milestone payment date must be within the programme date");
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenAcquisitionPaymentDateAfterProgrammeEndDate()
    {
        // given
        var acquisitionMilestoneDetails = new AcquisitionMilestoneDetailsBuilder()
            .WithoutAcquisitionDate()
            .WithPaymentDate(_programme.ProgrammeDates.ProgrammeEndDate.AddDays(1))
            .Build();

        // when && then
        ExecuteAndAssert(acquisitionMilestoneDetails, "The milestone payment date must be within the programme date");
    }

    private void ExecuteAndAssert(AcquisitionMilestoneDetails acquisitionMilestoneDetails, string errorMessage)
    {
        var milestones = new DeliveryPhaseMilestonesBuilder()
            .WithAcquisitionMilestoneDetails(acquisitionMilestoneDetails)
            .WithoutStartOnSiteMilestoneDetails()
            .WithoutCompletionMilestoneDetails()
            .Build();

        // when
        var action = async () => await _testCandidate.Validate(milestones, CancellationToken.None);

        // then
        AssertException(action, errorMessage);
    }

    private void AssertException(Func<Task> action, string errorMessage)
    {
        action
            .Should()
            .ThrowExactlyAsync<DomainValidationException>()
            .WithMessage(errorMessage);
    }
}
