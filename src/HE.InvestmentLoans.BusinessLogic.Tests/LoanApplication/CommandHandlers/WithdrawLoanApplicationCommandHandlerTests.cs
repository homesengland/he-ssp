using HE.InvestmentLoans.BusinessLogic.LoanApplication.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.Contract.Application.Commands;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.CommandHandlers;

public class WithdrawLoanApplicationCommandHandlerTests : TestBase<WithdrawLoanApplicationCommandHandler>
{
    [Fact]
    public async Task ShouldWithdrawLoanApplication()
    {
        // given
        var loanApplicationId = LoanApplicationIdTestData.LoanApplicationIdOne;

        var userAccount = LoanUserContextTestBuilder
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
