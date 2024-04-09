using FluentAssertions;
using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.Tests.FinancialDetails.ValueObjects;

public class ExpectedPurchasePriceValueTests
{
    [Fact]
    public void ShouldThrowDomainValidationException_WhenValueIsEmpty()
    {
        // given && when
        var action = () => new ExpectedPurchasePrice(string.Empty);

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == ValidationErrorMessage.MissingRequiredField(ExpectedPurchasePrice.Fields.DisplayName!));
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenValueIsNegative()
    {
        // given && when
        var action = () => new ExpectedPurchasePrice("-1");

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == "The expected purchase price of the land must be a whole number between 0 and 999999999");
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenValueIsOutOfRange()
    {
        // given && when
        var action = () => new ExpectedPurchasePrice("1000000000");

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == "The expected purchase price of the land must be a whole number between 0 and 999999999");
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenValueIsDecimal()
    {
        // given && when
        var action = () => new ExpectedPurchasePrice("10.234");

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == "The expected purchase price of the land must be entered as a number, in pounds");
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenValueIsNotANumber()
    {
        // given && when
        var action = () => new ExpectedPurchasePrice("abc");

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == "The expected purchase price of the land must be entered as a number, in pounds");
    }

    [Theory]
    [InlineData("0", 0)]
    [InlineData("100", 100)]
    [InlineData("999999999", 999999999)]
    public void ShouldCreateExpectedPurchasePrice_WhenValueIsValid(string input, decimal expectedValue)
    {
        // given && when
        var landValue = new ExpectedPurchasePrice(input);

        // then
        landValue.Value.Should().Be(expectedValue);
    }

    [Fact]
    public void ShouldCreateExpectedPurchasePrice_WhenIntValueIsValid()
    {
        // given && when
        var landValue = new ExpectedPurchasePrice(100);

        // then
        landValue.Value.Should().Be(100);
    }
}
