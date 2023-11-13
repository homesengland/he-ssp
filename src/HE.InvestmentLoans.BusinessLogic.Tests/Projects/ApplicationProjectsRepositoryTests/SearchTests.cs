using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.TestObjectBuilders;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.ApplicationProjectsRepositoryTests;

public class SearchTests : TestBase<ApplicationProjectsRepository>
{
    [Fact]
    public async Task ShouldReturnLocalAuthority()
    {
        // given
        var localAuthorities = LocalAuthorityTestData.LocalAuthoritiesList;
        var phrase = "Liverpool";

        var cacheServiceMock = CacheServiceMockTestBuilder
            .New()
            .MockSearchLocalAuthorityRequest(localAuthorities)
            .Build();

        RegisterDependency(cacheServiceMock);

        // when
        var (_, totalItems) = await TestCandidate.Search(phrase, 1, 10, CancellationToken.None);

        // then
        totalItems.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task ShouldReturnEmptyLocalAuthoritiesList()
    {
        // given
        var localAuthorities = LocalAuthorityTestData.LocalAuthoritiesList;
        var phrase = "non-existing local authority";

        var cacheServiceMock = CacheServiceMockTestBuilder
            .New()
            .MockSearchLocalAuthorityRequest(localAuthorities)
            .Build();

        RegisterDependency(cacheServiceMock);

        // when
        var (_, totalItems) = await TestCandidate.Search(phrase, 1, 10, CancellationToken.None);

        // then
        totalItems.Should().Be(0);
    }
}
