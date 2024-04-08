using System.Globalization;
using FluentAssertions;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.Contract.Common;
using Xunit;

namespace HE.Investments.Loans.Contract.Tests.Common;

public class MoneyValueObjectTests
{
    [Theory]
    [InlineData("1245367")]
    [InlineData("5.98")]
    [InlineData("1356.7")]
    public void ShouldCreateMoneyValueObject_WhenValueIsString(string value)
    {
        // given && when
        var action = () => TestMoneyValueObject.FromString(value);

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
    public void ShouldCreateMoneyValueObject_WhenValueIsDecimal(decimal value)
    {
        // given && when
        var action = () => TestMoneyValueObject.FromDecimal(value);

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
        var action = () => TestMoneyValueObject.FromString(value);

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == ValidationErrorMessage.MustProvideTheLowerNumber("money value", 999999999));
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenDecimalNumberIsTooLow()
    {
        // given
        var value = -12.22M;

        // when
        var action = () => TestMoneyValueObject.FromDecimal(value);

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == ValidationErrorMessage.MustProvideTheHigherNumber("money value", 0));
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenDecimalNumberIsTooHigh()
    {
        // given
        var value = 9999999999999M;

        // when
        var action = () => TestMoneyValueObject.FromDecimal(value);

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == ValidationErrorMessage.MustProvideTheLowerNumber("money value", 999999999));
    }

    private class TestMoneyValueObject : MoneyValueObject
    {
        public TestMoneyValueObject(string value, string fieldName, string displayName)
            : base(value, fieldName, displayName)
        {
        }

        public TestMoneyValueObject(decimal value, string fieldName, string displayName)
            : base(value, fieldName, displayName)
        {
        }

        public static TestMoneyValueObject FromString(string value)
        {
            return new TestMoneyValueObject(value, nameof(TestMoneyValueObject), "money value");
        }

        public static TestMoneyValueObject FromDecimal(decimal value)
        {
            return new TestMoneyValueObject(value, nameof(TestMoneyValueObject), "money value");
        }
    }
}
