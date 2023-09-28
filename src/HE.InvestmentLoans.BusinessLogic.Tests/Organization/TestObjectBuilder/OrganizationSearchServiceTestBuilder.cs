extern alias Org;
using HE.InvestmentLoans.BusinessLogic.Organization.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.Common.Tests.TestFramework;
using HE.InvestmentLoans.Contract.Organization;
using HE.InvestmentLoans.Contract.Organization.ValueObjects;
using MediatR;
using Moq;
using Org.HE.Common.IntegrationModel.PortalIntegrationModel;
using Org.HE.Investments.Organisation.Contract;
using Org.HE.Investments.Organisation.Services;
using Xunit;

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
            .Setup(c => c.GetByCompaniesHouseNumber(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetOrganizationByCompaniesHouseNumberResult(null!, "Error message"));

        return this;
    }

    public OrganizationSearchServiceTestBuilder ReturnsNoOrganization()
    {
        _mock
            .Setup(c => c.GetByCompaniesHouseNumber(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetOrganizationByCompaniesHouseNumberResult(null!, null!));

        return this;
    }

    public OrganizationSearchServiceTestBuilder Returns(OrganisationSearchItem organization)
    {
        _mock
            .Setup(c => c.GetByCompaniesHouseNumber(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetOrganizationByCompaniesHouseNumberResult(organization, null!));

        return this;
    }

    public OrganizationSearchServiceTestBuilder ReturnsOneOrganization(string companyHouseNumber, out OrganisationSearchItem result)
    {
        result = new OrganisationSearchItem(
            companyHouseNumber,
            "anyName",
            "anyCity",
            "Any Street 1",
            "ABCD 123");

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
