using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories.Mapper;
using HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.CRM.Model;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.LoanApplicationRepositoryTests;

public class WithdrawDraftTests : TestBase<LoanApplicationRepository>
{
    [Fact]
    public async Task ShouldWithdrawLoanApplicationInDraftStatus()
    {
        // given
        var loanApplicationId = LoanApplicationIdTestData.LoanApplicationIdOne;

        var userAccount = LoanUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        var loanApplicationEntity = LoanApplicationTestBuilder.NewDraft(userAccount).Build();

        var organizationServiceMock = OrganizationServiceAsyncMockTestBuilder.New().BuildMock();

        RegisterDependency(organizationServiceMock);

        var withdrawReason = WithdrawReasonTestData.WithdrawReasonOne;
        var crmRemoveStatus = ApplicationStatusMapper.MapToCrmStatus(ApplicationStatus.NA);

        // when
        await TestCandidate.WithdrawDraft(loanApplicationId, withdrawReason, CancellationToken.None);

        // then
        organizationServiceMock
            .Verify(
                x =>
                    x.ExecuteAsync(
                        It.Is<invln_changeloanapplicationexternalstatusRequest>(y =>
                            y.invln_loanapplicationid == loanApplicationEntity.Id.ToString() &&
                            y.invln_statusexternal == crmRemoveStatus &&
                            y.invln_changereason == withdrawReason.ToString()),
                        CancellationToken.None),
                Times.Once);
    }
}
