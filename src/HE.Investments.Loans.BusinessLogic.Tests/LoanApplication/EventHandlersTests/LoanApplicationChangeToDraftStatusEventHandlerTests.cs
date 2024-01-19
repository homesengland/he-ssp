using HE.Investments.Common.Tests.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.LoanApplication.EventHandlers;
using HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;
using HE.Investments.Loans.Contract.Application.Events;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.EventHandlersTests;

public class LoanApplicationChangeToDraftStatusEventHandlerTests : TestBase<LoanApplicationChangeToDraftStatusEventHandler>
{
    [Fact]
    public async Task ShouldCallRepositoryWithMoveToDraftAction()
    {
        // given
        var loanApplicationId = LoanApplicationIdTestData.LoanApplicationIdOne;

        var userAccount = AccountUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        var loanApplicationEntity = LoanApplicationTestBuilder.NewSubmitted(userAccount).Build();

        var loanApplicationRepositoryMock = LoanApplicationRepositoryTestBuilder
            .New()
            .ReturnLoanApplication(loanApplicationId, userAccount, loanApplicationEntity)
            .BuildMockAndRegister(this);

        // when
        await TestCandidate.Handle(new LoanApplicationChangeToDraftStatusEvent(loanApplicationId), CancellationToken.None);

        // then
        loanApplicationRepositoryMock
            .Verify(
                x =>
                    x.MoveToDraft(loanApplicationId, CancellationToken.None),
                Times.Once);
    }
}
