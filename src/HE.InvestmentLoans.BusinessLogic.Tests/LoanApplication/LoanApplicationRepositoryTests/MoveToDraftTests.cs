using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories.Mapper;
using HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.TestObjectBuilders;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.Investments.Account.Domain.Tests.User.TestObjectBuilder;
using HE.Investments.Common.CRM.Model;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.LoanApplicationRepositoryTests;

public class MoveToDraftTests : TestBase<LoanApplicationRepository>
{
    [Fact]
    public async Task ShouldChangeLoanApplicationStatusToDraft()
    {
        // given
        var loanApplicationId = LoanApplicationIdTestData.LoanApplicationIdOne;

        var userAccount = AccountUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        var loanApplicationEntity = LoanApplicationTestBuilder.NewSubmitted(userAccount).Build();

        var organizationServiceMock = OrganizationServiceAsyncMockTestBuilder.New().BuildMock();

        RegisterDependency(organizationServiceMock);

        var crmDraftStatus = ApplicationStatusMapper.MapToCrmStatus(ApplicationStatus.Draft);

        // when
        await TestCandidate.MoveToDraft(loanApplicationId, CancellationToken.None);

        // then
        organizationServiceMock
            .Verify(
                x =>
                    x.ExecuteAsync(
                        It.Is<invln_changeloanapplicationexternalstatusRequest>(y =>
                            y.invln_loanapplicationid == loanApplicationEntity.Id.ToString() &&
                            y.invln_statusexternal == crmDraftStatus),
                        CancellationToken.None),
                Times.Once);
    }
}
