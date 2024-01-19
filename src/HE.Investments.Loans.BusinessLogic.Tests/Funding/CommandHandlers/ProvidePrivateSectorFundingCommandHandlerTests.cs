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

        var userAccount = AccountUserContextTestBuilder
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
