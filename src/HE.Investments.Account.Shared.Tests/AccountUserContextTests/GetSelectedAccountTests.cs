using FluentAssertions;
using HE.Investments.Account.Shared.Tests.TestDataBuilders;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using HE.Investments.Common.User;
using HE.Investments.Loans.Common.Exceptions;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.Investments.Account.Shared.Tests.AccountUserContextTests;

public class GetSelectedAccountTests : TestBase<AccountUserContext>
{
    [Fact]
    public async Task ShouldThrowException_WhenUserHasNoAccounts()
    {
        // given
        CreateAndRegisterDependencyMock<IUserContext>().Setup(x => x.UserGlobalId).Returns("user-123");
        CreateAndRegisterDependencyMock<ICacheService>()
            .Setup(x => x.GetValueAsync("UserAccount-user-123", It.IsAny<Func<Task<IList<UserAccount>>>>()))
            .ReturnsAsync((IList<UserAccount>?)null);

        // when
        var get = () => TestCandidate.GetSelectedAccount();

        // then
        await get.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task ShouldReturnLowestOrganisationId_WhenUserHasMultipleOrganisationsAssigned()
    {
        // given
        var userAccounts = new[]
        {
            new UserAccountTestDataBuilder().WithUserGlobalId("user-123").WithOrganisationId("00000000-0000-0000-0000-000000000003").Build(),
            new UserAccountTestDataBuilder().WithUserGlobalId("user-123").WithOrganisationId("00000000-0000-0000-0000-000000000001").Build(),
            new UserAccountTestDataBuilder().WithUserGlobalId("user-123").WithOrganisationId("00000000-0000-0000-0000-000000000002").Build(),
        };

        CreateAndRegisterDependencyMock<IUserContext>().Setup(x => x.UserGlobalId).Returns("user-123");
        CreateAndRegisterDependencyMock<ICacheService>()
            .Setup(x => x.GetValueAsync("UserAccount-user-123", It.IsAny<Func<Task<IList<UserAccount>>>>()))
            .ReturnsAsync(userAccounts);

        // when
        var result = await TestCandidate.GetSelectedAccount();

        // then
        result.Should().Be(userAccounts[1]);
    }
}
