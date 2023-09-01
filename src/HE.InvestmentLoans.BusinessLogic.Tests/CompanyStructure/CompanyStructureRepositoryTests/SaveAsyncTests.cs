using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.User;
using HE.InvestmentLoans.Common.Tests.TestFramework;
using HE.InvestmentLoans.CRM.Model;
using Moq;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.CompanyStructureRepositoryTests;

public class SaveAsyncTests : TestBase<CompanyStructureRepository>
{
    [Fact]
    public async Task ShouldSaveCompanyStructureEntityWithAllAnswersAndSectionInProgressStatus()
    {
        // given
        var companyStructureEntity = CompanyStructureEntityTestBuilder
            .New()
            .WithHomesBuild()
            .WithCompanyPurpose()
            .WithMoreInformation()
            .Build();

        var userAccount = UserAccountTestData.UserAccountOne;

        var organizationServiceMock = OrganizationServiceAsyncMockTestBuilder.New().BuildMock();

        RegisterDependency(organizationServiceMock);

        // when
        await TestCandidate.SaveAsync(companyStructureEntity, userAccount, CancellationToken.None);

        // then
        organizationServiceMock
            .Verify(
                x =>
                    x.ExecuteAsync(
                        It.Is<invln_updatesingleloanapplicationRequest>(y =>
                            y.invln_accountid == userAccount.AccountId.ToString() &&
                            y.invln_loanapplicationid == companyStructureEntity.LoanApplicationId.ToString() &&
                            y.invln_loanapplication.Contains($"companyPurpose\":{companyStructureEntity.Purpose}") &&
                            y.invln_loanapplication.Contains($"existingCompany\":\"{companyStructureEntity.MoreInformation!.Information}\"") &&
                            y.invln_loanapplication.Contains($"companyExperience\":{companyStructureEntity.HomesBuilt!.Value}") &&
                            y.invln_loanapplication.Contains($"CompanyStructureAndExperienceCompletionStatus\":858110001")),
                        CancellationToken.None),
                Times.Once);
    }
}
