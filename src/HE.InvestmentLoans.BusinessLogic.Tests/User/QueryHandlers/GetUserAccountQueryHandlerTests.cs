using HE.InvestmentLoans.BusinessLogic.Tests.User.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.BusinessLogic.User.QueryHandlers;
using HE.InvestmentLoans.Contract.User.Queries;
using HE.Investments.Account.Domain.Tests.User.TestObjectBuilder;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.User.QueryHandlers;
public class GetUserAccountQueryHandlerTests : TestBase<GetUserAccountQueryHandler>
{
    [Fact]
    public async Task ShouldReturnUserAccount()
    {
        // given
        var userAccount = AccountUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        var userDetails = UserDetailsTestData.UserDetailsOne;

        AccountUserContextTestBuilder
            .New()
            .ReturnUserAccount(userAccount)
            .ReturnProfileDetails(userDetails)
            .Register(this);

        // when
        var result = await TestCandidate.Handle(new GetUserAccountQuery(), CancellationToken.None);

        // then
        result.Email.Should().Be(userAccount.UserEmail);
        result.UserGlobalId.Should().Be(userAccount.UserGlobalId.ToString());
        result.SelectedAccountId.Should().Be(userAccount.AccountId);
        result.FirstName.Should().Be(userDetails.FirstName?.ToString());
        result.LastName.Should().Be(userDetails.LastName?.ToString());
        result.TelephoneNumber.Should().Be(userDetails.TelephoneNumber?.ToString());
    }
}
