using FluentAssertions;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;

namespace HE.Investment.AHP.Domain.Tests.FinancialDetails.Entities.PublicGrantsTests;

public class CalculateTotalTests
{
    [Fact]
    public void ShouldCalculateTotal_WhenAllValuesAreProvided()
    {
        // given
        var sut = new PublicGrants(
            new PublicGrantValue(PublicGrantFields.CountyCouncilGrants, 1),
            new PublicGrantValue(PublicGrantFields.DhscExtraCareGrants, 2),
            new PublicGrantValue(PublicGrantFields.LocalAuthorityGrants, 3),
            new PublicGrantValue(PublicGrantFields.SocialServicesGrants, 4),
            new PublicGrantValue(PublicGrantFields.HealthRelatedGrants, 5),
            new PublicGrantValue(PublicGrantFields.LotteryGrants, 6),
            new PublicGrantValue(PublicGrantFields.OtherPublicBodiesGrants, 7));

        // when
        var result = sut.CalculateTotal();

        // then
        result.Should().Be(28);
    }

    [Fact]
    public void ShouldCalculateTotal_WhenSomeValuesAreProvided()
    {
        // given
        var sut = new PublicGrants(
            new PublicGrantValue(PublicGrantFields.CountyCouncilGrants, 1),
            null,
            new PublicGrantValue(PublicGrantFields.LocalAuthorityGrants, 3),
            null,
            new PublicGrantValue(PublicGrantFields.HealthRelatedGrants, 5),
            null,
            new PublicGrantValue(PublicGrantFields.OtherPublicBodiesGrants, 7));

        // when
        var result = sut.CalculateTotal();

        // then
        result.Should().Be(16);
    }

    [Fact]
    public void ShouldCalculateTotal_WhenNoValuesAreProvided()
    {
        // given
        var sut = new PublicGrants(
            null,
            null,
            null,
            null,
            null,
            null,
            null);

        // when
        var result = sut.CalculateTotal();

        // then
        result.Should().Be(0);
    }

    [Fact]
    public void ShouldCalculateTotal_WhenAllValuesAreZero()
    {
        // given
        var sut = new PublicGrants(
            new PublicGrantValue(PublicGrantFields.CountyCouncilGrants, 0),
            new PublicGrantValue(PublicGrantFields.DhscExtraCareGrants, 0),
            new PublicGrantValue(PublicGrantFields.LocalAuthorityGrants, 0),
            new PublicGrantValue(PublicGrantFields.SocialServicesGrants, 0),
            new PublicGrantValue(PublicGrantFields.HealthRelatedGrants, 0),
            new PublicGrantValue(PublicGrantFields.LotteryGrants, 0),
            new PublicGrantValue(PublicGrantFields.OtherPublicBodiesGrants, 0));

        // when
        var result = sut.CalculateTotal();

        // then
        result.Should().Be(0);
    }

    [Fact]
    public void ShouldCalculateTotal_WhenAllValuesHave9Digits()
    {
        // given
        var sut = new PublicGrants(
            new PublicGrantValue(PublicGrantFields.CountyCouncilGrants, "123456789"),
            new PublicGrantValue(PublicGrantFields.DhscExtraCareGrants, "123456789"),
            new PublicGrantValue(PublicGrantFields.LocalAuthorityGrants, "123456789"),
            new PublicGrantValue(PublicGrantFields.SocialServicesGrants, "123456789"),
            new PublicGrantValue(PublicGrantFields.HealthRelatedGrants, "123456789"),
            new PublicGrantValue(PublicGrantFields.LotteryGrants, "123456789"),
            new PublicGrantValue(PublicGrantFields.OtherPublicBodiesGrants, "123456789"));

        // when
        var result = sut.CalculateTotal();

        // then
        result.Should().Be(864197523);
    }
}
