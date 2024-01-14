using FluentAssertions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Validators;
using Xunit;

namespace HE.Investments.Common.Tests.Validators.NumericValidatorTests;

public class IsNumberTests
{
    private const string ErrorMessage = "The test must be a number";

    [Fact]
    public void ShouldNotReturnError_WhenValueIsNull()
    {
        // given
        var result = OperationResult.New();

        // when
        NumericValidator
            .For(null, "test", "test", result)
            .IsNumber();

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
            .IsNumber();

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
            .IsNumber();

        // then
        result.HasValidationErrors.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.ErrorMessage == ErrorMessage);
    }

    [Theory]
    [InlineData("23,45")]
    [InlineData("23.45")]
    [InlineData("2345")]
    public void ShouldNotReturnError_WhenValueIsNumber(string value)
    {
        // given
        var result = OperationResult.New();

        // when
        NumericValidator
            .For(value, "test", "test", result)
            .IsNumber();

        // then
        result.HasValidationErrors.Should().BeFalse();
    }
}
