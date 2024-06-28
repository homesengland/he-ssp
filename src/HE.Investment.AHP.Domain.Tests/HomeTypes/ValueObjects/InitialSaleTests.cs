using FluentAssertions;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.HomeTypes.ValueObjects;

public class InitialSaleTests
{
    [Theory]
    [InlineData("9")]
    [InlineData("76")]
    public void ShouldThrowValueMustBeBetweenError_WhenValueIsOutsideValidRanges(string value)
    {
        // given & when
        var create = () => new InitialSale(value);

        // then
        create.Should()
            .Throw<DomainValidationException>()
            .Which.Message.Should()
            .Be("The assumed average first tranche sale percentage must be must be a number between 10 and 75");
    }

    [Theory]
    [InlineData("10", 0.1)]
    [InlineData("75", 0.75)]
    public void ShouldCreateObject_WhenValueIsWithinValidRange(string value, decimal expectedValue)
    {
        // given & when
        var result = new InitialSale(value);

        // then
        result.Value.Should().Be(expectedValue);
    }
}
