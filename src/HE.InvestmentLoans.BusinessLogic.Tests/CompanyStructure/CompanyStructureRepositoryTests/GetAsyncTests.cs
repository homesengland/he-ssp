using HE.InvestmentLoans.BusinessLogic.CompanyStructure;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestData;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.CompanyStructureRepositoryTests;

public class GetAsyncTests : TestBase<CompanyStructureRepository>
{
    [Fact]
    public async Task ShouldReturnCompanyStructureEntityWithoutAnyAnswers()
    {
        // given
        var loanApplicationId = LoanApplicationIdTestData.LoanApplicationIdOne;

        var organizationServiceMock = OrganizationServiceAsyncMockTestBuilder
            .New()
            .MockGetSingleLoanApplicationForAccountAndContactRequest(
                loanApplicationId,
                GetSingleLoanApplicationForAccountAndContactResponseTestData.EmptyResponse)
            .Build();

        RegisterDependency(organizationServiceMock);

        // when
        var result = await TestCandidate.GetAsync(
            loanApplicationId,
            UserAccountTestData.UserAccountOne,
            CompanyStructureFieldsSet.GetAllFields,
            CancellationToken.None);

        // then
        result.LoanApplicationId.Should().Be(loanApplicationId);
        result.Purpose.Should().BeNull();
        result.MoreInformation.Should().BeNull();
        result.MoreInformationFiles.Should().BeNull();
        result.HomesBuilt.Should().BeNull();
        result.Status.Should().Be(SectionStatus.NotStarted);
    }

    [Fact]
    public async Task ShouldReturnCompanyStructureEntityWithAllAnserws()
    {
        // given
        var loanApplicationId = LoanApplicationIdTestData.LoanApplicationIdOne;

        var organizationServiceMock = OrganizationServiceAsyncMockTestBuilder
            .New()
            .MockGetSingleLoanApplicationForAccountAndContactRequest(
                loanApplicationId,
                GetSingleLoanApplicationForAccountAndContactResponseTestData.ResponseWithCompanyStructureFields)
            .Build();

        RegisterDependency(organizationServiceMock);

        // when
        var result = await TestCandidate.GetAsync(
            loanApplicationId,
            UserAccountTestData.UserAccountOne,
            CompanyStructureFieldsSet.GetAllFields,
            CancellationToken.None);

        // then
        result.LoanApplicationId.Should().Be(loanApplicationId);
        result.Purpose.Should().Be(CompanyPurpose.New(true));
        result.MoreInformation.Should().Be(new OrganisationMoreInformation("Short description"));
        result.MoreInformationFiles.Should().BeNull();
        result.HomesBuilt.Should().Be(new HomesBuilt(5));
        result.Status.Should().Be(SectionStatus.InProgress);
    }

    [Fact]
    public async Task ShouldThrowNotFoundException_WhenApplicationLoanDoesNotExist()
    {
        // given
        var loanApplicationId = LoanApplicationIdTestData.LoanApplicationIdOne;

        // when
        var action = () =>
            TestCandidate.GetAsync(loanApplicationId, UserAccountTestData.UserAccountOne, CompanyStructureFieldsSet.GetAllFields, CancellationToken.None);

        // then
        await action.Should().ThrowExactlyAsync<NotFoundException>().WithMessage($"*{loanApplicationId}*");
    }
}
