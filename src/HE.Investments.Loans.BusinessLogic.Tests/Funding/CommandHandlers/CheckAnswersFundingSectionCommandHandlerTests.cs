using HE.Investments.Account.Domain.Tests.User.TestObjectBuilder;
using HE.Investments.Common.Domain;
using HE.Investments.Loans.BusinessLogic.Funding.CommandHandlers;
using HE.Investments.Loans.BusinessLogic.Tests.Funding.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.Contract.Funding.Commands;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Funding.CommandHandlers;
public class CheckAnswersFundingSectionCommandHandlerTests : TestBase<CheckAnswersFundingSectionCommandHandler>
{
    [Fact]
    public async Task ShouldSaveFundingAsCompleted()
    {
        // given
        var loanApplicationId = LoanApplicationIdTestData.LoanApplicationIdOne;

        var fundingEntity = FundingEntityTestBuilder
            .New()
            .WithAllDataProvided()
            .Build();

        var userAccount = AccountUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        FundingRepositoryTestBuilder
            .New()
            .ReturnFundingEntity(loanApplicationId, userAccount, fundingEntity)
            .BuildMockAndRegister(this);

        // when
        var result = await TestCandidate.Handle(
            new CheckAnswersFundingSectionCommand(loanApplicationId, CommonResponse.Yes),
            CancellationToken.None);

        // then
        result.IsValid.Should().BeTrue();
        fundingEntity.Status!.Should().Be(SectionStatus.Completed);
    }
}
