using FluentAssertions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;
using Xunit;

namespace HE.Investments.Common.Tests.Validators.DateValidatorTests;

public class IsProvidedTests
{
    [Fact]
    public void ShouldNotReturnError_WhenValueIsProvided()
    {
        // given
        var result = OperationResult.New();

        // when
        DateValidator
            .For("1", "2", "3", "test", "test", result)
            .IsProvided();

        // then
        result.HasValidationErrors.Should().BeFalse();
    }

    [Theory]
    [InlineData("", "2", "3")]
    [InlineData("1", " ", "3")]
    [InlineData("1", "2", "     ")]
    [InlineData(null, "2", "3")]
    [InlineData("1", null, "3")]
    [InlineData("1", "2", null)]
    public void ShouldReturnError_WhenValueIsNotProvided(string? day, string? month, string? year)
    {
        // given
        var result = OperationResult.New();

        // when
        DateValidator
            .For(day, month, year, "test", "test", result)
            .IsProvided();

        // then
        result.HasValidationErrors.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.ErrorMessage == ValidationErrorMessage.EnterDate);
    }
}
