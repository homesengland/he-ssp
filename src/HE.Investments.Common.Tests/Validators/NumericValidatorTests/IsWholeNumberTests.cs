using FluentAssertions;
using HE.Investments.Common.Validators;
using Xunit;

namespace HE.Investments.Common.Tests.Validators.NumericValidatorTests;

public class IsWholeNumberTests
{
    private const string ErrorMessage = "The test must be a whole number";

    [Fact]
    public void ShouldNotReturnError_WhenValueIsNull()
    {
        // given
        var result = OperationResult.New();

        // when
        NumericValidator
            .For(null, "test", "test", result)
            .IsWholeNumber();

        // then
        result.HasValidationErrors.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnError_WhenValueIsEmpty()
    {
        // given
        var result = OperationResult.New();

        // when
        NumericValidator
            .For(string.Empty, "test", "test", result)
            .IsWholeNumber();

        // then
        result.HasValidationErrors.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.ErrorMessage == ErrorMessage);
    }

    [Fact]
    public void ShouldReturnError_WhenValueNotANumber()
    {
        // given
        var result = OperationResult.New();

        // when
        NumericValidator
            .For("some string", "test", "test", result)
            .IsWholeNumber();

        // then
        result.HasValidationErrors.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.ErrorMessage == ErrorMessage);
    }

    [Theory]
    [InlineData("23,45")]
    [InlineData("23.45")]
    public void ShouldReturnError_WhenValueIsDecimal(string value)
    {
        // given
        var result = OperationResult.New();

        // when
        NumericValidator
            .For(value, "test", "test", result)
            .IsWholeNumber();

        // then
        result.HasValidationErrors.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.ErrorMessage == ErrorMessage);
    }

    [Fact]
    public void ShouldNotReturnError_WhenValueIsWholeNumber()
    {
        // given
        var result = OperationResult.New();

        // when
        NumericValidator
            .For("234", "test", "test", result)
            .IsWholeNumber();

        // then
        result.HasValidationErrors.Should().BeFalse();
    }
}
