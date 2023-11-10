using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.Tests.Assertions;
using HE.InvestmentLoans.Common.Tests.TestData;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Messages;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.ValueObjects;
public class PublicSectorGrantFundingTests
{
    [Fact]
    public void ShouldThrowDomainValidationError_WhenProviderNameExceedsShortTextLimit()
    {
        var action = () => PublicSectorGrantFunding.FromString(
                TextTestData.TextThatExceedsShortInputLimit,
                PoundsTestData.CorrectAmountAsString,
                TextTestData.TextThatNotExceedsShortInputLimit,
                TextTestData.TextThatNotExceedsLongInputLimit);

        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.GrantFundingProviderName));
    }

    [Fact]
    public void ShouldThrowDomainValidationError_WhenAmountIsIncorrect()
    {
        var action = () => PublicSectorGrantFunding.FromString(
                TextTestData.TextThatNotExceedsShortInputLimit,
                PoundsTestData.IncorrectAmountAsString,
                TextTestData.TextThatNotExceedsShortInputLimit,
                TextTestData.TextThatNotExceedsLongInputLimit);

        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.IncorrectGrantFundingAmount);
    }

    [Fact]
    public void ShouldThrowDomainValidationError_WhenGrantNameExceedsShortTextLimit()
    {
        var action = () => PublicSectorGrantFunding.FromString(
                TextTestData.TextThatNotExceedsShortInputLimit,
                PoundsTestData.CorrectAmountAsString,
                TextTestData.TextThatExceedsShortInputLimit,
                TextTestData.TextThatNotExceedsLongInputLimit);

        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.GrantFundingName));
    }

    [Fact]
    public void ShouldThrowDomainValidationError_WhenPurposeExceedsLongTextLimit()
    {
        var action = () => PublicSectorGrantFunding.FromString(
                TextTestData.TextThatNotExceedsShortInputLimit,
                PoundsTestData.CorrectAmountAsString,
                TextTestData.TextThatNotExceedsShortInputLimit,
                TextTestData.TextThatExceedsLongInputLimit);

        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.GrantFundingPurpose));
    }

    [Fact]
    public void ShouldThrowDomainValidationWithAllErrors()
    {
        var action = () => PublicSectorGrantFunding.FromString(
                TextTestData.TextThatExceedsShortInputLimit,
                PoundsTestData.IncorrectAmountAsString,
                TextTestData.TextThatExceedsShortInputLimit,
                TextTestData.TextThatExceedsLongInputLimit);

        action.Should().ThrowExactly<DomainValidationException>()
            .WithErrorMessage(ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.GrantFundingPurpose))
            .WithErrorMessage(ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.GrantFundingName))
            .WithErrorMessage(ValidationErrorMessage.IncorrectGrantFundingAmount)
            .WithErrorMessage(ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.GrantFundingName));
    }
}
