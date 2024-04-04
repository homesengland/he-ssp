using System.Globalization;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.BusinessLogic.Tests.Assertions;
using HE.Investments.Loans.Contract.Funding.ValueObjects;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Funding.ValueObjects;
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

    [Fact]
    public void ShouldThrowDomainValidationException_WhenValueIsNull()
    {
        // given && when
        var action = () => GrossDevelopmentValue.FromString(string.Empty);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.EstimatedPoundInput("GDV"));
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenValueIsGreaterThanMaxValue()
    {
        // given && when
        var action = () => GrossDevelopmentValue.FromString("1000000000");

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.MustProvideTheLowerNumber("Gross Development Value", 999999999));
    }

    [Fact]
    public void ShouldCreateGrossDevelopmentValue_WhenValueIsEqualToMaxValue()
    {
        // given
        var action = () => GrossDevelopmentValue.FromString("999999999");

        // then
        action.Should().NotThrow<DomainValidationException>();
        action().Value.Should().Be(999999999);
    }

    [Fact]
    public void ShouldCreateGrossDevelopmentValue_WhenValueIsEqualToMinValue()
    {
        // given
        var action = () => GrossDevelopmentValue.FromString("0");

        // then
        action.Should().NotThrow<DomainValidationException>();
        action().Value.Should().Be(0);
    }
}
