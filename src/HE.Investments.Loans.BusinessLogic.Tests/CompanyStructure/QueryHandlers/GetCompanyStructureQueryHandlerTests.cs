using System.Globalization;
using HE.Investments.Common.Tests.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.QueryHandlers;
using HE.Investments.Loans.BusinessLogic.Tests.CompanyStructure.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;
using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.CompanyStructure.Queries;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.CompanyStructure.QueryHandlers;

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
