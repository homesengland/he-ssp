using FluentAssertions;
using HE.Investments.Organisation.LocalAuthorities;
using HE.Investments.Organisation.LocalAuthorities.Repositories;
using HE.Investments.Organisation.Tests.LocalAuthoritiesTests.TestData;
using HE.Investments.Organisation.Tests.TestObjectBuilders;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.Organisation.Tests.LocalAuthoritiesTests.LocalAuthorityRepositoryTests;

public class SearchTests : TestBase<LocalAuthorityRepository>
{
    [Fact]
    public async Task ShouldReturnLocalAuthority()
    {
        // given
        var localAuthorities = LocalAuthorityTestData.LocalAuthoritiesList;
        var phrase = "r";

        var cacheServiceMock = CacheServiceMockTestBuilder
            .New()
            .MockLoansSearchLocalAuthorityRequest(localAuthorities)
            .Build();

        RegisterDependency(cacheServiceMock);
        RegisterDependency(LocalAuthoritySource.Loans);

        // when
        var (items, totalItems) = await TestCandidate.Search(phrase, 0, 3, CancellationToken.None);

        // then
        items.Should().HaveCount(3);
        totalItems.Should().Be(4);
    }

    [Fact]
    public async Task ShouldReturnEmptyLocalAuthoritiesList()
    {
        // given
        var localAuthorities = LocalAuthorityTestData.LocalAuthoritiesList;
        var phrase = "non-existing local authority";

        var cacheServiceMock = CacheServiceMockTestBuilder
            .New()
            .MockLoansSearchLocalAuthorityRequest(localAuthorities)
            .Build();

        RegisterDependency(cacheServiceMock);

        // when
        var (_, totalItems) = await TestCandidate.Search(phrase, 1, 10, CancellationToken.None);

        // then
        totalItems.Should().Be(0);
    }
}
