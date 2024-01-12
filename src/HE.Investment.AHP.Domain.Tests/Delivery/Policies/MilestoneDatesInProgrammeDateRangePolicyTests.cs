using FluentAssertions;
using HE.Investment.AHP.Domain.Delivery.Policies;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;
using HE.Investments.Account.Contract.UserOrganisation;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.Repositories;
using HE.Investments.Common.Contract.Exceptions;
using Moq;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Policies;

public class MilestoneDatesInProgrammeDateRangePolicyTests
{
    private readonly ProgrammeBasicInfo _programmeBasicInfo = new ProgrammeBasicInfoBuilder().Build();
    private readonly MilestoneDatesInProgrammeDateRangePolicy _testCandidate;

    public MilestoneDatesInProgrammeDateRangePolicyTests()
    {
        var mock = new Mock<IProgrammeRepository>();
        mock.Setup(r => r.GetCurrentProgramme(ProgrammeType.Ahp))
            .ReturnsAsync(_programmeBasicInfo);

        _testCandidate = new MilestoneDatesInProgrammeDateRangePolicy(mock.Object);
    }

    [Fact]
    public void ShouldDoNotThrowDomainValidationException_WhenAcquisitionPaymentDatesInProgrammeDates()
    {
        // given
        var milestones = new DeliveryPhaseMilestonesBuilder()
            .Build();

        // when
        var action = async () => await _testCandidate.Validate(milestones);

        // then
        action
            .Should()
            .NotThrowAsync<DomainValidationException>();
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenAcquisitionDateBeforeProgrammeStartDate()
    {
        var acquisitionMilestoneDetails = new AcquisitionMilestoneDetailsBuilder()
            .WithAcquisitionDate(_programmeBasicInfo.EndAt.AddDays(1))
            .WithoutPaymentDate()
            .Build();

        // when && then
        ExecuteAndAssert(acquisitionMilestoneDetails, "Milestone date have to be within programme dates.");
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenAcquisitionDateAfterProgrammeEndDate()
    {
        // given
        var acquisitionMilestoneDetails = new AcquisitionMilestoneDetailsBuilder()
            .WithAcquisitionDate(_programmeBasicInfo.StartAt.AddDays(-1))
            .WithoutPaymentDate()
            .Build();

        // when && then
        ExecuteAndAssert(acquisitionMilestoneDetails, "Milestone date have to be within programme dates.");
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenAcquisitionPaymentDateBeforeProgrammeStartDate()
    {
        // given
        var acquisitionMilestoneDetails = new AcquisitionMilestoneDetailsBuilder()
            .WithoutAcquisitionDate()
            .WithPaymentDate(_programmeBasicInfo.StartAt.AddDays(-1))
            .Build();

        // when && then
        ExecuteAndAssert(acquisitionMilestoneDetails, "Milestone payment date have to be within programme dates.");
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenAcquisitionPaymentDateAfterProgrammeEndDate()
    {
        // given
        var acquisitionMilestoneDetails = new AcquisitionMilestoneDetailsBuilder()
            .WithoutAcquisitionDate()
            .WithPaymentDate(_programmeBasicInfo.EndAt.AddDays(1))
            .Build();

        // when && then
        ExecuteAndAssert(acquisitionMilestoneDetails, "Milestone payment date have to be within programme dates.");
    }

    private void ExecuteAndAssert(AcquisitionMilestoneDetails acquisitionMilestoneDetails, string errorMessage)
    {
        var milestones = new DeliveryPhaseMilestonesBuilder()
            .WithAcquisitionMilestoneDetails(acquisitionMilestoneDetails)
            .WithoutStartOnSiteMilestoneDetails()
            .WithoutCompletionMilestoneDetails()
            .Build();

        // when
        var action = async () => await _testCandidate.Validate(milestones);

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
