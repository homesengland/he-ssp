using FluentAssertions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Validators;
using Xunit;

namespace HE.Investments.Common.Tests.Validators.ValidatorTests;

public class IsLongInputTests
{
    private readonly Random _random = new();

    [Fact]
    public void ShouldNotReturnError_WhenValueIsValid()
    {
        // given
        var result = OperationResult.New();

        // when
        Validator
            .For("test value", "test", "test", result)
            .IsLongInput();

        // then
        result.HasValidationErrors.Should().BeFalse();
    }

    [Theory]
    [InlineData("\r\n")]
    [InlineData("\r")]
    [InlineData("\n")]
    public void ShouldNotReturnError_WhenValueWithLineEndingIsValid(string lineEnding)
    {
        // given
        var result = OperationResult.New();
        var testString = RandomString(1499);

        // when
        Validator
            .For($"{testString}{lineEnding}", "test", "test", result)
            .IsLongInput();

        // then
        result.HasValidationErrors.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnError_WhenValueTooLong()
    {
        // given
        var result = OperationResult.New();

        // when
        Validator
            .For(RandomString(1501), "test", "test", result)
            .IsLongInput();

        // then
        result.HasValidationErrors.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.ErrorMessage == "The test must be 1500 characters or less");
    }

    public string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
    }
}
