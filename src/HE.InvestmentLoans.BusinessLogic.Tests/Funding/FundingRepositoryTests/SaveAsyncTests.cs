using HE.InvestmentLoans.BusinessLogic.Funding.Repositories;
using HE.InvestmentLoans.BusinessLogic.Tests.Funding.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestData;
using HE.InvestmentLoans.Common.Tests.TestFramework;
using HE.InvestmentLoans.CRM.Model;
using Moq;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Funding.FundingRepositoryTests;

public class SaveAsyncTests : TestBase<FundingRepository>
{
    [Fact]
    public async Task ShouldSaveFundingEntityWithAllAnswersAndSectionInProgressStatus()
    {
        // given
        var fundingEntity = FundingEntityTestBuilder
            .New()
            .WithGrossDevelopmentValue()
            .WithEstimatedTotalCosts()
            .WithAbnormalCosts()
            .WithPrivateSectorFunding()
            .WithRepaymentSystem()
            .WithAdditionalProjects()
            .Build();

        var userAccount = UserAccountTestData.UserAccountOne;

        var organizationServiceMock = OrganizationServiceAsyncMockTestBuilder.New().BuildMock();

        RegisterDependency(organizationServiceMock);

        // when
        await TestCandidate.SaveAsync(fundingEntity, userAccount, CancellationToken.None);

        // then
        organizationServiceMock
            .Verify(
                x =>
                    x.ExecuteAsync(
                        It.Is<invln_updatesingleloanapplicationRequest>(y =>
                            y.invln_accountid == userAccount.AccountId.ToString() &&
                            y.invln_loanapplicationid == fundingEntity.LoanApplicationId.ToString() &&
                            y.invln_loanapplication.Contains($"projectGdv\":{fundingEntity.GrossDevelopmentValue!.Value}") &&
                            y.invln_loanapplication.Contains($"projectEstimatedTotalCost\":\"{fundingEntity.EstimatedTotalCosts!.Value}\"") &&
                            y.invln_loanapplication.Contains($"projectAbnormalCosts\":{fundingEntity.AbnormalCosts!.IsAnyAbnormalCost}") &&
                            y.invln_loanapplication.Contains($"projectAbnormalCostsInformation\":{fundingEntity.AbnormalCosts!.AbnormalCostsAdditionalInformation}") &&
                            y.invln_loanapplication.Contains($"privateSectorApproach\":{fundingEntity.PrivateSectorFunding!.IsApplied}") &&
                            y.invln_loanapplication.Contains($"privateSectorApproachInformation\":{fundingEntity.PrivateSectorFunding!.PrivateSectorFundingApplyResult}") &&
                            y.invln_loanapplication.Contains($"additionalProjects\":{fundingEntity.AdditionalProjects!.IsThereAnyAdditionalProject}") &&
                            y.invln_loanapplication.Contains($"refinanceRepayment\":{fundingEntity.RepaymentSystem!.Refinance!.Value}") &&
                            y.invln_loanapplication.Contains($"refinanceRepaymentDetails\":{fundingEntity.RepaymentSystem!.Refinance!.AdditionalInformation}") &&
                            y.invln_loanapplication.Contains($"FundingDetailsCompletionStatus\":858110001")),
                        CancellationToken.None),
                Times.Once);
    }
}
