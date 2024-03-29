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
public class ProvideGrossDevelopmentValueCommandHandlerTests : TestBase<ProvideGrossDevelopmentValueCommandHandler>
{
    [Fact]
    public async Task ShouldSaveGrossDevelopmentValue()
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

        var newGrossDevelopmentValue = "100";
        _ = int.TryParse(newGrossDevelopmentValue, out var parsedNewGrossDevelopmentValue);

        // when
        var result = await TestCandidate.Handle(
            new ProvideGrossDevelopmentValueCommand(loanApplicationId, newGrossDevelopmentValue),
            CancellationToken.None);

        // then
        result.IsValid.Should().BeTrue();
        fundingEntity.GrossDevelopmentValue!.Value.Should().Be(parsedNewGrossDevelopmentValue);
        fundingRepositoryMock.Verify(x => x.SaveAsync(
                It.Is<FundingEntity>(y =>
                y.GrossDevelopmentValue!.Value == parsedNewGrossDevelopmentValue),
                userAccount,
                CancellationToken.None));
    }
}
