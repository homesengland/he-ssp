using FluentAssertions;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Tests.TestData;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Contract.Common;
using HE.Investments.Common.Exceptions;
using Xunit;

namespace HE.InvestmentLoans.Contract.Tests.Common;

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
