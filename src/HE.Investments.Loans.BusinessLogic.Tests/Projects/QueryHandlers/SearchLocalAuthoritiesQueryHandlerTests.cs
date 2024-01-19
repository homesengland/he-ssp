using HE.Investments.Loans.BusinessLogic.Projects.QueryHandlers;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.ObjectBuilders;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData;
using HE.Investments.Loans.Contract.Projects.Queries;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.QueryHandlers;

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
        result.ReturnedData.Items![0].Name.Should().Be("Liverpool");
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
