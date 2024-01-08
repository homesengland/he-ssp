using FluentAssertions;
using HE.Investments.Common.Validators;
using Xunit;

namespace HE.Investments.Common.Tests.Validators.DateValidatorTests;

public class IsValidTests
{
    [Fact]
    public void ShouldNotReturnError_WhenValueIsValid()
    {
        // given
        var result = OperationResult.New();

        // when
        DateTime? date = DateValidator
            .For("1", "2", "3", "test", "test", result)
            .IsValid();

        // then
        result.HasValidationErrors.Should().BeFalse();
        date.Should().NotBeNull();
        date!.Value.Day.Should().Be(1);
        date.Value.Month.Should().Be(2);
        date.Value.Year.Should().Be(3);
        date.Value.Kind.Should().Be(DateTimeKind.Unspecified);
    }

    [Theory]
    [InlineData("0", "2", "3")]
    [InlineData("1", "0", "3")]
    [InlineData("1", "2", "0")]
    [InlineData("32", "2", "3")]
    [InlineData("1", "13", "3")]
    [InlineData("1", "2", "99999999999")]
    public void ShouldReturnError_WhenValueIsInvalid(string? day, string? month, string? year)
    {
        // given
        var result = OperationResult.New();

        // when
        DateValidator
            .For(day, month, year, "test", "test", result)
            .IsValid();

        // then
        result.HasValidationErrors.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.ErrorMessage == "The test must be a valid date");
    }
}
