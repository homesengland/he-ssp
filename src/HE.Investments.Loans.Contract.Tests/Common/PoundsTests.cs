using System.Globalization;
using FluentAssertions;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.Contract.Common;
using Xunit;

namespace HE.Investments.Loans.Contract.Tests.Common;

public class PoundsTests
{
    [Theory]
    [InlineData("0")]
    [InlineData("1")]
    [InlineData("1.42")]
    [InlineData("753.7")]
    [InlineData("99999")]
    public void ShouldCreateEstimatedTotalCosts(string estimatedTotalCostsAsString)
    {
        // given
        _ = decimal.TryParse(estimatedTotalCostsAsString, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var parsedValue);

        // given
        var action = () => Pounds.FromString(estimatedTotalCostsAsString);

        // then
        action.Should().NotThrow<DomainValidationException>();
        action().Value.Should().Be(parsedValue);
    }

    [Theory]
    [InlineData("dd")]
    [InlineData("1.234567")]
    [InlineData("-2134")]
    public void ShouldThrowDomainValidationException_WhenStringIsNotPositiveDecimalWithMaxTwoSpacesAfterDot(string estimatedTotalCostsAsString)
    {
        // given && when
        var action = () => Pounds.FromString(estimatedTotalCostsAsString);

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == GenericValidationError.InvalidPoundsValue);
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenValueIsNull()
    {
        // given && when
        var action = () => Pounds.FromString(null);

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == GenericValidationError.InvalidPoundsValue);
    }
}
