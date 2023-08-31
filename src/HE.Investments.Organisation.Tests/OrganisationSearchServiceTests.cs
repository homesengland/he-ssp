using Castle.Core.Logging;
using FluentAssertions;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Organisation.CompaniesHouse;
using HE.Investments.Organisation.CompaniesHouse.Contract;
using HE.Investments.Organisation.Contract;
using HE.Investments.Organisation.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace HE.Investments.Organisation.Tests;

public class OrganisationSearchServiceTests
{
    private OrganisationSearchService _searchService;
    private OrganisationSearchResult _response;

    private readonly Mock<ICompaniesHouseApi> _companiesHouseApiMock;
    private readonly Mock<IOrganizationCrmSearchService> _organizationCrmSearchServiceMock;

    public OrganisationSearchServiceTests()
    {
        _companiesHouseApiMock = new Mock<ICompaniesHouseApi>();
        _organizationCrmSearchServiceMock = new Mock<IOrganizationCrmSearchService>();
    }

    [Fact]
    public async Task Fail_WhenCompanyHousesReturnsError()
    {
        GivenThatCompanyHousesReturnsError();

        await WhenSearchingOrganizations();

        _response.IsSuccessfull().Should().BeFalse();
    }

    [Fact]
    public async Task ReturnResultsFromCompanyHouses_WhenCrmReturnsNoOrganization()
    {
        GivenThatCompanyHousesReturns(OrganizationWithCompanyHouseNumber("1234"));
        GivenThatCrmReturnsNothing();

        await WhenSearchingOrganizations();

        _response.Items.Should().HaveCount(1);
        _response.Items.Should().Contain(org => org.CompanyNumber == "1234");
    }

