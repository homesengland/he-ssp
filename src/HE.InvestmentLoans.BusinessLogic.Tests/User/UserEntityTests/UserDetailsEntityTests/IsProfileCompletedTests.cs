using HE.InvestmentLoans.BusinessLogic.Tests.Assertions;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Messages;
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
    public void ShouldThrowDomainValidationException_WhenJobTitleNameAreProvided()
    {
        // given
        var userDetailsEntity = UserDetailsEntityTestBuilder.New().Build();

        var firstName = "John";
        var lastName = "Smith";
        var jobTitle = string.Empty;
        var telephoneNumber = "123123";
        var secondaryTelephoneNumber = "678678";
        var userEmail = "john.smith@test.com";

        // when
        var action = () => userDetailsEntity.ProvideUserDetails(firstName, lastName, jobTitle, telephoneNumber, secondaryTelephoneNumber, userEmail);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.EnterJobTitle);
    }
}
