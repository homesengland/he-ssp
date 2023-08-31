using HE.Common.IntegrationModel.PortalIntegrationModel;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Organisation.Services;
public interface IContactService
{
    ContactDto? RetrieveUserProfile(IOrganizationServiceAsync2 service, string contactExternalId);

    void UpdateUserProfile(IOrganizationServiceAsync2 service, string contactExternalId, ContactDto contactDto);

    ContactRolesDto? GetContactRoles(IOrganizationServiceAsync2 service, string contactEmail, string portalType, string contactExternalId);
}
