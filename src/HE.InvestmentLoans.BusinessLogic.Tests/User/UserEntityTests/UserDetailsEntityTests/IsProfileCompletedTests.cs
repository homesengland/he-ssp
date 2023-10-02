using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Contract.User.ValueObjects;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.User.UserEntityTests.UserDetailsEntityTests;
public class IsProfileCompletedTests
{
    [Fact]
    public void ShouldReturnTrue_WhenAllUserDetailsAreProvided()
    {
        // given
        var userDetailsEntity = UserDetailsEntityTestBuilder.New().Build();

        // when
        var result = userDetailsEntity.IsProfileCompleted();

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnFalse_WhenJobTitleNameAreProvided()
    {
        // given
        var userDetails = new UserDetails(
            FirstName.New("John"),
            LastName.New("Smith"),
            JobTitle.New(string.Empty),
            "john.smith@test.com",
            TelephoneNumber.New("12345678"),
            SecondaryTelephoneNumber.New("87654321"),
            false);

        // when
        var result = userDetails.IsProfileCompleted();

        // then
        result.Should().BeFalse();
    }
}
