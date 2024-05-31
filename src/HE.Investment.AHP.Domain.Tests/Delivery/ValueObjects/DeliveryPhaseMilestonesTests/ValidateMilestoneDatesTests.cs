using FluentAssertions;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Common.TestData;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Utils;
using Moq;

namespace HE.Investment.AHP.Domain.Tests.Delivery.ValueObjects.DeliveryPhaseMilestonesTests;

public class ValidateMilestoneDatesTests
{
    private static readonly DateTime Now = new(2024, 05, 29, 10, 15, 20, DateTimeKind.Local);

    private readonly IDateTimeProvider _dateTimeProvider = Mock.Of<IDateTimeProvider>();

    public ValidateMilestoneDatesTests()
    {
        Mock.Get(_dateTimeProvider)
            .Setup(x => x.Now)
            .Returns(Now);
    }

    [Fact]
    public void ShouldThrowException_WhenStartOnSiteMilestoneIsBeforeAcquisitionMilestone()
    {
        // given
        var programme = new ProgrammeBuilder().Build();
        var milestones = new DeliveryPhaseMilestones(
            false,
            new AcquisitionMilestoneDetailsBuilder().WithMilestoneDate(Now.AddDays(10)).WithoutPaymentDate().Build(),
            new StartOnSiteMilestoneDetailsBuilder().WithMilestoneDate(Now.AddDays(9)).WithoutPaymentDate().Build(),
            new CompletionMilestoneDetailsBuilder().WithMilestoneDate(Now.AddDays(11)).WithoutPaymentDate().Build());

        // when
        var action = () => milestones.ValidateMilestoneDates(programme, _dateTimeProvider);

        // then
        action.Should().Throw<DomainValidationException>().WithMessage("Milestone dates should follow each other");
    }

    [Fact]
    public void ShouldThrowException_WhenCompletionMilestoneIsBeforeStartOnSiteMilestone()
    {
        // given
        var programme = new ProgrammeBuilder().Build();
        var milestones = new DeliveryPhaseMilestones(
            false,
            new AcquisitionMilestoneDetailsBuilder().WithMilestoneDate(Now.AddDays(9)).WithoutPaymentDate().Build(),
            new StartOnSiteMilestoneDetailsBuilder().WithMilestoneDate(Now.AddDays(11)).WithoutPaymentDate().Build(),
            new CompletionMilestoneDetailsBuilder().WithMilestoneDate(Now.AddDays(10)).WithoutPaymentDate().Build());

        // when
        var action = () => milestones.ValidateMilestoneDates(programme, _dateTimeProvider);

        // then
        action.Should().Throw<DomainValidationException>().WithMessage("Milestone dates should follow each other");
    }

    [Fact]
    public void ShouldThrowException_WhenStartOnSiteMilestoneClaimDateIsBeforeAcquisitionMilestoneClaimDate()
    {
        // given
        var programme = new ProgrammeBuilder().Build();
        var milestones = new DeliveryPhaseMilestones(
            false,
            new AcquisitionMilestoneDetailsBuilder().WithMilestoneDate(Now.AddDays(10)).WithPaymentDate(Now.AddDays(22)).Build(),
            new StartOnSiteMilestoneDetailsBuilder().WithMilestoneDate(Now.AddDays(20)).WithPaymentDate(Now.AddDays(21)).Build(),
            new CompletionMilestoneDetailsBuilder().WithMilestoneDate(Now.AddDays(30)).WithPaymentDate(Now.AddDays(31)).Build());

        // when
        var action = () => milestones.ValidateMilestoneDates(programme, _dateTimeProvider);

        // then
        action.Should().Throw<DomainValidationException>().WithMessage("Milestone claim dates should follow each other");
    }

    [Fact]
    public void ShouldThrowException_WhenCompletionMilestoneClaimDateIsBeforeStartOnSiteMilestoneClaimDate()
    {
        // given
        var programme = new ProgrammeBuilder().Build();
        var milestones = new DeliveryPhaseMilestones(
            false,
            new AcquisitionMilestoneDetailsBuilder().WithMilestoneDate(Now.AddDays(10)).WithPaymentDate(Now.AddDays(11)).Build(),
            new StartOnSiteMilestoneDetailsBuilder().WithMilestoneDate(Now.AddDays(20)).WithPaymentDate(Now.AddDays(32)).Build(),
            new CompletionMilestoneDetailsBuilder().WithMilestoneDate(Now.AddDays(30)).WithPaymentDate(Now.AddDays(31)).Build());

        // when
        var action = () => milestones.ValidateMilestoneDates(programme, _dateTimeProvider);

        // then
        action.Should().Throw<DomainValidationException>().WithMessage("Milestone claim dates should follow each other");
    }

