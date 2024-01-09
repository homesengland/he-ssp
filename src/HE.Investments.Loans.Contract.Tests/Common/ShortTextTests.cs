using FluentAssertions;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.Common.Tests.TestData;
using HE.Investments.Loans.Contract.Common;
using Xunit;

namespace HE.Investments.Loans.Contract.Tests.Common;

public class ShortTextTests
{
    [Fact]
    public void ShouldThrowValidationError_WhenNumberExceedsShortInputLimit()
    {
        // given && when
        var action = () => new ShortText(TextTestData.TextThatExceedsShortInputLimit);

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == GenericValidationError.TextTooLong);
    }

    [Fact]
    public void ShouldThrowValidationError_WhenTextNotProvided()
    {
        // given && when
        var action = () => new ShortText(null);

        // then
        action.Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == GenericValidationError.NoValueProvided);
    }

    [Fact]
    public void ShouldCreateText_WhenNumberDoesNotExceedShortInputLimit()
    {
        // given && when
        var text = new ShortText(TextTestData.TextThatNotExceedsShortInputLimit);

        // then
        text.Value.Should().Be(TextTestData.TextThatNotExceedsShortInputLimit);
    }
}
