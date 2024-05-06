using HE.Common.IntegrationModel.PortalIntegrationModel;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Organisation.Services;

public interface IContactService
{
    Task<ContactDto?> RetrieveUserProfile(IOrganizationServiceAsync2 service, string contactExternalId);

    Task UpdateUserProfile(IOrganizationServiceAsync2 service, string contactExternalId, ContactDto contactDto, CancellationToken cancellationToken);

    Task<ContactRolesDto?> GetContactRoles(IOrganizationServiceAsync2 service, string contactEmail, string contactExternalId, int? portalType = null);

    Task<string> LinkContactWithOrganization(IOrganizationServiceAsync2 service, string contactExternalId, string organisationId, int portalType);

    Task RemoveLinkBetweenContactAndOrganisation(IOrganizationServiceAsync2 service, string organisationId, string contactExternalId, int? portalType = null);

    Task UpdateContactWebRole(IOrganizationServiceAsync2 service, string contactExternalId, string contactAssigningExternalId, string organisationId, int newWebRole, int? portalType = null);

    Task<List<ContactDto>> GetAllOrganisationContactsForPortal(IOrganizationServiceAsync2 service, string organisationId, int? portalType = null);

    Task<List<ContactRolesDto>> GetContactRolesForOrganisationContacts(IOrganizationServiceAsync2 service, List<string> contactExternalId, string organisationId);

    Task<string> CreateNotConnectedContact(IOrganizationServiceAsync2 service, ContactDto contact, string organisationId, int role, string inviterExternalId, int? portalType = null);
}
