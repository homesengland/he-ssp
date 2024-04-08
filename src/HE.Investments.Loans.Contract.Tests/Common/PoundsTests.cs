using System.Globalization;
using FluentAssertions;
using HE.Investments.Common.Contract.Exceptions;
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
    public void ShouldCreatePounds_WhenValueIsString(string value)
    {
        // given && when
        var action = () => Pounds.FromString(value, nameof(Pounds), "pounds value");

        // then
        action.Should().NotThrow<DomainValidationException>();
        action().Value.Should().Be(decimal.Parse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(1.42)]
    [InlineData(753.7)]
    [InlineData(99999)]
    public void ShouldCreatePounds_WhenValueIsDecimal(decimal value)
    {
        // given && when
        var action = () => Pounds.New(value, nameof(Pounds), "pounds value");

        // then
        action.Should().NotThrow<DomainValidationException>();
        action().Value.Should().Be(value);
    }

    [Theory]
    [InlineData("9912315621456")]
    [InlineData("792281625142643375935439503352")]
    public void ShouldThrowDomainValidationException_WhenStringValueIsTooHigh(string value)
    {
        // given && when
        var action = () => Pounds.FromString(value, nameof(Pounds), "pounds value");

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == ValidationErrorMessage.MustProvideTheLowerNumber("pounds value", 999999999));
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenDecimalNumberIsTooLow()
    {
        // given
        var value = -12.22M;

        // when
        var action = () => Pounds.New(value, nameof(Pounds), "pounds value");

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == ValidationErrorMessage.MustProvideTheHigherNumber("pounds value", 0));
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenDecimalNumberIsTooHigh()
    {
        // given
        var value = 9999999999999M;

        // when
        var action = () => Pounds.New(value, nameof(Pounds), "pounds value");

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == ValidationErrorMessage.MustProvideTheLowerNumber("pounds value", 999999999));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("abc")]
    [InlineData("1.234")]
    [InlineData("12.34.56")]
    public void ShouldThrowDomainValidationException_WhenValueIsInvalid(string value)
    {
        // given && when
        var action = () => Pounds.FromString(value, nameof(Pounds), "pounds value");

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == GenericValidationError.InvalidPoundsValue);
    }
}
