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
        var result = await TestCandidate.Search(phrase, 1, 10, CancellationToken.None);

        // then
        result.TotalItems.Should().BeGreaterThan(0);
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
        var result = await TestCandidate.Search(phrase, 1, 10, CancellationToken.None);

        // then
        result.TotalItems.Should().Be(0);
    }
}
