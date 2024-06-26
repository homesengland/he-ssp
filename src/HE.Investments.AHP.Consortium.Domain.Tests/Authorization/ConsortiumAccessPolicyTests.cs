using FluentAssertions;
using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Account.Shared;
using HE.Investments.AHP.Consortium.Domain.Tests.TestData;
using HE.Investments.AHP.Consortium.Domain.Tests.TestObjectBuilders;
using HE.Investments.Consortium.Shared.Authorization;
using Xunit;

namespace HE.Investments.AHP.Consortium.Domain.Tests.Authorization;

public class ConsortiumAccessPolicyTests
{
    [Theory]
    [InlineData(UserRole.Limited)]
    [InlineData(UserRole.Input)]
    [InlineData(UserRole.Enhanced)]
    [InlineData(UserRole.Admin)]
    public async Task ShouldReturnTrueForNotConsortiumMember(UserRole userRole)
    {
        // given
        var consortiumUserContext = ConsortiumUserContextTestBuilder
            .New(AhpUserAccountTestData.UserAccountOneNoConsortium with { Roles = [userRole] })
            .Build();

        var consortiumCanAccess = new ConsortiumAccessPolicy(consortiumUserContext);

        // when
        var canAccess = await consortiumCanAccess.CanAccess(AccountAccessContext.EditApplicationRoles.ToList());

        // then
        canAccess.Should().BeTrue();
    }

    [Theory]
    [InlineData(UserRole.Input)]
    [InlineData(UserRole.Enhanced)]
    [InlineData(UserRole.Admin)]
    public async Task ShouldReturnTrueForLeadConsortiumMember(UserRole userRole)
    {
        // given
        var consortiumUserContext = ConsortiumUserContextTestBuilder
            .New(AhpUserAccountTestData.UserAccountOneWithConsortium with { Roles = [userRole] })
            .Build();

        var consortiumCanAccess = new ConsortiumAccessPolicy(consortiumUserContext);

        // when
        var canAccess = await consortiumCanAccess.CanAccess(AccountAccessContext.EditApplicationRoles.ToList());

        // then
        canAccess.Should().BeTrue();
    }

    [Theory]
    [InlineData(UserRole.Input)]
    [InlineData(UserRole.Enhanced)]
    [InlineData(UserRole.Admin)]
    public async Task ShouldReturnFalseForConsortiumMember(UserRole userRole)
    {
        // given
        var consortium = AhpUserAccountTestData.UserAccountOneWithConsortium.Consortium;
        var consortiumUserContext = ConsortiumUserContextTestBuilder
            .New(AhpUserAccountTestData.UserAccountOneWithConsortium with { Roles = [userRole], Consortium = consortium with { IsLeadPartner = false } })
            .Build();

        var consortiumCanAccess = new ConsortiumAccessPolicy(consortiumUserContext);

        // when
        var canAccess = await consortiumCanAccess.CanAccess(AccountAccessContext.EditApplicationRoles.ToList());

        // then
        canAccess.Should().BeFalse();
    }
}
