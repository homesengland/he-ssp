using FluentAssertions;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investment.AHP.Domain.Tests.HomeTypes.TestDataBuilders;
using HE.Investments.Common.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.HomeTypes.EntitiesTests.TenureDetailsSegmentEntityTests;

public class CalculateProspectiveRentAsPercentageOfTheUnsoldShareTests
{
    [Theory]
    [InlineData(1000, 800, 10, 46.22)]
    [InlineData(2000, 20, 35, 0.8)]
    [InlineData(511, 90, 12, 10.41)]
    [InlineData(14500, 945, 60, 8.47)]
    [InlineData(9800, 5000, 75, 106.12)]
    public void ShouldReturnCalculatedResult_WhenProvidedInputsAreNumber(int marketValue, decimal prospectiveRent, int initialSale, decimal expectedResult)
    {
        // given
        var marketValueVO = new MarketValue(marketValue);
        var prospectiveRentVO = new ProspectiveRent(prospectiveRent);
        var initialSaleVO = new InitialSale(initialSale);

        var tenureDetails = new TenureDetailsTestDataBuilder()
            .WithMarketValue(marketValueVO)
            .WithInitialSale(initialSaleVO)
            .WithProspectiveRent(prospectiveRentVO)
            .Build();

        // when
        tenureDetails.ChangeProspectiveRentAsPercentageOfTheUnsoldShare();

        // then
        tenureDetails.RentAsPercentageOfTheUnsoldShare.Should().NotBeNull();
        tenureDetails.RentAsPercentageOfTheUnsoldShare!.Value.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("input", "8", "10")]
    [InlineData("10", "input", "15")]
    [InlineData("", "10", "")]
    [InlineData("", "", "")]
    public void ShouldReturnZero_WhenProvidedInputIsNotANumber(string marketValue, string prospectiveRent, string initialSale)
    {
        // given
        var tenureDetails = new TenureDetailsTestDataBuilder().Build();

        // when
        Action action = () => _ = tenureDetails.CalculateProspectiveRentAsPercentageOfTheUnsoldShare(
            new MarketValue(marketValue, true),
            new ProspectiveRent(prospectiveRent, true),
            new InitialSale(initialSale, true));

        // then
        action.Should().Throw<DomainValidationException>();
    }
}
