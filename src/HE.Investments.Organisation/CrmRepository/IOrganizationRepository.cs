using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;

namespace HE.Investments.Organisation.CrmRepository;
internal interface IOrganizationRepository
{
    Guid? EnsureCreateOrganization(IOrganizationServiceAsync2 service, string companyNumber, string companyName);

    EntityCollection? SearchForOrganizations(IOrganizationServiceAsync2 service, IEnumerable<string> organizationNumbers);

    Entity? GetDefaultAccount(IOrganizationServiceAsync2 service);
}
