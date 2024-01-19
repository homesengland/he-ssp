using HE.Investments.Common.Contract;
using HE.Investments.Common.Tests.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.CommandHandlers;
using HE.Investments.Loans.BusinessLogic.Tests.CompanyStructure.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;
using HE.Investments.Loans.Contract.CompanyStructure.Commands;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.CompanyStructure.CommandHandlers;

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
