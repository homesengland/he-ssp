using System.Globalization;
using FluentAssertions;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Tests.TestObjectBuilders;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Tests.FluentAssertions;
using HE.Investments.Common.Utils;
using HE.Investments.Programme.Contract;
using HE.Investments.Programme.Contract.Enums;
using Moq;
using Xunit;
using AhpProgramme = HE.Investments.Programme.Contract.Programme;

namespace HE.Investments.AHP.Allocation.Domain.Tests.Claims.ValueObjects.MilestoneClaimTests;

public class WithAchievementDateTests
{
    private readonly IDateTimeProvider _dateTimeProvider = Mock.Of<IDateTimeProvider>();

    [Fact]
    public void ShouldThrowException_WhenAchievementDateIsNotProvided()
    {
        // given
        var testCandidate = MilestoneClaimTestBuilder.New()
            .WithType(MilestoneType.Acquisition)
            .Build();

        // when
        var result = () => testCandidate.WithAchievementDate(null, CreateProgramme(), null, _dateTimeProvider);

        // then
        result.Should()
            .Throw<DomainValidationException>()
            .WithSingleError("Enter the achievement date");
    }

    [Fact]
    public void ShouldCreateNewMilestoneClaim_WhenAchievementDateIsProvided()
    {
        // given
        GetDateTimeProviderMock();
        var achievementDate = new DateDetails("10", "07", "2023");
        var testCandidate = MilestoneClaimTestBuilder.New()
            .WithType(MilestoneType.Acquisition)
            .WithMilestoneAchievedDate(achievementDate)
            .Build();
        var programme = CreateProgramme();

        // when
        var result = testCandidate.WithAchievementDate(achievementDate, programme, null, _dateTimeProvider);

        // then
        result.ClaimDate.AchievementDate.Should().Be(achievementDate);
    }

    [Fact]
    public void ShouldThrowException_WhenAchievementDateIsInTheFuture()
    {
        // given
        var achievementDate = new DateDetails(
            DateTime.Today.AddDays(1).Day.ToString(CultureInfo.InvariantCulture),
            DateTime.Today.Month.ToString(CultureInfo.InvariantCulture),
            DateTime.Today.Year.ToString(CultureInfo.InvariantCulture));
        var testCandidate = MilestoneClaimTestBuilder.New()
            .WithType(MilestoneType.Acquisition)
            .Build();
        var programme = CreateProgramme();

        // when
        var result = () => testCandidate.WithAchievementDate(achievementDate, programme, null, _dateTimeProvider);

        // then
        result.Should()
            .Throw<DomainValidationException>()
            .WithSingleError("The date must be today or in the past");
    }

    [Fact]
    public void ShouldThrowException_WhenPreviousSubmissionDateIsAfterAchievementDate()
    {
        // given
        GetDateTimeProviderMock();
        var achievementDate = new DateDetails("10", "07", "2023");
        var laterSubmissionDate = new DateDetails("11", "07", "2023");
        var testCandidate = MilestoneClaimTestBuilder.New()
            .WithType(MilestoneType.Acquisition)
            .Build();
        var programme = CreateProgramme();

        // when
        var result = () => testCandidate.WithAchievementDate(achievementDate, programme, laterSubmissionDate, _dateTimeProvider);

        // then
        result.Should()
            .Throw<DomainValidationException>()
            .WithSingleError(ValidationErrorMessage.DatesOutsideTheProgramme);
    }

    [Fact]
    public void ShouldThrowException_WhenAchievementDateIsBeforeProgrammeStart()
    {
        // given
        GetDateTimeProviderMock();
        var achievementDate = new DateDetails("01", "01", "2020");
        var testCandidate = MilestoneClaimTestBuilder.New()
            .WithType(MilestoneType.Acquisition)
            .Build();
        var programme = CreateProgramme(new DateRange(new DateOnly(2021, 1, 1), new DateOnly(2024, 1, 1)));

        // when
        var result = () => testCandidate.WithAchievementDate(achievementDate, programme, null, _dateTimeProvider);

        // then
        result.Should()
            .Throw<DomainValidationException>()
            .WithSingleError(ValidationErrorMessage.DatesOutsideTheProgramme);
    }

