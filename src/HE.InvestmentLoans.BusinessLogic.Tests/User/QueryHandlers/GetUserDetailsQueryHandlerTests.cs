using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.BusinessLogic.User.QueryHandlers;
using HE.InvestmentLoans.Common.Tests.TestFramework;
using HE.InvestmentLoans.Contract.User.Queries;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.User.QueryHandlers;
public class GetUserDetailsQueryHandlerTests : TestBase<GetUserDetailsQueryHandler>
{
    [Fact]
    public async Task ShouldReturnUserDetails()
    {
        // given
        var userAccount = LoanUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        var userDetails = LoanUserContextTestBuilder
            .New()
            .Register(this)
            .UserDetailsFromMock;

        UserRepositoryTestBuilder
            .New()
            .ReturnUserDetailsEntity(userAccount.UserGlobalId, userDetails)
            .BuildMockAndRegister(this);

        // when
        var result = await TestCandidate.Handle(new GetUserDetailsQuery(), CancellationToken.None);

        // then
        result.ViewModel.FirstName.Should().Be(userDetails.FirstName?.ToString());
        result.ViewModel.LastName.Should().Be(userDetails.LastName?.ToString());
        result.ViewModel.JobTitle.Should().Be(userDetails.JobTitle?.ToString());
        result.ViewModel.TelephoneNumber.Should().Be(userDetails.TelephoneNumber?.ToString());
        result.ViewModel.SecondaryTelephoneNumber.Should().Be(userDetails.SecondaryTelephoneNumber?.ToString());
    }
}
