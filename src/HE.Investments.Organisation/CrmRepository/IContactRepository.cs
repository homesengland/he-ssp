using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;

namespace HE.Investments.Organisation.CrmRepository;
public interface IContactRepository
{
    Entity? GetContactViaExternalId(IOrganizationServiceAsync2 service, string contactExternalId, string[]? columnSet = null);

    Entity? GetContactWithGivenEmailAndExternalId(IOrganizationServiceAsync2 service, string contactEmail, string contactExternalId);
}
