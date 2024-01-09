using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.BusinessLogic.Tests.Assertions;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.ValueObjects;

public class WithdrawReasonCtorTests
{
    [Fact]
    public void ShouldCreateWithdrawReason()
    {
        // given
        var withdrawReason = "new very important reason";

        // when
        var action = () => WithdrawReason.New(withdrawReason);

        // then
        action.Should().NotThrow<DomainValidationException>();
        action().Value.Should().Be(withdrawReason);
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenWithdrawReasonIsNotProvided()
    {
        // given
        var withdrawReason = string.Empty;

        // when
        var action = () => WithdrawReason.New(withdrawReason);

        // then
        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.EnterWhyYouWantToWithdrawApplication);
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenWithdrawReasonInputIsLongerThan1500Chars()
    {
        // given
        var withdrawReason = new string('*', 1501);

        // when
        var action = () => WithdrawReason.New(withdrawReason);

        // then
        action.Should().Throw<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.WithdrawReason));
    }
}
