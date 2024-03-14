using FluentAssertions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;
using Xunit;

namespace HE.Investments.Common.Tests.Validators.TelephoneNumberValidatorTests;

public class IsValidTests
{
    [Theory]
    [InlineData("+44 (0)20 7450 4000")]
    [InlineData("020 7450 4000")]
    [InlineData("(0)20 7450 4000")]
    [InlineData("01273 800 900")]
    [InlineData("07771 900 900")]
    [InlineData("00 44 1234 567 890")]
    [InlineData("00-44-1234-567-890")]
    [InlineData("+44 1234 567 890")]
    [InlineData("+44-1234-567-890")]
    [InlineData("+44 (0)1234 567 890")]
    [InlineData("+44-(0)-1234-567-890")]
    [InlineData("0 1234 567 890")]
    [InlineData("0-1234-567-890")]
    [InlineData("(0)1234 567 890")]
    [InlineData("1234 567 890")]
    [InlineData("1234-567-890")]
    [InlineData("7-23456-7892")]
    [InlineData("07-23456-7892")]
    [InlineData("(0)7-23456-7892")]
    [InlineData("00-44-7-23456-7892")]
    [InlineData("+44-7-23456-7892")]
    public void ShouldNotReturnError_WhenValueIsValid(string telephoneNumber)
    {
        // given
        var result = OperationResult.New();

        // when
        string? validatedTelephoneNumber = TelephoneNumberValidator
            .For(telephoneNumber, "TelephoneNumber", "test", result)
            .IsValid();

        // then
        result.HasValidationErrors.Should().BeFalse();
        validatedTelephoneNumber.Should().NotBeNull();
        validatedTelephoneNumber.Should().Be(telephoneNumber);
    }

    [Theory]
    [InlineData("00481234567890")]
    [InlineData("00-48-1234-567-890")]
    [InlineData("00 48 1234 567 890")]
    [InlineData("00-48-(0)-1234-567-890")]
    [InlineData("00 48 (0)1234 567 890")]
    [InlineData("+481234567890")]
    [InlineData("+48-1234-567-890")]
    [InlineData("+48 1234 567 890")]
    [InlineData("+48-(0)-1234-567-890")]
    [InlineData("+48 (0)1234 567 890")]
    public void ShouldReturnUkTelephoneNumberValidationError_WhenValueIsInvalid(string? telephoneNumber)
    {
        // given
        var result = OperationResult.New();

        // when
        TelephoneNumberValidator
            .For(telephoneNumber, "TelephoneNumber", "test", result)
            .IsValid();

        // then
        result.HasValidationErrors.Should().BeTrue();
        result.Errors.Should().Contain(error => error.ErrorMessage == ValidationErrorMessage.EnterUkTelephoneNumber);
    }

    [Theory]
    [InlineData("test")]
    [InlineData("1234567891d")]
    [InlineData("12345678911")]
    [InlineData("012345678911")]
    [InlineData("0123456789")]
    [InlineData("004412345678911")]
    [InlineData("00-44-12345-678-911")]
    [InlineData("00 44 12345 678 911")]
    [InlineData("00 44(0)12345678911")]
    [InlineData("0044123456789")]
    [InlineData("00 44 123 456 789")]
    [InlineData("00 44(0)123456789")]
    [InlineData("+4412345678911")]
    [InlineData("+44-12345-678-911")]
    [InlineData("+44 12345 678 911")]
    [InlineData("+44(0)12345678911")]
    [InlineData("+44123456789")]
    [InlineData("+44-123-456-789")]
    [InlineData("+44 123 456 789")]
    [InlineData("+44(0)123456789")]
    [InlineData("00--44--1234--567--890")]
    [InlineData(")0(1234567890")]
    [InlineData("[0]1234567890")]
    [InlineData("(0)020 7450 400")]
    public void ShouldReturnTelephoneNumberFormatValidationError_WhenValueIsInvalid(string? telephoneNumber)
    {
        // given
        var result = OperationResult.New();

        // when
        TelephoneNumberValidator
            .For(telephoneNumber, "TelephoneNumber", "test", result)
            .IsValid();

        // then
        result.HasValidationErrors.Should().BeTrue();
        result.Errors.Should().Contain(error => error.ErrorMessage == ValidationErrorMessage.EnterTelephoneNumberInValidFormat);
    }
}
