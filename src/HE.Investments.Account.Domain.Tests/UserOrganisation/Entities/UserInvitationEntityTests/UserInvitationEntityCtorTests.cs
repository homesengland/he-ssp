using FluentAssertions;
using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Domain.UserOrganisation.Entities;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Exceptions;
using Xunit;

namespace HE.Investments.Account.Domain.Tests.UserOrganisation.Entities.UserInvitationEntityTests;

public class UserInvitationEntityCtorTests
{
    [Theory]
    [InlineData(null)]
    [InlineData(UserRole.Undefined)]
    [InlineData(UserRole.Admin)]
    [InlineData(UserRole.Limited)]
    public void ShouldThrowException_WhenRoleIsNotAllowed(UserRole? role)
    {
        // given
        var firstName = new FirstName("John");
        var lastName = new LastName("Paul");
        var emailAddress = new EmailAddress("john.paul@engineering.com");
        var jobTitle = new JobTitle("Engineer");

        // when
        var create = () => new UserInvitationEntity(firstName, lastName, emailAddress, jobTitle, role);

        // then
        create.Should().Throw<DomainValidationException>();
    }

    [Theory]
    [InlineData(UserRole.Enhanced)]
    [InlineData(UserRole.Input)]
    [InlineData(UserRole.ViewOnly)]
    public void ShouldCreateEntity_WhenRoleIsAllowed(UserRole role)
    {
        // given
        var firstName = new FirstName("John");
        var lastName = new LastName("Paul");
        var emailAddress = new EmailAddress("john.paul@engineering.com");
        var jobTitle = new JobTitle("Engineer");

        // when
        var result = new UserInvitationEntity(firstName, lastName, emailAddress, jobTitle, role);

        // then
        result.FirstName.Should().Be(firstName);
        result.LastName.Should().Be(lastName);
        result.Email.Should().Be(emailAddress);
        result.JobTitle.Should().Be(jobTitle);
        result.Role.Should().Be(role);
    }
}
