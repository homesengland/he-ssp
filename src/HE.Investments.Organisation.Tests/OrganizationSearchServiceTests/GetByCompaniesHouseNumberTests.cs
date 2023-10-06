using FluentAssertions;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Organisation.CompaniesHouse.Contract;
using HE.Investments.Organisation.Contract;
using HE.Investments.Organisation.Services;
using HE.Investments.Organisation.Tests.TestObjectBuilders;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.Investments.Organisation.Tests.OrganizationSearchServiceTests;

public class GetByCompaniesHouseNumberTests : TestBase<OrganisationSearchService>
{
    private GetOrganizationByCompaniesHouseNumberResult _response;

    [Fact]
    public async Task Fail_WhenCompanyHousesReturnsError()
    {
        GivenThatCompanyHousesReturnsError();

        await WhenGettingOrganizationByCompaniesHouseNumber();

        _response.IsSuccessfull().Should().BeFalse();
    }

    [Fact]
    public async Task ReturnOrganizationFromCompanyHouses_WhenCrmReturnsNoOrganization()
    {
        GivenThatCompanyHousesReturns(OrganizationWithCompanyHouseNumber("1234"));
        GivenThatCrmReturnsNothing();

        await WhenGettingOrganizationByCompaniesHouseNumber();

        _response.Item.CompanyNumber.Should().Be("1234");
    }

    [Fact]
    public async Task ReturnNothing_WhenCompanyHousesReturnsNothing()
    {
        GivenThatComanyHousesReturnsNothing();
        GivenThatCrmReturnsNothing();

        await WhenGettingOrganizationByCompaniesHouseNumber();

        _response.Item.Should().BeNull();
    }

    [Fact]
    public async Task ReturnOrganizationDataFromCompanyHouses_WhenOrganizationCannotBeFoundInCrm()
    {
        var organizationNumber = "1234";
        var differentOrganizationNumber = "12345";

        GivenThatCompanyHousesReturns(CompanyDetails(organizationNumber, "CompanyName", "Sheffield", "Letsby Avenue", "PO16 7GZ"));
        GivenThatCrmReturns(CrmOrganization(differentOrganizationNumber, "CrmName", "CrmCity", "CrmStreet", "AJP2 GMD"));

        await WhenGettingOrganizationByCompaniesHouseNumber();

        var foundOrganization = _response.Item;

        foundOrganization.CompanyNumber.Should().Be("1234");
        foundOrganization.Name.Should().Be("CompanyName");
        foundOrganization.City.Should().Be("Sheffield");
        foundOrganization.Street.Should().Be("Letsby Avenue");
        foundOrganization.PostalCode.Should().Be("PO16 7GZ");
    }


    [Fact]
    public async Task ReturnOrganizationDataFromCrm_WhenOrganzationExistsThereAndIsReturnedFormCompanyHousesApi()
    {
        var sameOrganizationNumber = "1234";
        GivenThatCompanyHousesReturns(CompanyDetails(sameOrganizationNumber, "CompanyName", "Sheffield", "Letsby Avenue", "PO16 7GZ"));
        GivenThatCrmReturns(CrmOrganization(sameOrganizationNumber, "CrmName", "CrmCity", "CrmStreet", "AJP2 GMD"));

        await WhenGettingOrganizationByCompaniesHouseNumber();

        var foundOrganization = _response.Item;

        foundOrganization.CompanyNumber.Should().Be("1234");
        foundOrganization.Name.Should().Be("CrmName");
        foundOrganization.City.Should().Be("CrmCity");
        foundOrganization.Street.Should().Be("CrmStreet");
        foundOrganization.PostalCode.Should().Be("AJP2 GMD");
    }

    private void GivenThatCrmReturns(params OrganizationDetailsDto[] organizationsToReturn)
    {
        OrganizationCrmSearchServiceTestBuilder.New().ByCompanyHouseNumberReturns(organizationsToReturn).Register(this);
    }

    private void GivenThatCrmReturnsNothing()
    {
        OrganizationCrmSearchServiceTestBuilder.New().ByCompanyHouseNumberReturnsNothing().Register(this);
    }

    private void GivenThatComanyHousesReturnsNothing()
    {
        CompaniesHouseApiTestBuilder.New().GetByCompanyNumberReturnsNothing().Register(this);
    }

    private void GivenThatCompanyHousesReturns(CompanyDetailsItem organizationToReturn)
    {
        CompaniesHouseApiTestBuilder.New().GetByCompanyNumberReturns(organizationToReturn).Register(this);
    }

    private void GivenThatCompanyHousesReturnsError()
    {
        CompaniesHouseApiTestBuilder.New().GetByCompanyNumberReturnsError().Register(this);
    }

    private async Task WhenGettingOrganizationByCompaniesHouseNumber()
    {
        _response = await TestCandidate.GetByCompaniesHouseNumber("any number", CancellationToken.None);
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
