using System.Globalization;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure.QueryHandlers;
using HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Contract.CompanyStructure.Queries;
using HE.Investments.Account.Domain.Tests.User.TestObjectBuilder;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.QueryHandlers;

public class GetCompanyStructureQueryHandlerTests : TestBase<GetCompanyStructureQueryHandler>
{
    [Fact]
    public async Task ShouldReturnCompanyStructure()
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
            new GetCompanyStructureQuery(loanApplicationId, CompanyStructureFieldsSet.GetAllFields),
            CancellationToken.None);

        // then
        result.ViewModel.Purpose.Should().Be("Yes");
        result.ViewModel.OrganisationMoreInformation.Should().Be(companyStructureEntity.MoreInformation!.Information);
        result.ViewModel.HomesBuilt.Should().Be(companyStructureEntity.HomesBuilt!.Value.ToString(CultureInfo.InvariantCulture));
    }
}
