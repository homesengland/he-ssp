using FluentAssertions;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investment.AHP.Domain.Tests.HomeTypes.TestDataBuilders;

namespace HE.Investment.AHP.Domain.Tests.HomeTypes.Entities.TenureDetailsSegmentEntityTests;

public class ChangeRentAsPercentageOfMarketRentTests
{
    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(1000, 500, 0.5)]
    [InlineData(9999.99, 0, 0)]
    [InlineData(0, 9999.99, 0)]
    [InlineData(9999.99, 9999.99, 1)]
    public void ShouldReturnCalculatedResult_WhenInputsAreProvided(decimal marketRentPerWeek, decimal rentPerWeek, decimal expectedResult)
    {
        // given
        var marketRentVO = new MarketRentPerWeek(marketRentPerWeek);
        var prospectiveRentVO = new RentPerWeek(rentPerWeek);

        var tenureDetails = new TenureDetailsTestDataBuilder()
            .WithMarketRent(marketRentVO)
            .WithProspectiveRent(prospectiveRentVO)
            .Build();

        // when
        tenureDetails.ChangeRentAsPercentageOfMarketRent();

        // then
        tenureDetails.ProspectiveRentAsPercentageOfMarketRent.Should().NotBeNull();
        tenureDetails.ProspectiveRentAsPercentageOfMarketRent!.Value.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("10", null)]
    [InlineData(null, "10")]
    public void ShouldReturnNull_WhenAnyValueIsNotProvided(string? marketRentPerWeek, string? rentPerWeek)
    {
        // given
        var tenureDetails = new TenureDetailsTestDataBuilder().Build();

        // when
        var result = tenureDetails.CalculateRentAsPercentageOfMarketRent(
            marketRentPerWeek == null ? null : new MarketRentPerWeek(marketRentPerWeek, true),
            rentPerWeek == null ? null : new RentPerWeek(rentPerWeek, true));

        // then
        result.Should().BeNull();
    }
}
