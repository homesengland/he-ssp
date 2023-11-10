using HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;
using HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.Investments.Account.Domain.Tests.User.TestObjectBuilder;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.LoanApplicationEntityTests;

public class WithdrawTests : TestBase<LoanApplicationEntity>
{
    [Fact]
    public void ShouldThrowDomainException_WhenLoanApplicationIsAlreadyWithdrawn()
    {
        // given
        var loanApplicationId = LoanApplicationIdTestData.LoanApplicationIdOne;

        var userAccount = AccountUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        var loanApplicationEntity = LoanApplicationTestBuilder.NewDraft(userAccount).Build();

        var loanApplicationRepository = LoanApplicationRepositoryTestBuilder
            .New()
            .ReturnLoanApplication(loanApplicationId, userAccount, loanApplicationEntity)
            .Build();

        var withdrawReason = WithdrawReasonTestData.WithdrawReasonOne;
        loanApplicationEntity.ExternalStatus = ApplicationStatus.Withdrawn;

        // when
        var action = () => loanApplicationEntity.Withdraw(loanApplicationRepository, withdrawReason, CancellationToken.None);

        // then
        action.Should().ThrowAsync<DomainException>().WithMessage("Loan application cannot be withdrawn");
    }

    [Fact]
    public async Task ShouldExecuteWithdrawDraft_WhenLoanApplicationStatusIsDraft()
    {
        // given
        var loanApplicationId = LoanApplicationIdTestData.LoanApplicationIdOne;

        var userAccount = AccountUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        var loanApplicationEntity = LoanApplicationTestBuilder.NewDraft(userAccount).Build();

        var loanApplicationRepository = LoanApplicationRepositoryTestBuilder
            .New()
            .ReturnLoanApplication(loanApplicationId, userAccount, loanApplicationEntity)
            .BuildMockAndRegister(this);

        var withdrawReason = WithdrawReasonTestData.WithdrawReasonOne;

        // when
        await loanApplicationEntity.Withdraw(loanApplicationRepository.Object, withdrawReason, CancellationToken.None);

        // then
        loanApplicationRepository.Verify(repo => repo.WithdrawDraft(loanApplicationId, withdrawReason, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task ShouldExecuteWithdrawSubmitted_WhenLoanApplicationStatusIsAplicationSubmitted()
    {
        // given
        var loanApplicationId = LoanApplicationIdTestData.LoanApplicationIdOne;

        var userAccount = AccountUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        var loanApplicationEntity = LoanApplicationTestBuilder.NewSubmitted(userAccount).Build();

        var loanApplicationRepository = LoanApplicationRepositoryTestBuilder
            .New()
            .ReturnLoanApplication(loanApplicationId, userAccount, loanApplicationEntity)
            .BuildMockAndRegister(this);

        var withdrawReason = WithdrawReasonTestData.WithdrawReasonOne;

        // when
        await loanApplicationEntity.Withdraw(loanApplicationRepository.Object, withdrawReason, CancellationToken.None);

        // then
        loanApplicationRepository.Verify(repo => repo.WithdrawSubmitted(loanApplicationId, withdrawReason, CancellationToken.None), Times.Once);
    }
}
