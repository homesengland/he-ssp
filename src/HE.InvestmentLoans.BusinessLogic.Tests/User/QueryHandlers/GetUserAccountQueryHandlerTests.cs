using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.BusinessLogic.User.QueryHandlers;
using HE.InvestmentLoans.Common.Tests.TestFramework;
using HE.InvestmentLoans.Contract.User.Queries;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.User.QueryHandlers;
public class GetUserAccountQueryHandlerTests : TestBase<GetUserAccountQueryHandler>
{
    [Fact]
    public async Task ShouldReturnUserAccount()
    {
        // given
        var userAccount = LoanUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        LoanUserContextTestBuilder
            .New()
            .ReturnUserAccount(userAccount)
            .Register(this);

        // when
        var result = await TestCandidate.Handle(new GetUserAccountQuery(), CancellationToken.None);

        // then
        result.Email.Should().Be(userAccount.UserEmail);
        result.UserGlobalId.Should().Be(userAccount.UserGlobalId.ToString());
        result.SelectedAccountId.Should().Be(userAccount.AccountId);
        result.FirstName.Should().Be(userAccount.FirstName);
        result.LastName.Should().Be(userAccount.LastName);
        result.TelephoneNumber.Should().Be(userAccount.TelephoneNumber);
    }
}
