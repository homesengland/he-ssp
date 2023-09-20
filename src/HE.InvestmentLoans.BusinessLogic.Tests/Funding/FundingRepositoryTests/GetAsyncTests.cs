using HE.InvestmentLoans.BusinessLogic.CompanyStructure;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.Funding.Repositories;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestData;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Tests.TestFramework;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Funding.FundingRepositoryTests;

public class GetAsyncTests : TestBase<FundingRepository>
{
    [Fact]
    public async Task ShouldReturnFundingEntityWithoutAnyAnswers()
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
        var result = await TestCandidate.GetAsync(loanApplicationId, UserAccountTestData.UserAccountOne, CancellationToken.None);

        // then
        result.LoanApplicationId.Should().Be(loanApplicationId);
        result.GrossDevelopmentValue.Should().BeNull();
        result.EstimatedTotalCosts.Should().BeNull();
        result.AbnormalCosts.Should().BeNull();
        result.PrivateSectorFunding.Should().BeNull();
        result.RepaymentSystem.Should().BeNull();
        result.AdditionalProjects.Should().BeNull();
        result.Status.Should().Be(SectionStatus.NotStarted);
    }

    [Fact]
    public async Task ShouldReturnFundingEntityWithAllAnserws()
    {
        // given
        var loanApplicationId = LoanApplicationIdTestData.LoanApplicationIdOne;

        var organizationServiceMock = OrganizationServiceAsyncMockTestBuilder
            .New()
            .MockGetSingleLoanApplicationForAccountAndContactRequest(
                loanApplicationId,
                GetSingleLoanApplicationForAccountAndContactResponseTestData.ResponseWithFundingFields)
            .Build();

        RegisterDependency(organizationServiceMock);

        // when
        var result = await TestCandidate.GetAsync(loanApplicationId, UserAccountTestData.UserAccountOne, CancellationToken.None);

        // then
        result.LoanApplicationId.Should().Be(loanApplicationId);
        result.GrossDevelopmentValue.Should().Be(CompanyPurpose.New(true));
        result.MoreInformation.Should().Be(new OrganisationMoreInformation("Short description"));
        result.MoreInformationFile.Should().BeNull();
        result.HomesBuilt.Should().Be(new HomesBuilt(5));
        result.HomesBuilt.Should().Be(new HomesBuilt(5));
        result.HomesBuilt.Should().Be(new HomesBuilt(5));
        result.HomesBuilt.Should().Be(new HomesBuilt(5));
        result.HomesBuilt.Should().Be(new HomesBuilt(5));
        result.HomesBuilt.Should().Be(new HomesBuilt(5));
        result.Status.Should().Be(SectionStatus.InProgress);
    }

    [Fact]
    public async Task ShouldThrowNotFoundException_WhenApplicationLoanDoesNotExist()
    {
        // given
        var loanApplicationId = LoanApplicationIdTestData.LoanApplicationIdOne;

        // when
        var action = () => TestCandidate.GetAsync(loanApplicationId, UserAccountTestData.UserAccountOne, CancellationToken.None);

        // then
        await action.Should().ThrowExactlyAsync<NotFoundException>().WithMessage($"*{loanApplicationId}*");
    }
}
