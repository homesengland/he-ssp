using HE.InvestmentLoans.BusinessLogic.LoanApplication.Entities;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.QueryHandlers;
using HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.Common.Tests.TestData;
using HE.InvestmentLoans.Contract.Application.Queries;
using HE.Investments.Account.Domain.Tests.User.TestObjectBuilder;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace HE.InvestmentLoans.BusinessLogic.Tests.LoanApplication.QueryHandlers;

public class GetDashboardDataQueryHandlerTests : TestBase<GetDashboardDataQueryHandler>
{
    [Fact]
    public async Task ShouldReturnOrderedApplicationLoans()
    {
        // given
        var userAccount = AccountUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        var firstDate = DateTimeTestData.OctoberDay05Year2023At0858;

        var loanApplicationOne = LoanApplicationTestBuilder.NewDraft(userAccount).WithCreatedOn(firstDate.AddMinutes(1)).Build();
        var loanApplicationTwo = LoanApplicationTestBuilder.NewDraft(userAccount).WithCreatedOn(firstDate.AddMinutes(2)).Build();
        var loanApplicationThree = LoanApplicationTestBuilder.NewDraft(userAccount).WithCreatedOn(firstDate).Build();

        LoanApplicationRepositoryTestBuilder
            .New()
            .ReturnLoanApplications(userAccount, new List<LoanApplicationEntity> { loanApplicationOne, loanApplicationTwo, loanApplicationThree })
            .BuildMockAndRegister(this);

        // when
        var result = await TestCandidate.Handle(new GetDashboardDataQuery(), CancellationToken.None);

        // then
        result.LoanApplications.Should().HaveCount(3);
        result.LoanApplications[0].Id.Should().Be(loanApplicationTwo.Id);
        result.LoanApplications[1].Id.Should().Be(loanApplicationOne.Id);
        result.LoanApplications[2].Id.Should().Be(loanApplicationThree.Id);
    }

    [Fact]
    public async Task ShouldReturnEmptyList_WhenNoLoanApplications()
    {
        // given
        var userAccount = AccountUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        LoanApplicationRepositoryTestBuilder
            .New()
            .ReturnLoanApplications(userAccount, Array.Empty<LoanApplicationEntity>())
            .BuildMockAndRegister(this);

        // when
        var result = await TestCandidate.Handle(new GetDashboardDataQuery(), CancellationToken.None);

        // then
        result.LoanApplications.Should().BeEmpty();
    }
}
