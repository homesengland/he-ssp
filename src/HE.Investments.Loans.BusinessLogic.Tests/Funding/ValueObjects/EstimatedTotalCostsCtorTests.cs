using System.Globalization;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.BusinessLogic.Tests.Assertions;
using HE.Investments.Loans.Contract.Funding.ValueObjects;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Funding.ValueObjects;
public class EstimatedTotalCostsCtorTests
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
        var action = () => EstimatedTotalCosts.FromString(estimatedTotalCostsAsString);

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
        var action = () => EstimatedTotalCosts.FromString(estimatedTotalCostsAsString);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.EstimatedPoundInput("total cost"));
    }
}
