using HE.InvestmentLoans.BusinessLogic.Funding.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.Funding.Entities;
using HE.InvestmentLoans.BusinessLogic.Tests.Funding.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.Common.Tests.TestFramework;
using HE.InvestmentLoans.Contract.Funding.Commands;
using Moq;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Funding.CommandHandlers;
public class ProvideEstimatedTotalCostsCommandHandlerTests : TestBase<ProvideEstimatedTotalCostsCommandHandler>
{
    [Fact]
    public async Task ShouldSaveEstimatedTotalCosts()
    {
        // given
        var loanApplicationId = LoanApplicationIdTestData.LoanApplicationIdOne;

        var fundingEntity = FundingEntityTestBuilder
            .New()
            .WithGrossDevelopmentValue()
            .WithEstimatedTotalCosts()
            .WithAbnormalCosts()
            .WithPrivateSectorFunding()
            .WithRepaymentSystem()
            .WithAdditionalProjects()
            .Build();

        var userAccount = LoanUserContextTestBuilder
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
