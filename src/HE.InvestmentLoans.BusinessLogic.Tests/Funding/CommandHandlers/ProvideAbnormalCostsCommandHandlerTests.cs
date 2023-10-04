using HE.InvestmentLoans.BusinessLogic.Funding.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.Funding.Entities;
using HE.InvestmentLoans.BusinessLogic.Tests.Funding.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.Contract.Funding.Commands;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Funding.CommandHandlers;
public class ProvideAbnormalCostsCommandHandlerTests : TestBase<ProvideAbnormalCostsCommandHandler>
{
    [Fact]
    public async Task ShouldSaveAbnormalCosts()
    {
        // given
        var loanApplicationId = LoanApplicationIdTestData.LoanApplicationIdOne;

        var fundingEntity = FundingEntityTestBuilder
            .New()
            .Build();

        var userAccount = LoanUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        var fundingRepositoryMock = FundingRepositoryTestBuilder
            .New()
            .ReturnFundingEntity(loanApplicationId, userAccount, fundingEntity)
            .BuildMockAndRegister(this);

        var newAbnormalCostBool = "false";

        // when
        var result = await TestCandidate.Handle(
            new ProvideAbnormalCostsCommand(loanApplicationId, newAbnormalCostBool, null),
            CancellationToken.None);

        // then
        result.IsValid.Should().BeTrue();
        fundingEntity.AbnormalCosts!.IsAnyAbnormalCost.Should().BeFalse();
        fundingEntity.AbnormalCosts!.AbnormalCostsAdditionalInformation.Should().BeNull();
        fundingRepositoryMock.Verify(x => x.SaveAsync(
                It.Is<FundingEntity>(y =>
                y.AbnormalCosts!.IsAnyAbnormalCost == bool.Parse(newAbnormalCostBool) &&
                y.AbnormalCosts!.AbnormalCostsAdditionalInformation == null),
                userAccount,
                CancellationToken.None));
    }
}
