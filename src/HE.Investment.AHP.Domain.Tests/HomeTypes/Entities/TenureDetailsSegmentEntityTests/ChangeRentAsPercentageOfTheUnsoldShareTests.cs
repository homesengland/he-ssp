using FluentAssertions;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investment.AHP.Domain.Tests.HomeTypes.TestDataBuilders;

namespace HE.Investment.AHP.Domain.Tests.HomeTypes.Entities.TenureDetailsSegmentEntityTests;

public class ChangeRentAsPercentageOfTheUnsoldShareTests
{
    [Theory]
    [InlineData(1, 0, 0.1, 0)]
    [InlineData(99_999_999, 9999.99, 0.75, 0.0207)]
    [InlineData(0, 1000, 0.50, 0)]
    public void ShouldReturnCalculatedResult_WhenInputsAreProvided(int marketValue, decimal rentPerWeek, decimal initialSale, decimal expectedResult)
    {
        // given
        var marketValueVO = new MarketValue(marketValue);
        var prospectiveRentVO = new RentPerWeek(rentPerWeek);
        var initialSaleVO = new InitialSale(initialSale);

        var tenureDetails = new TenureDetailsTestDataBuilder()
            .WithMarketValue(marketValueVO)
            .WithInitialSale(initialSaleVO)
            .WithProspectiveRent(prospectiveRentVO)
            .Build();

        // when
        tenureDetails.ChangeRentAsPercentageOfTheUnsoldShare();

        // then
        tenureDetails.RentAsPercentageOfTheUnsoldShare.Should().NotBeNull();
        tenureDetails.RentAsPercentageOfTheUnsoldShare!.Value.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("10", "10", null)]
    [InlineData("10", null, "10")]
    [InlineData(null, "10", "10")]
    public void ShouldReturnNull_WhenAnyValueIsNotProvided(string? marketValue, string? rentPerWeek, string? initialSale)
    {
        // given
        var tenureDetails = new TenureDetailsTestDataBuilder().Build();

        // when
        var result = tenureDetails.CalculateRentAsPercentageOfTheUnsoldShare(
            marketValue == null ? null : new MarketValue(marketValue, true),
            rentPerWeek == null ? null : new RentPerWeek(rentPerWeek, true),
            initialSale == null ? null : new InitialSale(initialSale, true));

        // then
        result.Should().BeNull();
    }
}
