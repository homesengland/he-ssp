using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;

namespace HE.Investments.Organisation.CrmRepository;
public interface IContactRepository
{
    Entity? GetContactViaExternalId(IOrganizationServiceAsync2 service, string contactExternalId, string[]? columnSet = null);

    Entity? GetContactWithGivenEmailOrExternalId(IOrganizationServiceAsync2 service, string contactEmail, string contactExternalId);

    Entity? GetContactWithGivenEmail(IOrganizationServiceAsync2 service, string contactEmail);

    List<Entity> GetContactsForOrganisation(IOrganizationServiceAsync2 service, string organisationId, string? portalTypeFilter = null);
}
