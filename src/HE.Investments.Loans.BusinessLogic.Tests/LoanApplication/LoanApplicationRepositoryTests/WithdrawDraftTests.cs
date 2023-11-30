using HE.Investments.Account.Domain.Tests.User.TestObjectBuilder;
using HE.Investments.Common.Contract;
using HE.Investments.Common.CRM;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.TestData;
using HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.TestObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;
using HE.Investments.Loans.BusinessLogic.Tests.TestObjectBuilders;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.LoanApplicationRepositoryTests;

public class WithdrawDraftTests : TestBase<LoanApplicationRepository>
{
    [Fact]
    public async Task ShouldWithdrawLoanApplicationInDraftStatus()
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
