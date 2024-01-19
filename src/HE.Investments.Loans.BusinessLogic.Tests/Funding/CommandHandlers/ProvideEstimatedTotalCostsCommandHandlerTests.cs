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
public class ProvideEstimatedTotalCostsCommandHandlerTests : TestBase<ProvideEstimatedTotalCostsCommandHandler>
{
    [Fact]
    public async Task ShouldSaveEstimatedTotalCosts()
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

        var newEstimatedTotalCosts = "100";
        _ = int.TryParse(newEstimatedTotalCosts, out var parsedNewEstimatedTotalCosts);

        // when
        var result = await TestCandidate.Handle(
            new ProvideEstimatedTotalCostsCommand(loanApplicationId, newEstimatedTotalCosts),
            CancellationToken.None);

        // then
        result.IsValid.Should().BeTrue();
        fundingEntity.EstimatedTotalCosts!.Value.Should().Be(parsedNewEstimatedTotalCosts);
        fundingRepositoryMock.Verify(x => x.SaveAsync(
                It.Is<FundingEntity>(y =>
                y.EstimatedTotalCosts!.Value == parsedNewEstimatedTotalCosts),
                userAccount,
                CancellationToken.None));
    }
}
