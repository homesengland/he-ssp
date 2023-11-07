using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Security.ValueObjects;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Messages;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Security.ValueObjects;

public class DebentureTests
{
    [Fact]
    public void ShouldCreateDebenture()
    {
        // given & when
        var debenture = new Debenture("holder", true);

        // then
        debenture.Holder.Should().Be("holder");
        debenture.Exists.Should().BeTrue();
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenHolderNameIsNotProvided()
    {
        // given & when
        var action = () => new Debenture(string.Empty, true);

        // then
        action.Should().Throw<DomainValidationException>().WithMessage(ValidationErrorMessage.EnterMoreDetails);
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenHolderNameIsTooLong()
    {
        // given & when
        var action = () => new Debenture(new string('a', MaximumInputLength.LongInput + 1), true);

        // then
        action.Should().Throw<DomainValidationException>().WithMessage(ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.Debenture));
    }

    [Fact]
    public void ShouldHolderBeEmpty_WhenExistsIsFalse()
    {
        // given & when
        var debenture = new Debenture("holder", false);

        // then
        debenture.Holder.Should().BeEmpty();
        debenture.Exists.Should().BeFalse();
    }
}
