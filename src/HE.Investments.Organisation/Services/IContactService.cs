using HE.Common.IntegrationModel.PortalIntegrationModel;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Organisation.Services;

public interface IContactService
{
    Task<ContactDto?> RetrieveUserProfile(IOrganizationServiceAsync2 service, string contactExternalId);

    Task UpdateUserProfile(IOrganizationServiceAsync2 service, string contactExternalId, ContactDto contactDto, CancellationToken cancellationToken);

    Task<ContactRolesDto?> GetContactRoles(IOrganizationServiceAsync2 service, string contactEmail, string contactExternalId, int? portalType = null);

    Task<Guid> LinkContactWithOrganization(IOrganizationServiceAsync2 service, string contactExternalId, Guid organisationGuid, int portalType);

    Task RemoveLinkBetweenContactAndOrganisation(IOrganizationServiceAsync2 service, Guid organisationGuid, string contactExternalId, int? portalType = null);

    Task UpdateContactWebrole(IOrganizationServiceAsync2 service, string contactExternalId, Guid organisationGuid, int newWebRole, int? portalType = null);

    Task<List<ContactDto>> GetAllOrganisationContactsForPortal(IOrganizationServiceAsync2 service, Guid organisationGuid, int? portalType = null);

    Task<List<ContactRolesDto>> GetContactRolesForOrganisationContacts(IOrganizationServiceAsync2 service, List<string> contactExternalId, Guid organisationGuid);

    Task<Guid> CreateNotConnectedContact(IOrganizationServiceAsync2 service, ContactDto contact, Guid organisationGuid, int role, string inviterExternalId, int? portalType = null);
}
