using HE.Common.IntegrationModel.PortalIntegrationModel;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Organisation.Services;

public interface IContactService
{
    Task<ContactDto?> RetrieveUserProfile(IOrganizationServiceAsync2 service, string contactExternalId);

    Task UpdateUserProfile(IOrganizationServiceAsync2 service, string contactExternalId, ContactDto contactDto, CancellationToken cancellationToken);

    Task<ContactRolesDto?> GetContactRoles(IOrganizationServiceAsync2 service, string contactEmail, string portalType, string contactExternalId);

    Task<Guid> LinkContactWithOrganization(IOrganizationServiceAsync2 service, string contactExternalId, string organizationNumber, string portalType);

    Task RemoveLinkBetweenContactAndOrganisation(IOrganizationServiceAsync2 service, string organizationNumber, string portalType, string contactExternalId);

    Task UpdateContactWebrole(IOrganizationServiceAsync2 service, string contactExternalId, Guid organisationGuid, string newWebRoleName);

    Task<List<ContactDto>> GetAllOrganisationContactsForPortal(IOrganizationServiceAsync2 service, string organizationNumber, string portalType);
}