    [Fact]
    public void ShouldThrowException_WhenAcquisitionClaimDateIsBeforeProgrammeFundingDate()
    {
        // given
        var programme = new ProgrammeBuilder()
            .WithFundingStartDate(Now.AddDays(20))
            .Build();
        var milestones = new DeliveryPhaseMilestones(
            false,
            new AcquisitionMilestoneDetailsBuilder().WithMilestoneDate(Now.AddDays(10)).WithPaymentDate(Now.AddDays(19)).Build());

        // when
        var action = () => milestones.ValidateMilestoneDates(programme, _dateTimeProvider);

        // then
        action.Should().Throw<DomainValidationException>().WithMessage("Milestone claim dates should be withing Programme Funding dates");
    }

    [Fact]
    public void ShouldThrowException_WhenCompletionClaimDateIsAfterProgrammeFundingDate()
    {
        // given
        var programme = new ProgrammeBuilder()
            .WithFundingEndDate(Now.AddDays(100))
            .Build();
        var milestones = new DeliveryPhaseMilestones(
            false,
            completionMilestone: new CompletionMilestoneDetailsBuilder().WithMilestoneDate(Now.AddDays(10)).WithPaymentDate(Now.AddDays(101)).Build());

        // when
        var action = () => milestones.ValidateMilestoneDates(programme, _dateTimeProvider);

        // then
        action.Should().Throw<DomainValidationException>().WithMessage("Milestone claim dates should be withing Programme Funding dates");
    }

    [Fact]
    public void ShouldThrowException_WhenIsOnlyCompletionMilestoneAndCompletionClaimDateIsBeforeProgrammeFundingDate()
    {
        // given
        var programme = new ProgrammeBuilder()
            .WithFundingStartDate(Now.AddDays(20))
            .WithFundingEndDate(Now.AddDays(100))
            .Build();
        var milestones = new DeliveryPhaseMilestones(
            true,
            completionMilestone: new CompletionMilestoneDetailsBuilder().WithMilestoneDate(Now.AddDays(10)).WithPaymentDate(Now.AddDays(19)).Build());

        // when
        var action = () => milestones.ValidateMilestoneDates(programme, _dateTimeProvider);

        // then
        action.Should().Throw<DomainValidationException>().WithMessage("Milestone claim dates should be withing Programme Funding dates");
    }

    [Fact]
    public void ShouldThrowException_WhenAcquisitionClaimDateIsInThePast()
    {
        // given
        var programme = new ProgrammeBuilder().Build();
        var milestones = new DeliveryPhaseMilestones(
            false,
            new AcquisitionMilestoneDetailsBuilder().WithMilestoneDate(Now.AddDays(-10)).WithPaymentDate(Now.AddDays(-1)).Build());

        // when
        var action = () => milestones.ValidateMilestoneDates(programme, _dateTimeProvider);

        // then
        action.Should().Throw<DomainValidationException>().WithMessage("Milestone claim dates should be in the future");
    }

    [Fact]
    public void ShouldThrowException_WhenIsOnlyCompletionMilestoneAndCompletionClaimDateIsInThePast()
    {
        // given
        var programme = new ProgrammeBuilder().Build();
        var milestones = new DeliveryPhaseMilestones(
            true,
            completionMilestone: new CompletionMilestoneDetailsBuilder().WithMilestoneDate(Now.AddDays(-10)).WithPaymentDate(Now.AddDays(-1)).Build());

        // when
        var action = () => milestones.ValidateMilestoneDates(programme, _dateTimeProvider);

        // then
        action.Should().Throw<DomainValidationException>().WithMessage("Milestone claim dates should be in the future");
    }

    [Fact]
    public void ShouldThrowException_WhenStartOnSiteMilestoneIsBeforeProgrammeStartOnSiteDate()
    {
        // given
        var programme = new ProgrammeBuilder()
            .WithStartOnSiteStartDate(Now.AddDays(10))
            .Build();
        var milestones = new DeliveryPhaseMilestones(
            false,
            startOnSiteMilestone: new StartOnSiteMilestoneDetailsBuilder().WithMilestoneDate(Now.AddDays(9)).WithoutPaymentDate().Build());

        // when
        var action = () => milestones.ValidateMilestoneDates(programme, _dateTimeProvider);

        // then
        action.Should().Throw<DomainValidationException>().WithMessage("Start on Site milestone dates should be within programme Start on Site dates");
    }