    [Fact]
    public async Task ReturnNothing_WhenCompanyHousesReturnsNothing()
    {
        GivenThatComanyHousesReturnsNothing();

        await WhenSearchingOrganizations();

        _response.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task ReturnTotalNumberOfOrganizationsFromCompanyHouses()
    {
        GivenThatCompanyHousesReturnsTotalOrganizations(10);

        await WhenSearchingOrganizations();

        _response.TotalItems.Should().Be(10);
    }

    [Fact]
    public async Task ReturnOrganizationDataFromCrm_WhenOrganzationExistsThereAndIsReturnedFormCompanyHousesApi()
    {
        var sameOrganizationNumber = "1234";
        GivenThatCompanyHousesReturns(CompanyDetails(sameOrganizationNumber, "CompanyName", "Sheffield", "Letsby Avenue", "PO16 7GZ"));
        GivenThatCrmReturns(CrmOrganization(sameOrganizationNumber, "CrmName", "CrmCity", "CrmStreet", "AJP2 GMD"));

        await WhenSearchingOrganizations();

        _response.Items.Should().HaveCount(1);
        var foundOrganization = _response.Items.First();

        foundOrganization.CompanyNumber.Should().Be("1234");
        foundOrganization.Name.Should().Be("CrmName");
        foundOrganization.City.Should().Be("CrmCity");
        foundOrganization.Street.Should().Be("CrmStreet");
        foundOrganization.PostalCode.Should().Be("AJP2 GMD");
    }

    [Fact]
    public async Task MatchMultipleOrganizations()
    {
        var firstOrganizationNumber = "1234";
        var secondOrganizationNumber = "12345";
        GivenThatCompanyHousesReturns(
            CompanyDetails(firstOrganizationNumber, "CompanyName", "Sheffield", "Letsby Avenue", "PO16 7GZ"),
            CompanyDetails(secondOrganizationNumber, "CompanyName", "Sheffield", "Letsby Avenue", "PO16 7GZ"));

        GivenThatCrmReturns(
            CrmOrganization(firstOrganizationNumber, "CrmName", "CrmCity", "CrmStreet", "AJP2 GMD"),
            CrmOrganization(secondOrganizationNumber, "CrmName2", "CrmCity2", "CrmStreet2", "BJP2 GMD"));

        await WhenSearchingOrganizations();

        _response.Items.Should().HaveCount(2);
        var firstOrganization = _response.Items.First(c => c.CompanyNumber == firstOrganizationNumber);

        firstOrganization.CompanyNumber.Should().Be(firstOrganizationNumber);
        firstOrganization.Name.Should().Be("CrmName");
        firstOrganization.City.Should().Be("CrmCity");
        firstOrganization.Street.Should().Be("CrmStreet");
        firstOrganization.PostalCode.Should().Be("AJP2 GMD");

        var secondOrganization = _response.Items.First(c => c.CompanyNumber == secondOrganizationNumber);

        secondOrganization.CompanyNumber.Should().Be(secondOrganizationNumber);
        secondOrganization.Name.Should().Be("CrmName2");
        secondOrganization.City.Should().Be("CrmCity2");
        secondOrganization.Street.Should().Be("CrmStreet2");
        secondOrganization.PostalCode.Should().Be("BJP2 GMD");
    }

    [Fact]
    public async Task ReturnOrganizationDataFromCompanyHouses_WhenOrganizationCannotBeFoundInCrm()
    {
        var organizationNumber = "1234";
        var differentOrganizationNumber = "12345";

        GivenThatCompanyHousesReturns(CompanyDetails(organizationNumber, "CompanyName", "Sheffield", "Letsby Avenue", "PO16 7GZ"));
        GivenThatCrmReturns(CrmOrganization(differentOrganizationNumber, "CrmName", "CrmCity", "CrmStreet", "AJP2 GMD"));

        await WhenSearchingOrganizations();

        var foundOrganization = _response.Items.First();

        _response.Items.Should().HaveCount(1);
        foundOrganization.CompanyNumber.Should().Be("1234");
        foundOrganization.Name.Should().Be("CompanyName");
        foundOrganization.City.Should().Be("Sheffield");
        foundOrganization.Street.Should().Be("Letsby Avenue");
        foundOrganization.PostalCode.Should().Be("PO16 7GZ");
    }

    private void GivenThatCrmReturns(params OrganizationDetailsDto[] organizationsToReturn)
    {
        _organizationCrmSearchServiceMock.Setup(c => c.SearchOrganizationInCrm(It.IsAny<IEnumerable<string>>()))
            .Returns(organizationsToReturn.ToList());
    }

    private void GivenThatCrmReturnsNothing()
    {
        _organizationCrmSearchServiceMock.Setup(c => c.SearchOrganizationInCrm(It.IsAny<IEnumerable<string>>()))
            .Returns(Enumerable.Empty<OrganizationDetailsDto>());
    }

    private void GivenThatComanyHousesReturnsNothing()
    {
        _companiesHouseApiMock.Setup(c => c.Search(It.IsAny<string>(), It.IsAny<PagingQueryParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CompaniesHouseSearchResult.New(Enumerable.Empty<CompanyDetailsItem>().ToList(), 0));
    }

    private void GivenThatCompanyHousesReturns(params CompanyDetailsItem[] organizationsToReturn)
    {
        _companiesHouseApiMock
            .Setup(c => c.Search(It.IsAny<string>(), It.IsAny<PagingQueryParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CompaniesHouseSearchResult.New(organizationsToReturn.ToList(), organizationsToReturn.Length));
    }

    private void GivenThatCompanyHousesReturnsTotalOrganizations(int numberOfOrganizations)
    {
        _companiesHouseApiMock
            .Setup(c => c.Search(It.IsAny<string>(), It.IsAny<PagingQueryParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CompaniesHouseSearchResult.New(Enumerable.Empty<CompanyDetailsItem>().ToList(), numberOfOrganizations));
    }

    private void GivenThatCompanyHousesReturnsError()
    {
        _companiesHouseApiMock
            .Setup(c => c.Search(It.IsAny<string>(), It.IsAny<PagingQueryParams>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException());
    }

    private async Task WhenSearchingOrganizations()
    {
        var loggerMock = new Mock<ILogger<OrganisationSearchService>>();
        _searchService = new OrganisationSearchService(_companiesHouseApiMock.Object, _organizationCrmSearchServiceMock.Object, loggerMock.Object);

        _response = await _searchService.Search("any phrase", new PagingQueryParams(1, 1), "any number", CancellationToken.None);
    }

    private CompanyDetailsItem OrganizationWithCompanyHouseNumber(string number)
    {
        return CompanyDetails(number, "AnyName", "AnyCity", "AnyStreet", "AnyPostalCode");
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

    private CompanyDetailsItem CompanyDetails(string companyNumber, string name, string city, string street, string postalcode)
    {
        return new CompanyDetailsItem
        {
            CompanyNumber = companyNumber,
            CompanyName = name,
            OfficeAddress = new OfficeAddress
            {
                PostalCode = postalcode,
                AddressLine1 = street,
                Locality = city,
            }
        };
    }
}
