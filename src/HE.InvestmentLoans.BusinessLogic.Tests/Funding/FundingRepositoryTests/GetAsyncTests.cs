using HE.InvestmentLoans.BusinessLogic.Funding.Repositories;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestData;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Constants.ViewName;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Funding.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;
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
        var result = await TestCandidate.GetAsync(loanApplicationId, UserAccountTestData.UserAccountOne, FundingViewOption.GetAllFields,CancellationToken.None);

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
        var result = await TestCandidate.GetAsync(loanApplicationId, UserAccountTestData.UserAccountOne, FundingViewOption.GetAllFields, CancellationToken.None);

        // then
        result.LoanApplicationId.Should().Be(loanApplicationId);
        result.GrossDevelopmentValue.Should().Be(GrossDevelopmentValue.New(5.23m));
        result.EstimatedTotalCosts.Should().Be(EstimatedTotalCosts.New(10));
        result.AbnormalCosts.Should().Be(AbnormalCosts.New(true, "Short description"));
        result.PrivateSectorFunding.Should().Be(PrivateSectorFunding.New(true, "Short description", null));
        result.RepaymentSystem.Should().Be(RepaymentSystem.New("refinance", "Short description"));
        result.AdditionalProjects.Should().Be(AdditionalProjects.New(true));
        result.Status.Should().Be(SectionStatus.InProgress);
    }

    [Fact]
    public async Task ShouldThrowNotFoundException_WhenApplicationLoanDoesNotExist()
    {
        // given
        var loanApplicationId = LoanApplicationIdTestData.LoanApplicationIdOne;

        // when
        var action = () => TestCandidate.GetAsync(loanApplicationId, UserAccountTestData.UserAccountOne, FundingViewOption.GetAllFields, CancellationToken.None);

        // then
        await action.Should().ThrowExactlyAsync<NotFoundException>().WithMessage($"*{loanApplicationId}*");
    }
}
