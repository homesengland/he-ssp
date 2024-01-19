using HE.Investments.Common.Tests.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Funding.CommandHandlers;
using HE.Investments.Loans.BusinessLogic.Funding.Entities;
using HE.Investments.Loans.BusinessLogic.Tests.Funding.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.Contract.Funding.Commands;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Funding.CommandHandlers;
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
