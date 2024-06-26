using HE.Investments.Common.Contract;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.Tests.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;
using HE.Investments.Loans.BusinessLogic.Tests.TestObjectBuilders;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.LoanApplicationRepositoryTests;

public class ChangeApplicationStatusTests : TestBase<LoanApplicationRepository>
{
    [Theory]
    [InlineData(ApplicationStatus.ReferredBackToApplicant)]
    [InlineData(ApplicationStatus.Draft)]
    [InlineData(ApplicationStatus.ConditionsSatisfied)]
    [InlineData(ApplicationStatus.ApplicationUnderReview)]
    [InlineData(ApplicationStatus.ApprovedSubjectToContract)]
    [InlineData(ApplicationStatus.UnderReview)]
    public async Task ShouldChangeApplicationStatus(ApplicationStatus applicationStatus)
    {
        // given
        var loanApplicationId = LoanApplicationIdTestData.LoanApplicationIdOne;

        var userAccount = AccountUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        var loanApplicationEntity = LoanApplicationTestBuilder.NewDraft(userAccount).Build();

        var organizationServiceMock = OrganizationServiceAsyncMockTestBuilder.New().BuildMock();

        RegisterDependency(organizationServiceMock);

        var crmStatus = LoanApplicationStatusMapper.MapToCrmStatus(applicationStatus);

        // when
        await TestCandidate.ChangeApplicationStatus(loanApplicationId, applicationStatus, CancellationToken.None);

        // then
        organizationServiceMock
            .Verify(
                x =>
                    x.ExecuteAsync(
                        It.Is<invln_changeloanapplicationexternalstatusRequest>(y =>
                            y.invln_loanapplicationid == loanApplicationEntity.Id.ToString() &&
                            y.invln_statusexternal == crmStatus),
                        CancellationToken.None),
                Times.Once);
    }
}
