using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Organisation.CrmRepository;
public interface IOrganizationRepository
{
    Guid? EnsureCreateOrganization(IOrganizationServiceAsync2 service, string companyNumber, string companyName);
}
