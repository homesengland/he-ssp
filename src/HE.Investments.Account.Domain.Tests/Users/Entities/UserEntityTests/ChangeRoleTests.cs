using FluentAssertions;
using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Account.Domain.Users.Entities;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using Xunit;

namespace HE.Investments.Account.Domain.Tests.Users.Entities.UserEntityTests;

public class ChangeRoleTests
{
    [Theory]
    [InlineData(UserRole.Enhanced)]
    [InlineData(UserRole.Input)]
    [InlineData(UserRole.ViewOnly)]
    public void ShouldAssignNewRole(UserRole role)
    {
        // given
        var entity = CreateTestUser();

        // when
        entity.ChangeRole(role);

        // then
        entity.Role.Should().Be(role);
        entity.IsRoleModified.Should().BeTrue();
    }

    [Theory]
    [InlineData(UserRole.Enhanced)]
    [InlineData(UserRole.Input)]
    [InlineData(UserRole.ViewOnly)]
    public void ShouldNotAssignNewRole_WhenRoleAlreadyAssigned(UserRole role)
    {
        // given
        var entity = CreateTestUser(role);

        // when
        entity.ChangeRole(role);

        // then
        entity.Role.Should().Be(role);
        entity.IsRoleModified.Should().BeFalse();
    }

    [Theory]
    [InlineData(UserRole.Undefined)]
    [InlineData(UserRole.Admin)]
    [InlineData(UserRole.Limited)]
    public void ShouldThrowException_WhenRoleCannotBeAssigned(UserRole role)
    {
        // given
        var entity = CreateTestUser();

        // when
        var result = () => entity.ChangeRole(role);

        // then
        entity.Role.Should().Be(null);
        entity.IsRoleModified.Should().BeFalse();
        result.Should().Throw<DomainValidationException>().WithMessage("Cannot assign role to user.");
    }

    private static UserEntity CreateTestUser(UserRole? role = null)
    {
        return new UserEntity(UserGlobalId.From("U1"), null, null, null, null, role, null);
    }
}
