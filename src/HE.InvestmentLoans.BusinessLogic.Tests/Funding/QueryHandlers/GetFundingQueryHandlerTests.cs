using HE.InvestmentLoans.BusinessLogic.Funding.QueryHandlers;
using HE.InvestmentLoans.BusinessLogic.Tests.Funding.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Funding.Queries;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Funding.QueryHandlers;
public class GetFundingQueryHandlerTests : TestBase<GetFundingQueryHandler>
{
    [Fact]
    public async Task ShouldReturnFunding()
    {
        // given
        var loanApplicationId = LoanApplicationIdTestData.LoanApplicationIdOne;

        var fundingEntity = FundingEntityTestBuilder
            .New()
            .WithAllDataProvided()
            .Build();

        var userAccount = LoanUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        FundingRepositoryTestBuilder
            .New()
            .ReturnFundingEntity(loanApplicationId, userAccount, fundingEntity)
            .BuildMockAndRegister(this);

        // when
        var result = await TestCandidate.Handle(new GetFundingQuery(loanApplicationId), CancellationToken.None);

        // then
        result.ViewModel.LoanApplicationId.Should().Be(fundingEntity.LoanApplicationId.ToString());
        result.ViewModel.GrossDevelopmentValue.Should().Be(fundingEntity.GrossDevelopmentValue!.ToString());
        result.ViewModel.TotalCosts.Should().Be(fundingEntity.EstimatedTotalCosts!.ToString());
        result.ViewModel.AbnormalCosts.Should().Be(CommonResponse.Yes);
        result.ViewModel.AbnormalCostsInfo.Should().Be(fundingEntity.AbnormalCosts!.AbnormalCostsAdditionalInformation);
        result.ViewModel.PrivateSectorFunding.Should().Be(CommonResponse.No);
        result.ViewModel.PrivateSectorFundingReason.Should().Be(fundingEntity.PrivateSectorFunding!.PrivateSectorFundingNotApplyingReason);
        result.ViewModel.Refinance.Should().Be(fundingEntity.RepaymentSystem!.Refinance!.Value);
        result.ViewModel.RefinanceInfo.Should().Be(fundingEntity.RepaymentSystem!.Refinance!.AdditionalInformation);
        result.ViewModel.AdditionalProjects.Should().Be(CommonResponse.Yes);
    }
}
