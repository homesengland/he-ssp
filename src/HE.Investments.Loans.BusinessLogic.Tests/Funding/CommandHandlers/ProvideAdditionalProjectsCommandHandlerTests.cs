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

        var userAccount = AccountUserContextTestBuilder
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
