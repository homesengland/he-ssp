using HE.InvestmentLoans.BusinessLogic.CompanyStructure;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.Contract.CompanyStructure.Commands;
using HE.Investments.Account.Domain.Tests.User.TestObjectBuilder;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.CommandHandlers;

public class ProvideMoreInformationAboutOrganizationCommandHandlerTests : TestBase<ProvideMoreInformationAboutOrganizationCommandHandler>
{
    [Fact]
    public async Task ShouldSaveMoreInformation()
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

        var companyStructureRepositoryMock = CompanyStructureRepositoryTestBuilder
            .New()
            .ReturnCompanyStructureEntity(loanApplicationId, userAccount, companyStructureEntity)
            .BuildMockAndRegister(this);

        var newMoreInformation = "NewMoreInformation";

        // when
        var result = await TestCandidate.Handle(
            new ProvideMoreInformationAboutOrganizationCommand(loanApplicationId, newMoreInformation, null, null),
            CancellationToken.None);

        // then
        result.IsValid.Should().BeTrue();
        companyStructureEntity.MoreInformation!.Information.Should().Be(newMoreInformation);
        companyStructureRepositoryMock.Verify(x =>
            x.SaveAsync(It.Is<CompanyStructureEntity>(y => y.MoreInformation!.Information == newMoreInformation), userAccount, CancellationToken.None));
    }
}
