using FluentAssertions;
using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Messages;
using Xunit;

namespace HE.Investment.AHP.Domain.Tests.FinancialDetails.ValueObjects;

public class ExpectedOnCostsTests
{
    [Fact]
    public void ShouldThrowDomainValidationException_WhenValueIsEmpty()
    {
        // given && when
        var action = () => new ExpectedOnCosts(string.Empty);

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == ValidationErrorMessage.MissingRequiredField(ExpectedOnCosts.Fields.DisplayName!));
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenValueIsNegative()
    {
        // given && when
        var action = () => new ExpectedOnCosts("-1");

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == FinancialDetailsValidationErrors.InvalidExpectedOnCosts);
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenValueIsNotANumber()
    {
        // given && when
        var action = () => new ExpectedOnCosts("abc");

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == FinancialDetailsValidationErrors.InvalidExpectedOnCosts);
    }

    [Theory]
    [InlineData("0", 0)]
    [InlineData("100", 100)]
    public void ShouldCreateExpectedOnCosts_WhenValueIsValid(string input, decimal expectedValue)
    {
        // given && when
        var landValue = new ExpectedOnCosts(input);

        // then
        landValue.Value.Should().Be(expectedValue);
    }
}
