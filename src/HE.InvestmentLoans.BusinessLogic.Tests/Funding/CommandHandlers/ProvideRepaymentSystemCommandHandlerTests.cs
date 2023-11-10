using HE.InvestmentLoans.BusinessLogic.Funding.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.Funding.Entities;
using HE.InvestmentLoans.BusinessLogic.Tests.Funding.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Funding.Commands;
using HE.Investments.Account.Domain.Tests.User.TestObjectBuilder;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Funding.CommandHandlers;
public class ProvideRepaymentSystemCommandHandlerTests : TestBase<ProvideRepaymentSystemCommandHandler>
{
    [Fact]
    public async Task ShouldSaveRepaymentSystem()
    {
        // given
        var loanApplicationId = LoanApplicationIdTestData.LoanApplicationIdOne;

        var fundingEntity = FundingEntityTestBuilder
            .New()
            .Build();

        var userAccount = AccountUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        var fundingRepositoryMock = FundingRepositoryTestBuilder
            .New()
            .ReturnFundingEntity(loanApplicationId, userAccount, fundingEntity)
            .BuildMockAndRegister(this);

        var newRepaymentSystemChoice = FundingFormOption.Repay;

        // when
        var result = await TestCandidate.Handle(
            new ProvideRepaymentSystemCommand(loanApplicationId, newRepaymentSystemChoice, null),
            CancellationToken.None);

        // then
        result.IsValid.Should().BeTrue();
        fundingEntity.RepaymentSystem!.Refinance.Should().BeNull();
        fundingEntity.RepaymentSystem!.Repay?.Value.Should().Be(newRepaymentSystemChoice);
        fundingRepositoryMock.Verify(x => x.SaveAsync(
                It.Is<FundingEntity>(y =>
                y.RepaymentSystem!.Refinance == null &&
                y.RepaymentSystem!.Repay!.Value == newRepaymentSystemChoice),
                userAccount,
                CancellationToken.None));
    }
}
