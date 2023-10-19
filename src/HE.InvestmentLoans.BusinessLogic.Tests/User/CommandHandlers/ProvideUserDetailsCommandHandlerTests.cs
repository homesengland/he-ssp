using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.BusinessLogic.User.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Contract.User.Commands;
using HE.InvestmentLoans.Contract.User.ValueObjects;
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
        var userDetails = UserDetailsEntityTestBuilder.New().Build();

        RegisterDependency(LoanUserContextTestBuilder.New().ReturnUserDetails(userDetails).Build());

        var userRepositoryMock = UserRepositoryTestBuilder
            .New()
            .ReturnUserDetailsEntity(userAccount.UserGlobalId, userDetails)
            .BuildMockAndRegister(this);

        var newUserDetails = new UserDetails(
            FirstName.FromString("Jacob"),
            LastName.FromString("Smith"),
            JobTitle.FromString("Developer"),
            "john.smith@test.com",
            TelephoneNumber.FromString("12345678"),
            TelephoneNumber.FromString("87654321", nameof(UserDetails.SecondaryTelephoneNumber)),
            false);

        // when
        await TestCandidate.Handle(
            new ProvideUserDetailsCommand(
                newUserDetails.FirstName!.ToString(),
                newUserDetails.LastName!.ToString(),
                newUserDetails.JobTitle!.ToString(),
                newUserDetails.TelephoneNumber!.ToString(),
                newUserDetails.SecondaryTelephoneNumber!.ToString()),
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
