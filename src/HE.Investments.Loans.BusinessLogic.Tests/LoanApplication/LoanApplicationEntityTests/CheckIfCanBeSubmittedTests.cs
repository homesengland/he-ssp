using HE.Investments.Account.Domain.Tests.User.TestObjectBuilder;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Entities;
using HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.TestObjectBuilders;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.LoanApplicationEntityTests;

public class CheckIfCanBeSubmittedTests : TestBase<LoanApplicationEntity>
{
    [Fact]
    public void ShouldThrowDomainException_WhenLoanApplicationHasDraftStatusAndItIsNotReadyForSubmit()
    {
        // given
        var userAccount = AccountUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        var loanApplicationEntity = LoanApplicationTestBuilder.NewDraft(userAccount).Build();

        // when
        var action = () => loanApplicationEntity.CheckIfCanBeSubmitted();

        // then
        action.Should().Throw<DomainException>().WithMessage("Loan application is not ready to be submitted");
    }

    [Fact]
    public void ShouldThrowDomainException_WhenLoanApplicationIsAlreadySubmitted()
    {
        // given
        var userAccount = AccountUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        var loanApplicationEntity = LoanApplicationTestBuilder.NewSubmitted(userAccount).Build();

        // when
        var action = () => loanApplicationEntity.CheckIfCanBeSubmitted();

        // then
        action.Should().Throw<DomainException>().WithMessage("Loan application has been submitted");
    }

    [Fact]
    public void ShouldNotThrowDomainException_WhenLoanApplicationCanBeSubmitted()
    {
        // given
        var userAccount = AccountUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        var loanApplicationEntity = LoanApplicationTestBuilder.NewDraft(userAccount).WithAllCompletedSections().Build();

        // when
        var action = () => loanApplicationEntity.CheckIfCanBeSubmitted();

        // then
        action.Should().NotThrow<DomainException>();
    }
}
