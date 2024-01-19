using HE.Investments.Common.Tests.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.LoanApplication.CommandHandlers;
using HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;
using HE.Investments.Loans.Contract.Application.Commands;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.CommandHandlers;

public class WithdrawLoanApplicationCommandHandlerTests : TestBase<WithdrawLoanApplicationCommandHandler>
{
    [Fact]
    public async Task ShouldWithdrawLoanApplication()
    {
        // given
        var loanApplicationId = LoanApplicationIdTestData.LoanApplicationIdOne;

        var userAccount = AccountUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        var loanApplicationEntity = LoanApplicationTestBuilder.NewDraft(userAccount).Build();

        LoanApplicationRepositoryTestBuilder
            .New()
            .ReturnLoanApplication(loanApplicationId, userAccount, loanApplicationEntity)
            .BuildMockAndRegister(this);

        var withdrawReason = "Withdraw reason";

        // when
        var result = await TestCandidate.Handle(new WithdrawLoanApplicationCommand(loanApplicationId, withdrawReason), CancellationToken.None);

        // then
        result.IsValid.Should().BeTrue();
    }
}
