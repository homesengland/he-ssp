using FluentAssertions;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Tests.TestObjectBuilders;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Tests.FluentAssertions;
using Xunit;
using MilestoneStatus = HE.Investments.AHP.Allocation.Domain.Claims.Enums.MilestoneStatus;

namespace HE.Investments.AHP.Allocation.Domain.Tests.Claims.ValueObjects.MilestoneClaimTests;

public class WithCostsIncurredTests
{
    [Fact]
    public void ShouldThrowException_WhenCostsIncurredAreNotProvided()
    {
        // given
        var testCandidate = MilestoneClaimTestBuilder.Draft()
            .WithType(MilestoneType.Acquisition)
            .Build();

        // when
        var withCostsIncurred = () => testCandidate.WithCostsIncurred(null);

        // then
        withCostsIncurred.Should()
            .Throw<DomainValidationException>()
            .WithSingleError("Select yes if you have incurred costs and made payments to at least the level of the grant");
    }

    [Theory]
    [InlineData(MilestoneType.StartOnSite)]
    [InlineData(MilestoneType.Completion)]
    public void ShouldThrowException_WhenCostsIncurredAreProvidedForMilestone(MilestoneType milestoneType)
    {
        // given
        var testCandidate = MilestoneClaimTestBuilder.Draft()
            .WithType(milestoneType)
            .Build();

        // when
        var withCostsIncurred = () => testCandidate.WithCostsIncurred(true);

        // then
        withCostsIncurred.Should()
            .Throw<DomainValidationException>()
            .WithSingleError("Costs incurred can be provided only for Acquisition milestone");
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShouldCreateNewMilestoneClaim_WhenCostsIncurredAreProvidedForAcquisitionMilestone(bool costsIncurred)
    {
        // given
        var forecastClaimDate = new DateTime(2025, 10, 11, 0, 0, 0, DateTimeKind.Local);
        var testCandidate = MilestoneClaimTestBuilder.Draft()
            .WithType(MilestoneType.Acquisition)
            .WithForecastClaimDate(forecastClaimDate)
            .Build();

        // when
        var result = testCandidate.WithCostsIncurred(costsIncurred);

        // then
        result.Should().NotBe(testCandidate);
        result.Type.Should().Be(MilestoneType.Acquisition);
        result.Status.Should().Be(MilestoneStatus.Draft);
        result.ClaimDate.ForecastClaimDate.Should().Be(forecastClaimDate);
        result.CostsIncurred.Should().Be(costsIncurred);
        result.IsConfirmed.Should().BeNull();
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
        var withConfirmation = () => testCandidate.WithCostsIncurred(true);

        // then
        withConfirmation.Should()
            .Throw<DomainValidationException>()
            .WithSingleError("Providing costs incurred is not allowed for Submitted Claim");
    }
}
