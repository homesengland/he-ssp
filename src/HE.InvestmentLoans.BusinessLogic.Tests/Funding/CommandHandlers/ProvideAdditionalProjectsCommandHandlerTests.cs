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
public class ProvideAdditionalProjectsCommandHandlerTests : TestBase<ProvideAdditionalProjectsCommandHandler>
{
    [Fact]
    public async Task ShouldSaveAdditionalProjects()
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

        var newAdditionalProjectsBool = "false";

        // when
        var result = await TestCandidate.Handle(
            new ProvideAdditionalProjectsCommand(loanApplicationId, newAdditionalProjectsBool),
            CancellationToken.None);

        // then
        result.IsValid.Should().BeTrue();
        fundingEntity.AdditionalProjects!.IsThereAnyAdditionalProject.Should().BeFalse();
        fundingRepositoryMock.Verify(x => x.SaveAsync(
                It.Is<FundingEntity>(y =>
                y.AdditionalProjects!.IsThereAnyAdditionalProject == bool.Parse(newAdditionalProjectsBool)),
                userAccount,
                CancellationToken.None));
    }
}
