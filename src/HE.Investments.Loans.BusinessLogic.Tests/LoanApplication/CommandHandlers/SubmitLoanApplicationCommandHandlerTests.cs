using HE.Investments.Common.Contract;
using HE.Investments.Common.Tests.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.LoanApplication.CommandHandlers;
using HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;
using HE.Investments.Loans.Contract.Application.Commands;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.CommandHandlers;

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
