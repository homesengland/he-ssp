using HE.InvestmentLoans.BusinessLogic.CompanyStructure.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.CompanyStructure.Commands;
using HE.Investments.Account.Domain.Tests.User.TestObjectBuilder;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.CommandHandlers;

public class CheckAnswersCompanyStructureSectionCommandHandlerTests : TestBase<CheckAnswersCompanyStructureSectionCommandHandler>
{
    [Fact]
    public async Task ShouldSaveCompanyStructureAsCompleted()
    {
        // given
        var loanApplicationId = LoanApplicationIdTestData.LoanApplicationIdOne;

        var companyStructureEntity = CompanyStructureEntityTestBuilder
            .New()
            .WithHomesBuild()
            .WithCompanyPurpose()
            .WithMoreInformation()
            .Build();

        var userAccount = AccountUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        CompanyStructureRepositoryTestBuilder
            .New()
            .ReturnCompanyStructureEntity(loanApplicationId, userAccount, companyStructureEntity)
            .BuildMockAndRegister(this);

        // when
        var result = await TestCandidate.Handle(
            new CheckAnswersCompanyStructureSectionCommand(loanApplicationId, "Yes"),
            CancellationToken.None);

        // then
        result.IsValid.Should().BeTrue();
        companyStructureEntity.Status!.Should().Be(SectionStatus.Completed);
    }
}
