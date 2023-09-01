using HE.Common.IntegrationModel.PortalIntegrationModel;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Organisation.Services;
public interface IContactService
{
    Task<ContactDto?> RetrieveUserProfile(IOrganizationServiceAsync2 service, string contactExternalId);

    Task UpdateUserProfile(IOrganizationServiceAsync2 service, string contactExternalId, ContactDto contactDto);

    Task<ContactRolesDto?> GetContactRoles(IOrganizationServiceAsync2 service, string contactEmail, string portalType, string contactExternalId);
}
