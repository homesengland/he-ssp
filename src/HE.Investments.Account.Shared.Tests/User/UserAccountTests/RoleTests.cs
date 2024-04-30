using FluentAssertions;
using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Common.Tests.TestData;
using Xunit;

namespace HE.Investments.Account.Shared.Tests.User.UserAccountTests;

public class RoleTests
{
    [Fact]
    public void ShouldThrowException_WhenThereIsNoRole()
    {
        // given
        var userAccount = UserAccountTestData.UserAccountOne with { Roles = [] };

        // when
        Action action = () => userAccount.Role();

        // then
        action.Should().ThrowExactly<UnauthorizedAccessException>();
    }

    [Fact]
    public void ShouldReturnRole_WhenThereIsOneRole()
    {
        // given
        var userAccount = UserAccountTestData.UserAccountOne;

        // when
        var role = userAccount.Role();

        // then
        role.Should().Be(UserRole.Limited);
    }

    [Fact]
    public void ShouldReturnMaxRole_WhenThereAreMultipleRoles()
    {
        // given
        var userAccount = UserAccountTestData.AdminUserAccountOne;

        // when
        var role = userAccount.Role();

        // then
        role.Should().Be(UserRole.Admin);
    }
}
