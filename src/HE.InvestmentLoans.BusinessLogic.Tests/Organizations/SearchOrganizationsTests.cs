extern alias Org;

using FluentAssertions;
using HE.InvestmentLoans.BusinessLogic.Organization.QueryHandlers;
using HE.InvestmentLoans.Contract.Organization;
using Moq;
using Org.HE.Common.IntegrationModel.PortalIntegrationModel;
using Org.HE.Investments.Organisation.CompaniesHouse.Contract;
using Org.HE.Investments.Organisation.Contract;
using Org.HE.Investments.Organisation.Services;

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

    [TestMethod]
    public async Task Return_organization_data_from_crm_when_org_exists_there_and_is_returned_form_company_houses()
    {
        var sameOrganizationNumber = "1234";
        GivenThatCompanyHousesReturns(new OrganisationSearchItem(sameOrganizationNumber, "CompanyName", "Sheffield", "Letsby Avenue", "PO16 7GZ"));
        GivenThatCrmReturns(CrmOrganization(sameOrganizationNumber, "CrmName", "CrmCity", "CrmStreet", "AJP2 GMD"));

        await WhenSearchingOrganizations();

        _response.Result.Organizations.Should().HaveCount(1);
        var foundOrganization = _response.Result.Organizations.First();

        foundOrganization.CompaniesHouseNumber.Should().Be("1234");
        foundOrganization.Name.Should().Be("CrmName");
        foundOrganization.City.Should().Be("CrmCity");
        foundOrganization.Street.Should().Be("CrmStreet");
        foundOrganization.Code.Should().Be("AJP2 GMD");
    }

    [TestMethod]
    public async Task Match_multiple_organizations()
    {
        var firstOrganizationNumber = "1234";
        var secondOrganizationNumber = "12345";
        GivenThatCompanyHousesReturns(
            new OrganisationSearchItem(firstOrganizationNumber, "CompanyName", "Sheffield", "Letsby Avenue", "PO16 7GZ"),
            new OrganisationSearchItem(secondOrganizationNumber, "CompanyName", "Sheffield", "Letsby Avenue", "PO16 7GZ"));

        GivenThatCrmReturns(
            CrmOrganization(firstOrganizationNumber, "CrmName", "CrmCity", "CrmStreet", "AJP2 GMD"),
            CrmOrganization(secondOrganizationNumber, "CrmName2", "CrmCity2", "CrmStreet2", "BJP2 GMD"));

        await WhenSearchingOrganizations();

        _response.Result.Organizations.Should().HaveCount(2);
        var firstOrganization = _response.Result.Organizations.First(c => c.CompaniesHouseNumber == firstOrganizationNumber);

        firstOrganization.CompaniesHouseNumber.Should().Be(firstOrganizationNumber);
        firstOrganization.Name.Should().Be("CrmName");
        firstOrganization.City.Should().Be("CrmCity");
        firstOrganization.Street.Should().Be("CrmStreet");
        firstOrganization.Code.Should().Be("AJP2 GMD");

        var secondOrganization = _response.Result.Organizations.First(c => c.CompaniesHouseNumber == secondOrganizationNumber);

        secondOrganization.CompaniesHouseNumber.Should().Be(secondOrganizationNumber);
        secondOrganization.Name.Should().Be("CrmName2");
        secondOrganization.City.Should().Be("CrmCity2");
        secondOrganization.Street.Should().Be("CrmStreet2");
        secondOrganization.Code.Should().Be("BJP2 GMD");
    }

    [TestMethod]
    public async Task Return_organization_data_from_company_houses_when_org_cannot_be_found_in_crm()
    {
        var organizationNumber = "1234";
        var differentOrganizationNumber = "12345";

        GivenThatCompanyHousesReturns(new OrganisationSearchItem(organizationNumber, "CompanyName", "Sheffield", "Letsby Avenue", "PO16 7GZ"));
        GivenThatCrmReturns(CrmOrganization(differentOrganizationNumber, "CrmName", "CrmCity", "CrmStreet", "AJP2 GMD"));

        await WhenSearchingOrganizations();

        var foundOrganization = _response.Result.Organizations.First();

        _response.Result.Organizations.Should().HaveCount(1);
        foundOrganization.CompaniesHouseNumber.Should().Be("1234");
        foundOrganization.Name.Should().Be("CompanyName");
        foundOrganization.City.Should().Be("Sheffield");
        foundOrganization.Street.Should().Be("Letsby Avenue");
        foundOrganization.Code.Should().Be("PO16 7GZ");
    }

    private void GivenThatCrmReturns(params OrganizationDetailsDto[] organizationsToReturn)
    {
        _searchServiceMock.Setup(c => c.SearchOrganizationInCrm(It.IsAny<IEnumerable<string>>(), null!))
            .Returns(organizationsToReturn.ToList());
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
        _handler = new SearchOrganizationsHandler(_searchServiceMock.Object, null!);

        _response = await _handler.Handle(_query, CancellationToken.None);
    }

    private OrganisationSearchItem OrganizationWithCompanyHouseNumber(string number)
    {
        return new OrganisationSearchItem(number, "AnyName", "AnyCity", "AnyStreet", "AnyPostalCode");
    }

    private OrganizationDetailsDto CrmOrganization(string registrationNumber, string name, string city, string street, string postalCode)
    {
        return new OrganizationDetailsDto
        {
            registeredCompanyName = name,
            city = city,
            addressLine1 = street,
            companyRegistrationNumber = registrationNumber,
            postalcode = postalCode,
        };
    }
}
