using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Contract.User.ValueObjects;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.User.UserEntityTests.UserDetailsEntityTests;
public class ProvideUserDetailsTests
{
    [Fact]
    public void ShouldChangeUserDetails_WhenProvideCorrectData()
    {
        // given
        var userDetailsEntity = UserDetailsEntityTestBuilder.New().Build();

        var newUserDetails = new UserDetails(
            FirstName.New("Jacob"),
            LastName.New("Smith"),
            JobTitle.New("Developer"),
            "john.smith@test.com",
            TelephoneNumber.New("12345678"),
            SecondaryTelephoneNumber.New("87654321"),
            false);

        // when
        userDetailsEntity.ProvideUserDetails(
            newUserDetails.FirstName!.ToString(),
            newUserDetails.LastName!.ToString(),
            newUserDetails.JobTitle!.ToString(),
            newUserDetails.TelephoneNumber!.ToString(),
            newUserDetails.SecondaryTelephoneNumber!.ToString(),
            newUserDetails.Email!);

        // then
        userDetailsEntity.FirstName.Should().Be(newUserDetails.FirstName);
        userDetailsEntity.LastName.Should().Be(newUserDetails.LastName);
        userDetailsEntity.JobTitle.Should().Be(newUserDetails.JobTitle);
        userDetailsEntity.TelephoneNumber.Should().Be(newUserDetails.TelephoneNumber);
        userDetailsEntity.SecondaryTelephoneNumber.Should().Be(newUserDetails.SecondaryTelephoneNumber);
        userDetailsEntity.Email.Should().Be(newUserDetails.Email);
    }
}
