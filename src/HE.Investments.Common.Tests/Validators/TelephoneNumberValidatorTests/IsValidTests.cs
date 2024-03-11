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
    [InlineData("+48 123 456 7890")]
    [InlineData("+481234567890")]
    [InlineData("+444 07771 900 900")]
    [InlineData("+31 (0)20 7450 4000")]
    [InlineData("+ (0)20 7450 4000")]
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
        result.Errors.Should().ContainSingle(ValidationErrorMessage.EnterUkTelephoneNumber);
    }

    [Theory]
    [InlineData("test")]
    [InlineData(null)]
    [InlineData("020 7450 40005")]
    [InlineData("123 123 123 123")]
    [InlineData("+44 (0)20 7450 4002df")]
    [InlineData("+44 (0)20 7450 abcd")]
    [InlineData("+44")]
    [InlineData("dd+44 (0)20 7450 400")]
    [InlineData("+44 (0)11 22 44 543")]
    [InlineData("(0)11 22 44 543")]
    [InlineData("011 22 44 543")]
    [InlineData("+44 (0)11 222 444 543")]
    [InlineData("(0)11 222 444 543")]
    [InlineData("+011 222 444 543")]
    [InlineData("+011 222 444 5433567")]
    [InlineData("(0)00 7450 4000")]
    [InlineData("00273 800 900")]
    [InlineData("00071 900 900")]
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
        result.Errors.Should().ContainSingle(ValidationErrorMessage.EnterTelephoneNumberInValidFormat);
    }
}
