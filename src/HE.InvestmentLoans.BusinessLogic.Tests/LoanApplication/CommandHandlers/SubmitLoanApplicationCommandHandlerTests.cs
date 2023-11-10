using HE.InvestmentLoans.BusinessLogic.LoanApplication.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.Contract.Application.Commands;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.Investments.Account.Domain.Tests.User.TestObjectBuilder;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.CommandHandlers;

public class SubmitLoanApplicationCommandHandlerTests : TestBase<SubmitLoanApplicationCommandHandler>
{
    [Fact]
    public async Task ShouldSubmitLoanApplication()
    {
        // given
        var loanApplicationId = LoanApplicationIdTestData.LoanApplicationIdOne;

        var userAccount = AccountUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        var loanApplicationEntity = LoanApplicationTestBuilder.NewDraft(userAccount).WithAllCompletedSections().Build();
        LoanApplicationRepositoryTestBuilder
            .New()
            .ReturnLoanApplication(loanApplicationId, userAccount, loanApplicationEntity)
            .BuildMockAndRegister(this);

        var iCanSubmitMock = LoanApplicationRepositoryTestBuilder
            .New()
            .BuildICanSubmitMockAndRegister(this);

        // when
        await TestCandidate.Handle(new SubmitLoanApplicationCommand(loanApplicationId), CancellationToken.None);

        // then
        iCanSubmitMock
            .Verify(
                x =>
                    x.Submit(loanApplicationId, CancellationToken.None),
                Times.Once);
    }

    [Fact]
    public async Task ShouldCallNotificationServiceIfApplicationWasPreviouslySubmitted()
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

        loanApplicationEntity.ExternalStatus = ApplicationStatus.Draft;

        // when
        await TestCandidate.Handle(new SubmitLoanApplicationCommand(loanApplicationId), CancellationToken.None);

        // then
        loanApplicationRepository.Verify(x => x.DispatchEvents(loanApplicationEntity, CancellationToken.None), Times.Once);
    }
}
