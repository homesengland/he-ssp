using HE.InvestmentLoans.BusinessLogic.LoanApplication.QueryHandlers;
using HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.Contract.Application.Queries;
using HE.Investments.Account.Domain.Tests.User.TestObjectBuilder;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.QueryHandlers;

public class GetApplicationDashboardQueryHandlerTests : TestBase<GetApplicationDashboardQueryHandler>
{
    [Fact]
    public async Task ShouldReturnApplicationDashboardData()
    {
        // given
        var loanApplicationId = LoanApplicationIdTestData.LoanApplicationIdOne;

        var userAccount = AccountUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        var loanApplicationEntity = LoanApplicationTestBuilder.NewDraft(userAccount).Build();

        LoanApplicationRepositoryTestBuilder
            .New()
            .ReturnLoanApplication(loanApplicationId, userAccount, loanApplicationEntity)
            .BuildMockAndRegister(this);

        // when
        var result = await TestCandidate.Handle(new GetApplicationDashboardQuery(loanApplicationId), CancellationToken.None);

        // then
        result.ApplicationId.Should().Be(loanApplicationId);
        result.ApplicationName.Should().Be(loanApplicationEntity.Name);
        result.ApplicationStatus.Should().Be(loanApplicationEntity.ExternalStatus);
        result.ApplicationReferenceNumber.Should().Be(loanApplicationEntity.ReferenceNumber);
        result.LastEditedOn.Should().Be(loanApplicationEntity.LastModificationDate);
        result.OrganizationName.Should().Be(userAccount.AccountName);
    }
}
