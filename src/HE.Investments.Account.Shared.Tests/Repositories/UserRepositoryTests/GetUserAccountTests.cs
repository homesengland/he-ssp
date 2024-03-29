using FluentAssertions;
using HE.Investments.Account.Shared.Repositories;
using HE.Investments.Common.Contract;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.Account.Shared.Tests.Repositories.UserRepositoryTests;

public class GetUserAccountTests : TestBase<AccountCrmRepository>
{
    [Fact]
    public async Task ShouldReturnUserAccounts_WhenUserGlobalIdAndEmailAreCorrect()
    {
        // given
        var contactRolesDto = ContactServiceMockTestBuilder
            .New()
            .Register(this)
            .ContactRolesFromMock;

        ContactServiceMockTestBuilder
            .New()
            .ReturnContactRolesDto(contactRolesDto)
            .Register(this);

        // when
        var result = await TestCandidate.GetUserAccounts(UserGlobalId.From(contactRolesDto.externalId), contactRolesDto.email);

        // then
        var account = result[0];
        account.UserGlobalId.Value.Should().Be(contactRolesDto.externalId);
        account.UserEmail.Should().Be(contactRolesDto.email);
        account.Roles.Should().ContainSingle(x => x == UserRoleMapper.ToDomain(contactRolesDto.contactRoles[0].permission));
    }

    [Fact]
    public async Task ShouldReturnEmptyUserAccounts_WhenUserGlobalIdAndEmailAreFake()
    {
        // given
        var contactRolesDto = ContactServiceMockTestBuilder
            .New()
            .Register(this)
            .ContactRolesFromMock;

        ContactServiceMockTestBuilder
            .New()
            .ReturnContactRolesDto(contactRolesDto)
            .Register(this);

        var wrongUserGlobalId = "Wrong user global id";
        var fakeEmail = "fake@fake.com";

        // when
        var result = await TestCandidate.GetUserAccounts(UserGlobalId.From(wrongUserGlobalId), fakeEmail);

        // then
        result.Should().BeEmpty();
    }
}
