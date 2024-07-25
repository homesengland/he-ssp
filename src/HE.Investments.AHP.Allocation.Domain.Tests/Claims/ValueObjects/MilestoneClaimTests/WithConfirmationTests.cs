using FluentAssertions;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Tests.TestObjectBuilders;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Tests.FluentAssertions;
using Xunit;
using MilestoneStatus = HE.Investments.AHP.Allocation.Domain.Claims.Enums.MilestoneStatus;

namespace HE.Investments.AHP.Allocation.Domain.Tests.Claims.ValueObjects.MilestoneClaimTests;

public class WithConfirmationTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(null)]
    public void ShouldThrowException_WhenConfirmationIsNotProvided(bool? isConfirmed)
    {
        // given
        var testCandidate = MilestoneClaimTestBuilder.Draft()
            .WithType(MilestoneType.Acquisition)
            .Build();

        // when
        var withConfirmation = () => testCandidate.WithConfirmation(isConfirmed);

        // then
        withConfirmation.Should()
            .Throw<DomainValidationException>()
            .WithSingleError("Confirm the declaration to continue");
    }

    [Fact]
    public void ShouldCreateNewMilestoneClaim_WhenClaimIsConfirmed()
    {
        // given
        var forecastClaimDate = new DateTime(2025, 10, 11, 0, 0, 0, DateTimeKind.Local);
        var testCandidate = MilestoneClaimTestBuilder.Draft()
            .WithType(MilestoneType.Acquisition)
            .WithForecastClaimDate(forecastClaimDate)
            .WithCostsIncurred(true)
            .Build();

        // when
        var result = testCandidate.WithConfirmation(true);

        // then
        result.Should().NotBe(testCandidate);
        result.Type.Should().Be(MilestoneType.Acquisition);
        result.Status.Should().Be(MilestoneStatus.Draft);
        result.ClaimDate.ForecastClaimDate.Should().Be(forecastClaimDate);
        result.CostsIncurred.Should().BeTrue();
        result.IsConfirmed.Should().BeTrue();
    }

    [Fact]
    public void ShouldThrowException_WhenMilestoneClaimIsSubmitted()
    {
        // given
        var testCandidate = MilestoneClaimTestBuilder.Draft()
            .WithType(MilestoneType.Acquisition)
            .Submitted()
            .Build();

        // when
        var withConfirmation = () => testCandidate.WithConfirmation(true);

        // then
        withConfirmation.Should()
            .Throw<DomainValidationException>()
            .WithSingleError("Providing confirmation is not allowed for Submitted Claim");
    }
}
