using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.BusinessLogic.Tests.Assertions;
using HE.Investments.Loans.Common.Tests.TestData;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.ValueObjects;
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

        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.PoundInput("grant funding amount"));
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
            .WithErrorMessage(ValidationErrorMessage.PoundInput("grant funding amount"))
            .WithErrorMessage(ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.GrantFundingName));
    }
}
