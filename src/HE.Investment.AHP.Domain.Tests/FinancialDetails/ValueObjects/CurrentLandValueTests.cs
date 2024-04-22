using FluentAssertions;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.FinancialDetails.ValueObjects;

public class CurrentLandValueTests
{
    [Fact]
    public void ShouldThrowDomainValidationException_WhenValueIsEmpty()
    {
        // given && when
        var action = () => new CurrentLandValue(string.Empty);

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .WithMessage("Enter the current value of the land, in pounds");
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenValueIsNegative()
    {
        // given && when
        var action = () => new CurrentLandValue("-1");

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == "The current value of the land must be 0 or more");
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenValueIsLessThanMinInteger()
    {
        // given && when
        var action = () => new CurrentLandValue("-1000000000000000000000000000000000000000000000000");

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == "The current value of the land must be 0 or more");
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenValueIsOutOfRange()
    {
        // given && when
        var action = () => new CurrentLandValue("1000000000");

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == "The current value of the land must be 999999999 or fewer");
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenValueIsMoreThanMaxInteger()
    {
        // given && when
        var action = () => new CurrentLandValue("1000000000000000000000000000000000000000000000000");

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == "The current value of the land must be 999999999 or fewer");
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenValueIsDecimal()
    {
        // given && when
        var action = () => new CurrentLandValue("10.234");

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == "The current value of the land must not include pence, like 300");
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenValueIsNotANumber()
    {
        // given && when
        var action = () => new CurrentLandValue("abc");

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == "The current value of the land must be a whole number, like 300");
    }

    [Theory]
    [InlineData("0", 0)]
    [InlineData("100", 100)]
    [InlineData("999999999", 999999999)]
    public void ShouldCreateLandValue_WhenValueIsValid(string input, int expectedValue)
    {
        // given && when
        var landValue = new CurrentLandValue(input);

        // then
        landValue.Value.Should().Be(expectedValue);
    }

    [Fact]
    public void ShouldCreateLandValue_WhenIntValueIsValid()
    {
        // given && when
        var landValue = CurrentLandValue.FromCrm(100);

        // then
        landValue.Value.Should().Be(100);
    }
}