    [Fact]
    public void ShouldThrowException_WhenAchievementDateIsAfterProgrammeStart()
    {
        // given
        GetDateTimeProviderMock();
        var achievementDate = new DateDetails("01", "01", "2024");
        var testCandidate = MilestoneClaimTestBuilder.New()
            .WithType(MilestoneType.Acquisition)
            .Build();
        var programme = CreateProgramme(new DateRange(new DateOnly(2021, 1, 1), new DateOnly(2023, 1, 1)));

        // when
        var result = () => testCandidate.WithAchievementDate(achievementDate, programme, null, _dateTimeProvider);

        // then
        result.Should()
            .Throw<DomainValidationException>()
            .WithSingleError(ValidationErrorMessage.DatesOutsideTheProgramme);
    }

    [Fact]
    public void ShouldThrowException_WhenAchievementDateIsBeforeFundingDateAndGrantAmountIsMoreThenZero()
    {
        // given
        GetDateTimeProviderMock();
        var achievementDate = new DateDetails("01", "01", "2021");
        var testCandidate = MilestoneClaimTestBuilder.New()
            .WithType(MilestoneType.Acquisition)
            .WithGrantApportioned(10000, 40)
            .Build();
        var programme = CreateProgramme(fundingDates: new DateRange(new DateOnly(2022, 1, 1), new DateOnly(2024, 1, 1)));

        // when
        var result = () => testCandidate.WithAchievementDate(achievementDate, programme, null, _dateTimeProvider);

        // then
        result.Should()
            .Throw<DomainValidationException>()
            .WithSingleError(ValidationErrorMessage.DatesOutsideTheProgramme);
    }

    [Fact]
    public void ShouldNotThrowException_WhenAchievementDateIsBeforeFundingDateAndGrantAmountIsZero()
    {
        // given
        GetDateTimeProviderMock();
        var achievementDate = new DateDetails("01", "01", "2021");
        var testCandidate = MilestoneClaimTestBuilder.New()
            .WithType(MilestoneType.Acquisition)
            .WithGrantApportioned(0, 40)
            .Build();
        var programme = CreateProgramme(fundingDates: new DateRange(new DateOnly(2022, 1, 1), new DateOnly(2024, 1, 1)));

        // when
        var result = () => testCandidate.WithAchievementDate(achievementDate, programme, null, _dateTimeProvider);

        // then
        result.Should().NotThrow();
    }

    [Fact]
    public void ShouldThrowException_WhenAchievementDateIsAfterFundingDateAndGrantAmountIsMoreThenZero()
    {
        // given
        GetDateTimeProviderMock();
        var achievementDate = new DateDetails("01", "01", "2024");
        var testCandidate = MilestoneClaimTestBuilder.New()
            .WithType(MilestoneType.Acquisition)
            .WithGrantApportioned(10000, 40)
            .Build();
        var programme = CreateProgramme(fundingDates: new DateRange(new DateOnly(2022, 1, 1), new DateOnly(2023, 1, 1)));

        // when
        var result = () => testCandidate.WithAchievementDate(achievementDate, programme, null, _dateTimeProvider);

        // then
        result.Should()
            .Throw<DomainValidationException>()
            .WithSingleError(ValidationErrorMessage.DatesOutsideTheProgramme);
    }

