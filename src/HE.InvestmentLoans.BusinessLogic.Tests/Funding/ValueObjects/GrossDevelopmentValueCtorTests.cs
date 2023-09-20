using System.Globalization;
using HE.InvestmentLoans.BusinessLogic.Tests.Assertions;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Contract.Funding.ValueObjects;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Funding.ValueObjects;
public class GrossDevelopmentValueCtorTests
{
    [Theory]
    [InlineData("0")]
    [InlineData("1")]
    [InlineData("1.42")]
    [InlineData("753.7")]
    [InlineData("99999")]
    public void ShouldCreateGrossDevelopmentValue(string grossDevelopmentValuesAsString)
    {
        // given
        _ = decimal.TryParse(grossDevelopmentValuesAsString, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var parsedValue);

        // given
        var action = () => GrossDevelopmentValue.FromString(grossDevelopmentValuesAsString);

        // then
        action.Should().NotThrow<DomainValidationException>();
        action().Value.Should().Be(parsedValue);
    }

    [Theory]
    [InlineData("dd")]
    [InlineData("1.234567")]
    [InlineData("-2134")]
    public void ShouldThrowDomainValidationException_WhenStringIsNotPositiveDecimalWithMaxTwoSpacesAfterDot(string grossDevelopmentValuesAsString)
    {
        // given && when
        var action = () => GrossDevelopmentValue.FromString(grossDevelopmentValuesAsString);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.EstimatedPoundInput("GDV"));
    }
}