    [Fact]
    public void ShouldThrowException_WhenStartOnSiteMilestoneIsAfterProgrammeStartOnSiteDate()
    {
        // given
        var programme = new ProgrammeBuilder()
            .WithStartOnSiteEndDate(Now.AddDays(100))
            .Build();
        var milestones = new DeliveryPhaseMilestones(
            false,
            startOnSiteMilestone: new StartOnSiteMilestoneDetailsBuilder().WithMilestoneDate(Now.AddDays(101)).WithoutPaymentDate().Build());

        // when
        var action = () => milestones.ValidateMilestoneDates(programme, _dateTimeProvider);

        // then
        action.Should().Throw<DomainValidationException>().WithMessage("Start on Site milestone dates should be within programme Start on Site dates");
    }

    [Fact]
    public void ShouldThrowException_WhenAndCompletionMilestoneIsBeforeProgrammeCompletionDate()
    {
        // given
        var programme = new ProgrammeBuilder()
            .WithCompletionStartDate(Now.AddDays(10))
            .Build();
        var milestones = new DeliveryPhaseMilestones(
            false,
            completionMilestone: new CompletionMilestoneDetailsBuilder().WithMilestoneDate(Now.AddDays(9)).WithoutPaymentDate().Build());

        // when
        var action = () => milestones.ValidateMilestoneDates(programme, _dateTimeProvider);

        // then
        action.Should().Throw<DomainValidationException>().WithMessage("Completion milestone dates should be within programme Completion dates");
    }

    [Fact]
    public void ShouldThrowException_WhenCompletionMilestoneIsAfterProgrammeCompletionDate()
    {
        // given
        var programme = new ProgrammeBuilder()
            .WithCompletionEndDate(Now.AddDays(100))
            .Build();
        var milestones = new DeliveryPhaseMilestones(
            false,
            completionMilestone: new CompletionMilestoneDetailsBuilder().WithMilestoneDate(Now.AddDays(101)).WithoutPaymentDate().Build());

        // when
        var action = () => milestones.ValidateMilestoneDates(programme, _dateTimeProvider);

        // then
        action.Should().Throw<DomainValidationException>().WithMessage("Completion milestone dates should be within programme Completion dates");
    }

    [Fact]
    public void ShouldNotThrowException_WhenIsOnlyCompletionMilestoneIsValid()
    {
        // given
        var programme = new ProgrammeBuilder()
            .WithFundingStartDate(Now.AddDays(100))
            .WithFundingEndDate(Now.AddDays(101))
            .WithCompletionStartDate(Now.AddDays(50))
            .WithCompletionEndDate(Now.AddDays(51))
            .Build();
        var milestones = new DeliveryPhaseMilestones(
            true,
            completionMilestone: new CompletionMilestoneDetailsBuilder().WithMilestoneDate(Now.AddDays(50)).WithPaymentDate(Now.AddDays(100)).Build());

        // when
        var action = () => milestones.ValidateMilestoneDates(programme, _dateTimeProvider);

        // then
        action.Should().NotThrow();
    }

    [Fact]
    public void ShouldNotThrowException_WhenAllMilestonesAreValid()
    {
        // given
        var programme = new ProgrammeBuilder()
            .WithFundingStartDate(Now.AddDays(15))
            .WithFundingEndDate(Now.AddDays(25))
            .WithStartOnSiteStartDate(Now.AddDays(14))
            .WithStartOnSiteEndDate(Now.AddDays(15))
            .WithCompletionStartDate(Now.AddDays(19))
            .WithCompletionEndDate(Now.AddDays(20))
            .Build();
        var milestones = new DeliveryPhaseMilestones(
            false,
            new AcquisitionMilestoneDetailsBuilder().WithMilestoneDate(Now.AddDays(10)).WithPaymentDate(Now.AddDays(15)).Build(),
            new StartOnSiteMilestoneDetailsBuilder().WithMilestoneDate(Now.AddDays(14)).WithPaymentDate(Now.AddDays(20)).Build(),
            new CompletionMilestoneDetailsBuilder().WithMilestoneDate(Now.AddDays(19)).WithPaymentDate(Now.AddDays(25)).Build());

        // when
        var action = () => milestones.ValidateMilestoneDates(programme, _dateTimeProvider);

        // then
        action.Should().NotThrow();
    }
}
