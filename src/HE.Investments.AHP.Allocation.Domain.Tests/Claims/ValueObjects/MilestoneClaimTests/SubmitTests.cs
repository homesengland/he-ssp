using FluentAssertions;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;
using HE.Investments.AHP.Allocation.Domain.Tests.TestObjectBuilders;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Utils;
using Moq;
using Xunit;
using MilestoneStatus = HE.Investments.AHP.Allocation.Domain.Claims.Enums.MilestoneStatus;

namespace HE.Investments.AHP.Allocation.Domain.Tests.Claims.ValueObjects.MilestoneClaimTests;

public class SubmitTests
{
    private readonly IDateTimeProvider _dateTimeProvider = Mock.Of<IDateTimeProvider>();

    [Fact]
    public void ShouldReturnSubmittedClaim_WhenClaimIsInDraftStatusAndAllAnswersAreProvided()
    {
        // given
        GetDateTimeProviderMock();
        var testCandidate = MilestoneClaimTestBuilder.Draft()
            .WithType(MilestoneType.Acquisition)
            .WithMilestoneAchievedDate(new AchievementDate(DateTime.Today))
            .WithCostsIncurred(true)
            .WithConfirmation(true)
            .Build();

        // when
        var result = testCandidate.Submit(_dateTimeProvider.Now);

        // then
        result.Should().BeOfType<SubmittedMilestoneClaim>();
        result.Type.Should().Be(MilestoneType.Acquisition);
        result.Status.Should().Be(MilestoneStatus.Submitted);
        result.ClaimDate.ForecastClaimDate.Should().Be(testCandidate.ClaimDate.ForecastClaimDate);
        result.ClaimDate.AchievementDate.Should().Be(testCandidate.ClaimDate.AchievementDate);
        result.CostsIncurred.Should().Be(testCandidate.CostsIncurred);
        result.IsConfirmed.Should().Be(testCandidate.IsConfirmed);
        result.ClaimDate.SubmissionDate.Should().Be(_dateTimeProvider.Now);
    }

    [Fact]
    public void ShouldThrowException_WhenClaimIsSubmitted()
    {
        // given
        var testCandidate = MilestoneClaimTestBuilder.Draft().Submitted().Build();

        // when
        var submit = () => testCandidate.Submit(_dateTimeProvider.Now);

        // then
        submit.Should().Throw<DomainValidationException>().WithMessage("Submission is not allowed for Submitted Claim");
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData(null, true)]
    [InlineData(null, false)]
    [InlineData(true, null)]
    [InlineData(false, null)]
    public void ShouldThrowException_WhenCostIncurredOrConfirmationAreNotProvided(bool? costsIncurred, bool? isConfirmed)
    {
        // given
        var testCandidate = MilestoneClaimTestBuilder.Draft()
            .WithType(MilestoneType.Acquisition)
            .WithMilestoneAchievedDate(new AchievementDate(DateTime.Today))
            .WithCostsIncurred(costsIncurred)
            .WithConfirmation(isConfirmed)
            .Build();

        // when
        var submit = () => testCandidate.Submit(_dateTimeProvider.Now);

        // then
        submit.Should().Throw<DomainValidationException>().WithMessage("To submit, you have to provide all answers");
    }

    [Fact]
    public void ShouldThrowException_WhenAchievementDateIsNotProvided()
    {
        // given
        var testCandidate = MilestoneClaimTestBuilder.Draft()
            .WithType(MilestoneType.Acquisition)
            .WithMilestoneAchievedDate(null)
            .WithCostsIncurred(true)
            .WithConfirmation(true)
            .Build();

        // when
        var submit = () => testCandidate.Submit(_dateTimeProvider.Now);

        // then
        submit.Should().Throw<DomainValidationException>().WithMessage("To submit, you have to provide all answers");
    }

    private void GetDateTimeProviderMock()
    {
        Mock.Get(_dateTimeProvider)
            .Setup(x => x.Now)
            .Returns(new DateTime(2024, 07, 07, 12, 25, 0, DateTimeKind.Unspecified));
    }
}
