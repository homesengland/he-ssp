using HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;
using HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.Common.Exceptions;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.LoanApplicationEntityTests;

public class CheckIfCanBeSubmittedTests : TestBase<LoanApplicationEntity>
{
    [Fact]
    public void ShouldThrowDomainException_WhenLoanApplicationHasDraftStatusAndItIsNotReadyForSubmit()
    {
        // given
        var userAccount = LoanUserContextTestBuilder
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
        var userAccount = LoanUserContextTestBuilder
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
        var userAccount = LoanUserContextTestBuilder
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
