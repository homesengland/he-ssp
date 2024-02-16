using FluentAssertions;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Messages;
using Xunit;

namespace HE.Investments.Common.Tests.Domain.ValueObjects;

public class WholePercentageTests
{
    [Fact]
    public void ShouldThrowDomainValidationException_WhenValueIsEmpty()
    {
        // given && when
        var action = () => WholePercentage.FromString(string.Empty);

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == ValidationErrorMessage.MissingRequiredField("Value"));
    }

    [Theory]
    [InlineData("-1")]
    [InlineData("10.1")]
    [InlineData("1,1")]
    public void ShouldThrowDomainValidationException_WhenValueIsNotCorrectPercentage(string value)
    {
        // given && when
        var action = () => WholePercentage.FromString(value);

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == ValidationErrorMessage.PercentageInput("Value"));
    }

    [Theory]
    [InlineData("0", 0)]
    [InlineData("100", 1)]
    [InlineData("1", 0.01)]
    [InlineData("33", 0.33)]
    [InlineData("123", 1.23)]
    public void ShouldCreatePercentage_WhenValueIsValid(string input, decimal expectedValue)
    {
        // given && when
        var percentage = WholePercentage.FromString(input);

        // then
        percentage.Value.Should().Be(expectedValue);
    }
}
