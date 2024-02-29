using FluentAssertions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Validators;
using Xunit;

namespace HE.Investments.Common.Tests.Validators.TelephoneNumberValidatorTests;

public class IsValidTests
{
    [Theory]
    [InlineData("+44 (0)20 7450 4000")]
    [InlineData("+44 (0)27 2678 6125")]
    public void ShouldNotReturnError_WhenValueIsValid(string telephoneNumber)
    {
        // given
        var result = OperationResult.New();

        // when
        string? validatedTelephoneNumber = TelephoneNumberValidator
            .For(telephoneNumber, "TelephoneNumber", "test", false, result)
            .IsValid();

        // then
        result.HasValidationErrors.Should().BeFalse();
        validatedTelephoneNumber.Should().NotBeNull();
        validatedTelephoneNumber.Should().Be(telephoneNumber);
    }

    [Theory]
    [InlineData("test")]
    [InlineData(null)]
    public void ShouldReturnError_WhenValueIsInvalid(string? telephoneNumber)
    {
        // given
        var result = OperationResult.New();

        // when
        TelephoneNumberValidator
            .For(telephoneNumber, "TelephoneNumber", "test", false, result)
            .IsValid();

        // then
        result.HasValidationErrors.Should().BeTrue();
    }
}
