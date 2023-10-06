using HE.InvestmentLoans.BusinessLogic.Generic;
using HE.InvestmentLoans.BusinessLogic.Tests.Assertions;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Tests.TestData;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Generic.ValueObjects;
public class ShortTextTests
{
    [Fact]
    public void ShouldThrowValidationError_WhenNumberExceedsShortInputLimit()
    {
        var action = () => new ShortText(TextTestData.TextThatExceedsShortInputLimit);

        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(GenericValidationError.TextTooLong);
    }

    [Fact]
    public void ShouldThrowValidationError_WhenTextNotProvided()
    {
        var action = () => new ShortText(null);

        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(GenericValidationError.NoValueProvided);
    }

    [Fact]
    public void ShouldCreateText_WhenNumberDoesNotExceedShortInputLimit()
    {
        var text = new ShortText(TextTestData.TextThatNotExceedsShortInputLimit);

        text.Value.Should().Be(TextTestData.TextThatNotExceedsShortInputLimit);
    }
}
