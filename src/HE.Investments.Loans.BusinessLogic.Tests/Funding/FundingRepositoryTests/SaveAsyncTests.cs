using HE.Investments.Common.CRM.Model;
using HE.Investments.Loans.BusinessLogic.Funding.Repositories;
using HE.Investments.Loans.BusinessLogic.Tests.Funding.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Tests.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Tests.User.TestData;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Funding.FundingRepositoryTests;

public class SaveAsyncTests : TestBase<FundingRepository>
{
    [Fact]
    public async Task ShouldSaveFundingEntityWithAllAnswersAndSectionInProgressStatus()
    {
        // given
        var fundingEntity = FundingEntityTestBuilder
            .New()
            .WithAllDataProvided()
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
                    y.invln_loanapplication.Contains($"projectEstimatedTotalCost\":{fundingEntity.EstimatedTotalCosts!.Value}") &&
                    y.invln_loanapplication.Contains($"projectAbnormalCosts\":{fundingEntity.AbnormalCosts!.IsAnyAbnormalCost.ToString().ToLowerInvariant()}") &&
                    y.invln_loanapplication.Contains($"projectAbnormalCostsInformation\":\"{fundingEntity.AbnormalCosts!.AbnormalCostsAdditionalInformation}\"") &&
                    y.invln_loanapplication.Contains($"privateSectorApproach\":{fundingEntity.PrivateSectorFunding!.IsApplied.ToString().ToLowerInvariant()}") &&
                    y.invln_loanapplication.Contains($"privateSectorApproachInformation\":\"{fundingEntity.PrivateSectorFunding.PrivateSectorFundingNotApplyingReason}\"") &&
                    y.invln_loanapplication.Contains($"additionalProjects\":{fundingEntity.AdditionalProjects!.IsThereAnyAdditionalProject.ToString().ToLowerInvariant()}") &&
                    y.invln_loanapplication.Contains($"refinanceRepayment\":\"{fundingEntity.RepaymentSystem!.Refinance!.Value}\"") &&
                    y.invln_loanapplication.Contains($"refinanceRepaymentDetails\":\"{fundingEntity.RepaymentSystem!.Refinance!.AdditionalInformation}\"") &&
                    y.invln_loanapplication.Contains($"FundingDetailsCompletionStatus\":858110001")),
                CancellationToken.None),
        Times.Once);
    }
}
