using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;

namespace HE.Investments.Organisation.CrmRepository;
public interface IOrganizationRepository
{
    Guid? EnsureCreateOrganization(IOrganizationServiceAsync2 service, string companyNumber, string companyName);

    EntityCollection? SearchForOrganizations(IOrganizationServiceAsync2 service, IEnumerable<string> names, bool recordsWithoutCopanyNumberIncluded);

    Entity? GetDefaultAccount(IOrganizationServiceAsync2 service);

    Entity? GetOrganizationViaCompanyHouseNumber(IOrganizationServiceAsync2 service, string companyHouseNumber);
}
