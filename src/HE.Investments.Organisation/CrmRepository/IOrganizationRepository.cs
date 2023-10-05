using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;

namespace HE.Investments.Organisation.CrmRepository;
public interface IOrganizationRepository
{
    Guid? EnsureCreateOrganization(IOrganizationServiceAsync2 service, string companyNumber, string companyName);

    Task<EntityCollection?> SearchForOrganizationsByName(IOrganizationServiceAsync2 service, IEnumerable<string> names, bool recordWithCompanyHouseNumberIncluded);

    Task<EntityCollection?> SearchForOrganizationsByCompanyHouseNumber(IOrganizationServiceAsync2 service, IEnumerable<string> organizationNumbers);

    Entity? GetDefaultAccount(IOrganizationServiceAsync2 service);

    Entity? GetOrganizationViaCompanyHouseNumber(IOrganizationServiceAsync2 service, string companyHouseNumber);
}
