using FluentAssertions;
using HE.InvestmentLoans.BusinessLogic.Organization.QueryHandlers;
using HE.InvestmentLoans.Contract.Organization;
using HE.Investments.Organisation.CompaniesHouse.Contract;
using HE.Investments.Organisation.Contract;
using HE.Investments.Organisation.Services;
using Moq;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Organizations;

[TestClass]
public class SearchOrganizationsTests
{
    private SearchOrganizationsHandler _handler;
    private SearchOrganizations _query;
    private Mock<IOrganisationSearchService> _searchServiceMock;
    private SearchOrganisationResponse _response;

    [TestInitialize]
    public void Init()
    {
        _searchServiceMock = new Mock<IOrganisationSearchService>();
        _query = new SearchOrganizations("anyPhrase", 1, 1);
    }

    [TestMethod]
    public async Task Fail_when_api_company_houses_returns_error()
    {
        GivenThatCompanyHousesReturnsError();

        Func<Task> testDelegate = async () => await WhenSearchingOrganizations();

        await testDelegate.Should().ThrowAsync<Exception>();
    }

    [TestMethod]
    public async Task Return_results_from_company_houses()
    {
        GivenThatCompanyHousesReturns(OrganizationWithCompanyHouseNumber("1234"));

        await WhenSearchingOrganizations();

        _response.Result.Organizations.Should().HaveCount(1);
        _response.Result.Organizations.Should().Contain(org => org.CompaniesHouseNumber == "1234");
    }

    [TestMethod]
    public async Task Result_from_company_houses_should_have_correct_data()
    {
        GivenThatCompanyHousesReturns(new OrganisationSearchItem("1234", "CompanyName", "Sheffield", "Letsby Avenue", "PO16 7GZ"));

        await WhenSearchingOrganizations();

        var foundOrganization = _response.Result.Organizations.First();

        foundOrganization.CompaniesHouseNumber.Should().Be("1234");
        foundOrganization.Name.Should().Be("CompanyName");
        foundOrganization.City.Should().Be("Sheffield");
        foundOrganization.Street.Should().Be("Letsby Avenue");
        foundOrganization.Code.Should().Be("PO16 7GZ");
    }

    [TestMethod]
    public async Task Return_nothing_company_houses_returns_nothing()
    {
        GivenThatComanyHousesReturnsNothing();

        await WhenSearchingOrganizations();

        _response.Result.Organizations.Should().BeEmpty();
    }

    [TestMethod]
    public async Task Return_total_number_of_organizations_from_company_houses()
    {
        GivenThatCompanyHousesReturnsTotalOrganizations(10);

        await WhenSearchingOrganizations();

        _response.Result.TotalOrganizations.Should().Be(10);
    }

    private void GivenThatComanyHousesReturnsNothing()
    {
        _searchServiceMock.Setup(c => c.Search(It.IsAny<string>(), It.IsAny<PagingQueryParams>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new OrganisationSearchResult(Enumerable.Empty<OrganisationSearchItem>().ToList(), 0, null!));
    }

    private void GivenThatCompanyHousesReturns(params OrganisationSearchItem[] organizationsToReturn)
    {
        _searchServiceMock
            .Setup(c => c.Search(It.IsAny<string>(), It.IsAny<PagingQueryParams>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new OrganisationSearchResult(organizationsToReturn.ToList(), organizationsToReturn.Length, null!));
    }

    private void GivenThatCompanyHousesReturnsTotalOrganizations(int numberOfOrganizations)
    {
        _searchServiceMock
            .Setup(c => c.Search(It.IsAny<string>(), It.IsAny<PagingQueryParams>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new OrganisationSearchResult(Enumerable.Empty<OrganisationSearchItem>().ToList(), numberOfOrganizations, null!));
    }

    private void GivenThatCompanyHousesReturnsError()
    {
        _searchServiceMock
            .Setup(c => c.Search(It.IsAny<string>(), It.IsAny<PagingQueryParams>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new OrganisationSearchResult(null!, 0, "Any error message"));
    }

    private async Task WhenSearchingOrganizations()
    {
        _handler = new SearchOrganizationsHandler(_searchServiceMock.Object);

        _response = await _handler.Handle(_query, CancellationToken.None);
    }

    private OrganisationSearchItem OrganizationWithCompanyHouseNumber(string number)
    {
        return new OrganisationSearchItem(number, "AnyName", "AnyCity", "AnyStreet", "AnyPostalCode");
    }
}
