using FluentAssertions;
using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Messages;
using Xunit;

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
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == ValidationErrorMessage.MissingRequiredField(CurrentLandValue.Fields.DisplayName!));
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
            .ContainSingle(x => x.ErrorMessage == FinancialDetailsValidationErrors.InvalidLandValue);
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
            .ContainSingle(x => x.ErrorMessage == FinancialDetailsValidationErrors.InvalidLandValue);
    }

    [Theory]
    [InlineData("0", 0)]
    [InlineData("100", 100)]
    [InlineData("100.12366", 100.12)]
    public void ShouldCreateLandValue_WhenValueIsValid(string input, decimal expectedValue)
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
        var landValue = new CurrentLandValue(100);

        // then
        landValue.Value.Should().Be(100);
    }
}
