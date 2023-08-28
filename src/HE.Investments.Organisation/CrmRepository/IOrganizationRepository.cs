using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;

namespace HE.Investments.Organisation.CrmRepository;
public interface IOrganizationRepository
{
    Guid? EnsureCreateOrganization(IOrganizationServiceAsync2 service, string companyNumber, string companyName);

    EntityCollection? SearchForOrganizations(IOrganizationServiceAsync2 service, List<string> organizationNumbers);
}
