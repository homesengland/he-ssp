using HE.Investments.Common.Tests.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Funding.CommandHandlers;
using HE.Investments.Loans.BusinessLogic.Funding.Entities;
using HE.Investments.Loans.BusinessLogic.Tests.Funding.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;
using HE.Investments.Loans.Contract.Funding.Commands;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Funding.CommandHandlers;
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

        var userAccount = AccountUserContextTestBuilder
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
