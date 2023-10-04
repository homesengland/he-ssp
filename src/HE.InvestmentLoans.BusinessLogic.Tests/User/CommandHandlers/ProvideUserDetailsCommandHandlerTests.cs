using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.BusinessLogic.User.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Contract.User.Commands;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.User.CommandHandlers;
public class ProvideUserDetailsCommandHandlerTests : TestBase<ProvideUserDetailsCommandHandler>
{
    [Fact]
    public async Task ShouldUpdateUserDetails_WhenProvidedCorrectData()
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

        var userRepositoryMock = UserRepositoryTestBuilder
            .New()
            .ReturnUserDetailsEntity(userAccount.UserGlobalId, userDetails)
            .BuildMockAndRegister(this);

        var newUserDetails = new UserDetails("Jacob", "Smith", "Developer", "john.smith@test.com", "12345678", "87654321", false);

        // when
        await TestCandidate.Handle(
            new ProvideUserDetailsCommand(
                newUserDetails.FirstName!,
                newUserDetails.LastName!,
                newUserDetails.JobTitle!,
                newUserDetails.TelephoneNumber!,
                newUserDetails.SecondaryTelephoneNumber!),
            CancellationToken.None);

        // then
        userDetails.FirstName.Should().Be(newUserDetails.FirstName);
        userDetails.LastName.Should().Be(newUserDetails.LastName);
        userDetails.JobTitle.Should().Be(newUserDetails.JobTitle);
        userDetails.TelephoneNumber.Should().Be(newUserDetails.TelephoneNumber);
        userDetails.SecondaryTelephoneNumber.Should().Be(newUserDetails.SecondaryTelephoneNumber);
        userRepositoryMock.Verify(x =>
            x.SaveAsync(
                It.Is<UserDetails>(
                y => y.FirstName! == newUserDetails.FirstName
                && y.LastName! == newUserDetails.LastName
                && y.JobTitle! == newUserDetails.JobTitle
                && y.TelephoneNumber! == newUserDetails.TelephoneNumber
                && y.SecondaryTelephoneNumber! == newUserDetails.SecondaryTelephoneNumber),
                userAccount.UserGlobalId,
                CancellationToken.None));
    }
}