    [Fact]
    public void ShouldNotThrowException_WhenAchievementDateIsAfterFundingDateAndGrantAmountIsZero()
    {
        // given
        GetDateTimeProviderMock();
        var achievementDate = new DateDetails("01", "01", "2024");
        var testCandidate = MilestoneClaimTestBuilder.New()
            .WithType(MilestoneType.Acquisition)
            .WithGrantApportioned(0, 40)
            .Build();
        var programme = CreateProgramme(fundingDates: new DateRange(new DateOnly(2022, 1, 1), new DateOnly(2023, 1, 1)));

        // when
        var result = () => testCandidate.WithAchievementDate(achievementDate, programme, null, _dateTimeProvider);

        // then
        result.Should().NotThrow();
    }

    [Fact]
    public void ShouldThrowException_WhenMilestoneTypeIsStartOnSiteAndAchievementDateIsBeforeStartOnSiteDate()
    {
        // given
        GetDateTimeProviderMock();
        var achievementDate = new DateDetails("01", "01", "2021");
        var testCandidate = MilestoneClaimTestBuilder.New()
            .WithType(MilestoneType.StartOnSite)
            .Build();
        var programme = CreateProgramme(startOnSiteDates: new DateRange(new DateOnly(2022, 1, 1), new DateOnly(2024, 1, 1)));

        // when
        var result = () => testCandidate.WithAchievementDate(achievementDate, programme, null, _dateTimeProvider);

        // then
        result.Should()
            .Throw<DomainValidationException>()
            .WithSingleError(ValidationErrorMessage.DatesOutsideTheProgramme);
    }

    [Fact]
    public void ShouldNotThrowException_WhenMilestoneTypeIsCompletionAndAchievementDateIsBeforeStartOnSiteDate()
    {
        // given
        GetDateTimeProviderMock();
        var achievementDate = new DateDetails("01", "01", "2021");
        var testCandidate = MilestoneClaimTestBuilder.New()
            .WithType(MilestoneType.Completion)
            .Build();
        var programme = CreateProgramme(startOnSiteDates: new DateRange(new DateOnly(2022, 1, 1), new DateOnly(2024, 1, 1)));

        // when
        var result = () => testCandidate.WithAchievementDate(achievementDate, programme, null, _dateTimeProvider);

        // then
        result.Should().NotThrow();
    }

    [Fact]
    public void ShouldThrowException_WhenMilestoneTypeIsStartOnSiteAndAchievementDateIsAfterStartOnSiteDate()
    {
        // given
        GetDateTimeProviderMock();
        var achievementDate = new DateDetails("01", "01", "2024");
        var testCandidate = MilestoneClaimTestBuilder.New()
            .WithType(MilestoneType.StartOnSite)
            .Build();
        var programme = CreateProgramme(startOnSiteDates: new DateRange(new DateOnly(2022, 1, 1), new DateOnly(2023, 1, 1)));

        // when
        var result = () => testCandidate.WithAchievementDate(achievementDate, programme, null, _dateTimeProvider);

        // then
        result.Should()
            .Throw<DomainValidationException>()
            .WithSingleError(ValidationErrorMessage.DatesOutsideTheProgramme);
    }

    [Fact]
    public void ShouldNotThrowException_WhenMilestoneTypeIsCompletionAndAchievementDateIsAfterStartOnSiteDate()
    {
        // given
        GetDateTimeProviderMock();
        var achievementDate = new DateDetails("01", "01", "2024");
        var testCandidate = MilestoneClaimTestBuilder.New()
            .WithType(MilestoneType.Completion)
            .Build();
        var programme = CreateProgramme(startOnSiteDates: new DateRange(new DateOnly(2022, 1, 1), new DateOnly(2023, 1, 1)));

        // when
        var result = () => testCandidate.WithAchievementDate(achievementDate, programme, null, _dateTimeProvider);

        // then
        result.Should().NotThrow();
    }

