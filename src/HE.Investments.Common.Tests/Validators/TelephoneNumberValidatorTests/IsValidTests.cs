using FluentAssertions;
using HE.Investments.Common.Contract.Validators;
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
    [InlineData("test")]
    [InlineData(null)]
    [InlineData("020 7450 40005")]
    [InlineData("+444 (0)20 7450 4000")]
    [InlineData("123 123 123 123")]
    [InlineData("+44 (0)20 7450 4002df")]
    [InlineData("+44 (0)20 7450 abcd")]
    [InlineData("+44")]
    [InlineData("dd+44 (0)20 7450 400")]
    public void ShouldReturnError_WhenValueIsInvalid(string? telephoneNumber)
    {
        // given
        var result = OperationResult.New();

        // when
        TelephoneNumberValidator
            .For(telephoneNumber, "TelephoneNumber", "test", result)
            .IsValid();

        // then
        result.HasValidationErrors.Should().BeTrue();
    }
}