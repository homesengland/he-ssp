using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Organisation.Services;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.Investments.Organisation.Tests.TestObjectBuilders;

public class OrganizationCrmSearchServiceTestBuilder : TestServiceBuilder<IOrganizationCrmSearchService>
{
    public static OrganizationCrmSearchServiceTestBuilder New() => new();

    public OrganizationCrmSearchServiceTestBuilder ByCompanyHouseNumberReturnsNothing()
    {
        Mock.Setup(c => c.SearchOrganizationInCrmByCompanyHouseNumber(It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(Array.Empty<OrganizationDetailsDto>());

        return this;
    }

    public OrganizationCrmSearchServiceTestBuilder ByNameReturnsNothing()
    {
        Mock.Setup(c => c.SearchOrganizationInCrmByName(It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(Array.Empty<OrganizationDetailsDto>());

        return this;
    }

    public OrganizationCrmSearchServiceTestBuilder ByCompanyHouseNumberReturns(params OrganizationDetailsDto[] organizationsToReturn)
    {
        Mock.Setup(c => c.SearchOrganizationInCrmByCompanyHouseNumber(It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(organizationsToReturn.ToList());

        return this;
    }

    public OrganizationCrmSearchServiceTestBuilder ByNameReturns(string? searchPhrase, params OrganizationDetailsDto[] organizationDetailsDto)
    {
        Mock.Setup(c => c.SearchOrganizationInCrmByName(searchPhrase ?? It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(organizationDetailsDto.ToList());

        return this;
    }
}