    [Fact]
    public void ShouldThrowException_WhenMilestoneTypeIsCompletionAndAchievementDateIsBeforeCompletionDate()
    {
        // given
        GetDateTimeProviderMock();
        var achievementDate = new DateDetails("01", "01", "2021");
        var testCandidate = MilestoneClaimTestBuilder.New()
            .WithType(MilestoneType.Completion)
            .Build();
        var programme = CreateProgramme(completionDates: new DateRange(new DateOnly(2022, 1, 1), new DateOnly(2024, 1, 1)));

        // when
        var result = () => testCandidate.WithAchievementDate(achievementDate, programme, null, _dateTimeProvider);

        // then
        result.Should()
            .Throw<DomainValidationException>()
            .WithSingleError(ValidationErrorMessage.DatesOutsideTheProgramme);
    }

    [Fact]
    public void ShouldNotThrowException_WhenMilestoneTypeIsStartOnSiteAndAchievementDateIsBeforeCompletionDate()
    {
        // given
        GetDateTimeProviderMock();
        var achievementDate = new DateDetails("01", "01", "2021");
        var testCandidate = MilestoneClaimTestBuilder.New()
            .WithType(MilestoneType.StartOnSite)
            .Build();
        var programme = CreateProgramme(completionDates: new DateRange(new DateOnly(2022, 1, 1), new DateOnly(2024, 1, 1)));

        // when
        var result = () => testCandidate.WithAchievementDate(achievementDate, programme, null, _dateTimeProvider);

        // then
        result.Should().NotThrow();
    }

    [Fact]
    public void ShouldThrowException_WhenMilestoneTypeIsCompletionAndAchievementDateIsAfterCompletionDate()
    {
        // given
        GetDateTimeProviderMock();
        var achievementDate = new DateDetails("01", "01", "2024");
        var testCandidate = MilestoneClaimTestBuilder.New()
            .WithType(MilestoneType.Completion)
            .Build();
        var programme = CreateProgramme(completionDates: new DateRange(new DateOnly(2022, 1, 1), new DateOnly(2023, 1, 1)));

        // when
        var result = () => testCandidate.WithAchievementDate(achievementDate, programme, null, _dateTimeProvider);

        // then
        result.Should()
            .Throw<DomainValidationException>()
            .WithSingleError(ValidationErrorMessage.DatesOutsideTheProgramme);
    }

    [Fact]
    public void ShouldNotThrowException_WhenMilestoneTypeIsStartOnSiteAndAchievementDateIsAfterCompletionDate()
    {
        // given
        GetDateTimeProviderMock();
        var achievementDate = new DateDetails("01", "01", "2024");
        var testCandidate = MilestoneClaimTestBuilder.New()
            .WithType(MilestoneType.StartOnSite)
            .Build();
        var programme = CreateProgramme(completionDates: new DateRange(new DateOnly(2022, 1, 1), new DateOnly(2023, 1, 1)));

        // when
        var result = () => testCandidate.WithAchievementDate(achievementDate, programme, null, _dateTimeProvider);

        // then
        result.Should().NotThrow();
    }

    private AhpProgramme CreateProgramme(
        DateRange? programmeDate = null,
        DateRange? fundingDates = null,
        DateRange? startOnSiteDates = null,
        DateRange? completionDates = null)
    {
        return new AhpProgramme(
            ProgrammeId.From("d5fe3baa-eeae-ee11-a569-0022480041cf"),
            "Ahp",
            "ProgrammeType Ahp",
            true,
            ProgrammeType.Ahp,
            programmeDate ?? new DateRange(DateOnly.MinValue, DateOnly.MaxValue),
            fundingDates ?? new DateRange(DateOnly.MinValue, DateOnly.MaxValue),
            startOnSiteDates ?? new DateRange(DateOnly.MinValue, DateOnly.MaxValue),
            completionDates ?? new DateRange(DateOnly.MinValue, DateOnly.MaxValue));
    }

    private void GetDateTimeProviderMock()
    {
        Mock.Get(_dateTimeProvider)
            .Setup(x => x.Now).Returns(new DateTime(2024, 07, 07, 12, 25, 0, DateTimeKind.Unspecified));
    }
}
