extern alias Org;

using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Org.HE.Investments.Organisation.Contract;
using Org.HE.Investments.Organisation.Services;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Organization.TestObjectBuilder;

public class OrganizationSearchServiceTestBuilder
{
    private readonly Mock<IOrganisationSearchService> _mock;

    public OrganizationSearchServiceTestBuilder()
    {
        _mock = new Mock<IOrganisationSearchService>();
    }

    public static OrganizationSearchServiceTestBuilder New() => new();

    public OrganizationSearchServiceTestBuilder ReturnUnsuccessfulResponse()
    {
        _mock
            .Setup(c => c.GetByOrganisation(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetOrganizationByCompaniesHouseNumberResult(null!, "Error message"));

        return this;
    }

    public OrganizationSearchServiceTestBuilder GetByCompaniesHouseNumberReturnsNoOrganization()
    {
        _mock
            .Setup(c => c.GetByCompaniesHouseNumber(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetOrganizationByCompaniesHouseNumberResult(null));

        return this;
    }

    public OrganizationSearchServiceTestBuilder GeOrganisationReturnsNoOrganization()
    {
        _mock
            .Setup(c => c.GetByOrganisation(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetOrganizationByCompaniesHouseNumberResult(null));

        return this;
    }

    public OrganizationSearchServiceTestBuilder GetByOrganisationIdReturnsNoOrganization()
    {
        _mock
            .Setup(c => c.GetByOrganisationId(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetOrganizationByCompaniesHouseNumberResult(null, "Not found"));

        return this;
    }

    public OrganizationSearchServiceTestBuilder GetByOrganisationReturnsNoOrganization()
    {
        _mock
            .Setup(c => c.GetByOrganisation(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetOrganizationByCompaniesHouseNumberResult(null, "Not found"));

        return this;
    }

    public OrganizationSearchServiceTestBuilder GetByOrganisationIdReturns(string organisationId, out OrganisationSearchItem organisationSearchItem)
    {
        organisationSearchItem = new OrganisationSearchItem(
            null,
            "anyName",
            "anyCity",
            "Any Street 1",
            "ABCD 123",
            organisationId,
            true);

        _mock
            .Setup(c => c.GetByOrganisationId(organisationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetOrganizationByCompaniesHouseNumberResult(organisationSearchItem));

        return this;
    }

    public OrganizationSearchServiceTestBuilder GetByOrganisationReturns(string? organisationId, string? companyHouseNumber, out OrganisationSearchItem organisationSearchItem)
    {
        organisationSearchItem = new OrganisationSearchItem(
            companyHouseNumber,
            "anyName",
            "anyCity",
            "Any Street 1",
            "ABCD 123",
            organisationId,
            true);

        _mock
            .Setup(c => c.GetByOrganisation(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetOrganizationByCompaniesHouseNumberResult(organisationSearchItem));

        return this;
    }

    public OrganizationSearchServiceTestBuilder Returns(OrganisationSearchItem organization)
    {
        _mock
            .Setup(c => c.GetByOrganisation(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetOrganizationByCompaniesHouseNumberResult(organization));

        return this;
    }

    public OrganizationSearchServiceTestBuilder GetByCompaniesHouseNumberReturnsOneOrganization(string companyHouseNumber, out OrganisationSearchItem result)
    {
        result = new OrganisationSearchItem(
            companyHouseNumber,
            "anyName",
            "anyCity",
            "Any Street 1",
            "ABCD 123",
            null);

        _mock
            .Setup(c => c.GetByCompaniesHouseNumber(companyHouseNumber, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetOrganizationByCompaniesHouseNumberResult(result, string.Empty));

        return this;
    }

    public IOrganisationSearchService Build()
    {
        return _mock.Object;
    }

    public OrganizationSearchServiceTestBuilder Register(IRegisterDependency registerDependency)
    {
        var mockedObject = Build();
        registerDependency.RegisterDependency(mockedObject);

        return this;
    }
}
