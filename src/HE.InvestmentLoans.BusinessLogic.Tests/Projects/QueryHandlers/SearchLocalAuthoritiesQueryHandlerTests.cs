using HE.InvestmentLoans.BusinessLogic.Projects.QueryHandlers;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.ObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.TestData;
using HE.InvestmentLoans.Contract.Projects.Queries;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.QueryHandlers;

public class SearchLocalAuthoritiesQueryHandlerTests : TestBase<SearchLocalAuthoritiesQueryHandler>
{
    [Fact]
    public async Task ShouldReturnLocalAuthority()
    {
        // given
        var localAuthorities = LocalAuthorityTestData.LocalAuthoritiesList;
        var phrase = "live";

        ApplicationProjectsRepositoryBuilder.New().ReturnsLocalAuthorities(phrase, localAuthorities).BuildLocalAuthorityMockAndRegister(this);

        // when
        var result = await TestCandidate.Handle(new SearchLocalAuthoritiesQuery(phrase, 1, 10), CancellationToken.None);

        // then
        result.ReturnedData.Items!.First().Name.Should().Be("Liverpool");
    }

    [Fact]
    public async Task ShouldReturnEmptyLocalAuthoritiesList()
    {
        // given
        var localAuthorities = LocalAuthorityTestData.LocalAuthoritiesList;
        var phrase = "non-existing local authority";

        ApplicationProjectsRepositoryBuilder.New().ReturnsLocalAuthorities(phrase, localAuthorities).BuildLocalAuthorityMockAndRegister(this);

        // when
        var result = await TestCandidate.Handle(new SearchLocalAuthoritiesQuery(phrase, 1, 10), CancellationToken.None);

        // then
        result.ReturnedData.Items.Should().BeEmpty();
    }
}
