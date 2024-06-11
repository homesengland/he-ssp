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
    private static readonly DateTime Now = DateTime.UtcNow;

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
        action.Should().Throw<DomainValidationException>().WithMessage("The acquisition date must be before, or the same as, the start on site date");
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
        action.Should().Throw<DomainValidationException>().WithMessage("The start on site date must be before, or the same as, the completion date");
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
        action.Should().Throw<DomainValidationException>().WithMessage("The forecast acquisition claim date must be before, or the same as, the forecast start on site claim date");
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
        action.Should().Throw<DomainValidationException>().WithMessage("The forecast start on site claim date must be before, or the same as, the forecast completion claim date");
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
        action.Should().Throw<DomainValidationException>().WithMessage("Dates fall outside of the programme requirements. Check your dates against the published funding requirements");
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
        action.Should().Throw<DomainValidationException>().WithMessage("Dates fall outside of the programme requirements. Check your dates against the published funding requirements");
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
        action.Should().Throw<DomainValidationException>().WithMessage("Dates fall outside of the programme requirements. Check your dates against the published funding requirements");
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
        action.Should().Throw<DomainValidationException>().WithMessage("The forecast acquisition claim date must be today or in the future");
    }

    [Fact]
    public void ShouldThrowException_WhenStartOnSiteDateClaimDateIsInThePast()
    {
        // given
        var programme = new ProgrammeBuilder().Build();
        var milestones = new DeliveryPhaseMilestones(
            false,
            startOnSiteMilestone: new StartOnSiteMilestoneDetailsBuilder().WithMilestoneDate(Now.AddDays(-10)).WithPaymentDate(Now.AddDays(-1)).Build());

        // when
        var action = () => milestones.ValidateMilestoneDates(programme, _dateTimeProvider);

        // then
        action.Should().Throw<DomainValidationException>().WithMessage("The forecast start on site claim date must be today or in the future");
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
        action.Should().Throw<DomainValidationException>().WithMessage("The forecast completion claim date must be today or in the future");
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
        action.Should().Throw<DomainValidationException>().WithMessage("Dates fall outside of the programme requirements. Check your dates against the published funding requirements");
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
        action.Should().Throw<DomainValidationException>().WithMessage("Dates fall outside of the programme requirements. Check your dates against the published funding requirements");
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
        action.Should().Throw<DomainValidationException>().WithMessage("Dates fall outside of the programme requirements. Check your dates against the published funding requirements");
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
        action.Should().Throw<DomainValidationException>().WithMessage("Dates fall outside of the programme requirements. Check your dates against the published funding requirements");
    }

    [Fact]
    public void ShouldThrowException_WhenAcquisitionMilestoneIsAfterAcquisitionClaimDate()
    {
        // given
        var programme = new ProgrammeBuilder()
            .Build();

        var milestones = new DeliveryPhaseMilestones(
            false,
            acquisitionMilestone: new AcquisitionMilestoneDetailsBuilder().WithMilestoneDate(Now.AddDays(101)).WithPaymentDate(Now).Build());

        // when
        var action = () => milestones.ValidateMilestoneDates(programme, _dateTimeProvider);

        // then
        action.Should().Throw<DomainValidationException>().WithMessage("The acquisition date must be before, or the same as, the forecast acquisition claim date");
    }

    [Fact]
    public void ShouldThrowException_WhenStartOnSiteMilestoneIsAfterStartOnSiteClaimDate()
    {
        // given
        var programme = new ProgrammeBuilder()
            .Build();
        var milestones = new DeliveryPhaseMilestones(
            false,
            startOnSiteMilestone: new StartOnSiteMilestoneDetailsBuilder().WithMilestoneDate(Now.AddDays(101)).WithPaymentDate(Now).Build());

        // when
        var action = () => milestones.ValidateMilestoneDates(programme, _dateTimeProvider);

        // then
        action.Should().Throw<DomainValidationException>().WithMessage("The start on site date must be before, or the same as, the forecast start on site claim date");
    }

    [Fact]
    public void ShouldThrowException_WhenCompletionMilestoneIsAfterCompletionClaimDate()
    {
        // given
        var programme = new ProgrammeBuilder()
            .Build();
        var milestones = new DeliveryPhaseMilestones(
            false,
            completionMilestone: new CompletionMilestoneDetailsBuilder().WithMilestoneDate(Now.AddDays(101)).WithPaymentDate(Now).Build());

        // when
        var action = () => milestones.ValidateMilestoneDates(programme, _dateTimeProvider);

        // then
        action.Should().Throw<DomainValidationException>().WithMessage("The completion date must be before, or the same as, the forecast completion claim date");
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
