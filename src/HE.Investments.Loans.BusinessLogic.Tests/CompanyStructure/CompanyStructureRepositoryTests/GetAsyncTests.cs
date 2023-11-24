using HE.Investments.Common.Domain;
using HE.Investments.Loans.BusinessLogic.CompanyStructure;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Repositories;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;
using HE.Investments.Loans.BusinessLogic.Tests.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Tests.User.TestData;
using HE.Investments.Loans.Common.Exceptions;
using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.CompanyStructure.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.CompanyStructure.CompanyStructureRepositoryTests;

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
