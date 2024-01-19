using FluentAssertions;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Organisation.CompaniesHouse.Contract;
using HE.Investments.Organisation.Contract;
using HE.Investments.Organisation.Services;
using HE.Investments.Organisation.Tests.TestAssertions;
using HE.Investments.Organisation.Tests.TestObjectBuilders;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.Organisation.Tests.OrganizationSearchServiceTests;

public class SearchTests : TestBase<OrganisationSearchService>
{
    private OrganisationSearchResult _response;

    [Fact]
    public async Task Fail_WhenCompanyHousesReturnsError()
    {
        CompaniesHouseApiTestBuilder
            .New()
            .SearchReturnsError()
            .Register(this);

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
        GivenThatCompanyHousesReturnsNothing();
        GivenThatCrmReturnsNothing();

        await WhenSearchingOrganizations();

        _response.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task ReturnTotalNumberOfOrganizationsFromCompanyHouses()
    {
        GivenThatCompanyHousesReturnsTotalOrganizations(10);
        GivenThatCrmReturnsNothing();

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
        var foundOrganization = _response.Items[0];

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

        var foundOrganization = _response.Items[0];

        _response.Items.Should().HaveCount(1);
        foundOrganization.CompanyNumber.Should().Be("1234");
        foundOrganization.Name.Should().Be("CompanyName");
        foundOrganization.City.Should().Be("Sheffield");
        foundOrganization.Street.Should().Be("Letsby Avenue");
        foundOrganization.PostalCode.Should().Be("PO16 7GZ");
    }

    [Fact]
    public async Task ShouldReturnSpvOrganisation_WhenItExistInCrm()
    {
        // given
        var organizationDetailsDto = OrganizationDetailsDtoTestBuilder.NewSpvCompany().Build();
        GivenThatCompanyHousesReturnsNothing();

        OrganizationCrmSearchServiceTestBuilder
            .New()
            .ByNameReturns(organizationDetailsDto.registeredCompanyName, organizationDetailsDto)
            .ByCompanyHouseNumberReturnsNothing()
            .Register(this);

        // when
        var result = await TestCandidate.Search(organizationDetailsDto.registeredCompanyName, new PagingQueryParams(1, 1), CancellationToken.None);

        // then
        result.IsSuccessfull().Should().BeTrue();
        result.TotalItems.Should().Be(1);
        result.Items.Should().ContainSingle(
            organizationDetailsDto.registeredCompanyName,
            organizationDetailsDto.companyRegistrationNumber,
            organizationDetailsDto.organisationId,
            true);
    }

    [Fact]
    public async Task ShouldReturnSpvOrganisationAndCompanyHouseCompany_WhenItExistInCrmAndCompanyHouse()
    {
        // given
        var spvCompany = OrganizationDetailsDtoTestBuilder.NewSpvCompany().Build();
        var companyFromCompanyHouse = CompanyDetailsItemTestBuilder.New().Build();

        GivenThatCompanyHousesReturns(companyFromCompanyHouse);

        OrganizationCrmSearchServiceTestBuilder
            .New()
            .ByNameReturns(spvCompany.registeredCompanyName, spvCompany)
            .ByCompanyHouseNumberReturnsNothing()
            .Register(this);

        // when
        var result = await TestCandidate.Search(spvCompany.registeredCompanyName, new PagingQueryParams(2, 1), CancellationToken.None);

        // then
        result.IsSuccessfull().Should().BeTrue();
        result.TotalItems.Should().Be(2);
        result.Items.Count.Should().Be(2);
        result.Items[0].ShouldBe(companyFromCompanyHouse.CompanyName, companyFromCompanyHouse.CompanyNumber, null, false);
        result.Items[result.Items.Count - 1].ShouldBe(spvCompany.registeredCompanyName, spvCompany.companyRegistrationNumber, spvCompany.organisationId, true);
    }

    [Fact]
    public async Task ShouldNotReturnSpvCompany_WhenItIsOutOfPaging()
    {
        // given
        var spvCompany1 = OrganizationDetailsDtoTestBuilder.NewSpvCompany("CRM1").Build();
        var spvCompany2 = OrganizationDetailsDtoTestBuilder.NewSpvCompany("CRM2").Build();

        var companyFromCompanyHouse1 = CompanyDetailsItemTestBuilder.New("1111111").Build();

        GivenThatCompanyHousesReturns(companyFromCompanyHouse1);

        OrganizationCrmSearchServiceTestBuilder
            .New()
            .ByNameReturns(spvCompany1.registeredCompanyName, spvCompany1, spvCompany2)
            .ByCompanyHouseNumberReturnsNothing()
            .Register(this);

        // when
        var result = await TestCandidate.Search(spvCompany1.registeredCompanyName, new PagingQueryParams(2, 1), CancellationToken.None);

        // then
        result.IsSuccessfull().Should().BeTrue();
        result.TotalItems.Should().Be(3);
        result.Items.Count.Should().Be(2);
        result.Items[0].ShouldBe(companyFromCompanyHouse1.CompanyName, companyFromCompanyHouse1.CompanyNumber, null, false);
        result.Items[1].ShouldBe(spvCompany1.registeredCompanyName, spvCompany1.companyRegistrationNumber, spvCompany1.organisationId, true);
    }

    [Fact]
    public async Task ShouldNotReturnSpvCompanies_WhenThereAreOnTwoPages()
    {
        // given
        var spvCompany1 = OrganizationDetailsDtoTestBuilder.NewSpvCompany("CRM1").Build();
        var spvCompany2 = OrganizationDetailsDtoTestBuilder.NewSpvCompany("CRM2").Build();
        var spvCompany3 = OrganizationDetailsDtoTestBuilder.NewSpvCompany("CRM3").Build();
        var spvCompany4 = OrganizationDetailsDtoTestBuilder.NewSpvCompany("CRM4").Build();

        CompaniesHouseApiTestBuilder.New().SearchReturnsTotalOrganizations(1).Register(this);

        OrganizationCrmSearchServiceTestBuilder
            .New()
            .ByNameReturns(spvCompany1.registeredCompanyName, spvCompany1, spvCompany2, spvCompany3, spvCompany4)
            .ByCompanyHouseNumberReturnsNothing()
            .Register(this);

        // when
        var result = await TestCandidate.Search(spvCompany1.registeredCompanyName, new PagingQueryParams(2, 4), CancellationToken.None);

        // then
        result.IsSuccessfull().Should().BeTrue();
        result.TotalItems.Should().Be(5);
        result.Items.Count.Should().Be(1);
        result.Items.Single().ShouldBe(spvCompany4.registeredCompanyName, spvCompany4.companyRegistrationNumber, spvCompany4.organisationId, true);
    }

    private void GivenThatCrmReturns(params OrganizationDetailsDto[] crmOrganization)
    {
        OrganizationCrmSearchServiceTestBuilder.New().ByCompanyHouseNumberReturns(crmOrganization).ByNameReturnsNothing().Register(this);
    }

    private void GivenThatCrmReturnsNothing()
    {
        OrganizationCrmSearchServiceTestBuilder.New().ByNameReturnsNothing().ByCompanyHouseNumberReturnsNothing().Register(this);
    }

    private void GivenThatCompanyHousesReturnsNothing()
    {
        CompaniesHouseApiTestBuilder.New().SearchReturnsNothing().Register(this);
    }

    private void GivenThatCompanyHousesReturns(params CompanyDetailsItem[] organizationsToReturn)
    {
        CompaniesHouseApiTestBuilder.New().SearchReturns(organizationsToReturn).Register(this);
    }

    private void GivenThatCompanyHousesReturnsTotalOrganizations(int numberOfOrganizations)
    {
        CompaniesHouseApiTestBuilder.New().SearchReturnsTotalOrganizations(numberOfOrganizations).Register(this);
    }

    private async Task<OrganisationSearchResult> WhenSearchingOrganizations(string searchPhrase = "any phrase")
    {
        _response = await TestCandidate.Search(searchPhrase, new PagingQueryParams(1, 1), CancellationToken.None);
        return _response;
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
            OfficeAddress = new OfficeAddress { PostalCode = postalcode, AddressLine1 = street, Locality = city, }
        };
    }
}
