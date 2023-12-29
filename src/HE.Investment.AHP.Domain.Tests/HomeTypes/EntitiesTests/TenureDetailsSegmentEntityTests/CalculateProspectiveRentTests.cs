using FluentAssertions;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investment.AHP.Domain.Tests.HomeTypes.TestDataBuilders;
using HE.Investments.Common.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.HomeTypes.EntitiesTests.TenureDetailsSegmentEntityTests;

public class CalculateProspectiveRentTests
{
    [Theory]
    [InlineData(10, 8, 80)]
    [InlineData(20, 5, 25)]
    [InlineData(5, 20, 400)]
    [InlineData(1450, 1200, 83)]
    [InlineData(980, 1250, 128)]
    public void ShouldReturnCalculatedResult_WhenMarketRentAndProspectiveRentAreNumbers(decimal marketRent, decimal prospectiveRent, decimal expectedResult)
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
    [InlineData("input", "8")]
    [InlineData("10", "input")]
    [InlineData("10", "")]
    [InlineData("", "10")]
    [InlineData("", "")]
    public void ShouldReturnZero_WhenMarketRentOrProspectiveRentIsNotANumber(string marketRent, string prospectiveRent)
    {
        // given
        var tenureDetails = new TenureDetailsTestDataBuilder().Build();

        // when
        Action action = () => _ = tenureDetails.CalculateProspectiveRent(
            new MarketRent(marketRent, true),
            new ProspectiveRent(prospectiveRent, true));

        // then
        action.Should().Throw<DomainValidationException>();
    }
}
