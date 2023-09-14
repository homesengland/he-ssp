using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.BusinessLogic.User.Repositories;
using HE.InvestmentLoans.Common.Tests.TestFramework;
using HE.InvestmentLoans.Contract.User.ValueObjects;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.User.UserRepositoryTests;

public class GetUserAccountTests : TestBase<LoanUserRepository>
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
        var account = result.First();
        account.UserGlobalId.Value.Should().Be(contactRolesDto.externalId);
        account.UserEmail.Should().Be(contactRolesDto.email);
        account.Roles.Should().ContainSingle(x => x.Role == contactRolesDto.contactRoles.First().webRoleName);
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
