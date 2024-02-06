using FluentAssertions;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investment.AHP.Domain.Tests.HomeTypes.TestDataBuilders;

namespace HE.Investment.AHP.Domain.Tests.HomeTypes.EntitiesTests.TenureDetailsSegmentEntityTests;

public class CalculateProspectiveRentTests
{
    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(1000, 500, 50)]
    [InlineData(9999.99, 0, 0)]
    [InlineData(0, 9999.99, 0)]
    [InlineData(9999.99, 9999.99, 100)]
    public void ShouldReturnCalculatedResult_WhenInputsAreProvided(decimal marketRent, decimal prospectiveRent, decimal expectedResult)
    {
        // given
        var marketRentVO = new MarketRent(marketRent);
        var prospectiveRentVO = new ProspectiveRent(prospectiveRent);

        var tenureDetails = new TenureDetailsTestDataBuilder()
            .WithMarketRent(marketRentVO)
            .WithProspectiveRent(prospectiveRentVO)
            .Build();

        // when
        tenureDetails.ChangeProspectiveRentAsPercentageOfMarketRent();

        // then
        tenureDetails.ProspectiveRentAsPercentageOfMarketRent.Should().NotBeNull();
        tenureDetails.ProspectiveRentAsPercentageOfMarketRent!.Value.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("10", null)]
    [InlineData(null, "10")]
    public void ShouldReturnNull_WhenAnyValueIsNotProvided(string? marketRent, string? prospectiveRent)
    {
        // given
        var tenureDetails = new TenureDetailsTestDataBuilder().Build();

        // when
        var result = tenureDetails.CalculateProspectiveRent(
            marketRent == null ? null : new MarketRent(marketRent, true),
            prospectiveRent == null ? null : new ProspectiveRent(prospectiveRent, true));

        // then
        result.Should().BeNull();
    }
}
