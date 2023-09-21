using HE.InvestmentLoans.BusinessLogic.Funding.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.Funding.Entities;
using HE.InvestmentLoans.BusinessLogic.Tests.Funding.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.Common.Tests.TestFramework;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Funding.Commands;
using Moq;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Funding.CommandHandlers;
public class ProvidePrivateSectorFundingCommandHandlerTests : TestBase<ProvidePrivateSectorFundingCommandHandler>
{
    [Fact]
    public async Task ShouldSavePrivateSectorFunding()
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

        var newPrivateSectorFundingBool = CommonResponse.Yes;
        var newPrivateSectorFundingResult = "NewPrivateSectorFundingResult";

        // when
        var result = await TestCandidate.Handle(
            new ProvidePrivateSectorFundingCommand(loanApplicationId, newPrivateSectorFundingBool, newPrivateSectorFundingResult, null),
            CancellationToken.None);

        // then
        result.IsValid.Should().BeTrue();
        fundingEntity.PrivateSectorFunding!.IsApplied.Should().BeTrue();
        fundingEntity.PrivateSectorFunding!.PrivateSectorFundingApplyResult.Should().Be(newPrivateSectorFundingResult);
        fundingEntity.PrivateSectorFunding!.PrivateSectorFundingNotApplyingReason.Should().BeNull();
        fundingRepositoryMock.Verify(x => x.SaveAsync(
                It.Is<FundingEntity>(y =>
                y.PrivateSectorFunding!.IsApplied == (newPrivateSectorFundingBool == CommonResponse.Yes) &&
                y.PrivateSectorFunding!.PrivateSectorFundingApplyResult == newPrivateSectorFundingResult &&
                y.PrivateSectorFunding!.PrivateSectorFundingNotApplyingReason == null),
                userAccount,
                CancellationToken.None));
    }
}
