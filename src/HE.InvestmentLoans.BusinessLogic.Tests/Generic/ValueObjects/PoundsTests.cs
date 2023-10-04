using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.Generic;
using HE.InvestmentLoans.BusinessLogic.Tests.Assertions;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Contract.Funding.ValueObjects;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Generic.ValueObjects;
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
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(GenericValidationError.InvalidPoundsValue);
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenValueIsNull()
    {
        // given && when
        var action = () => Pounds.FromString(null);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(GenericValidationError.InvalidPoundsValue);
    }
}
