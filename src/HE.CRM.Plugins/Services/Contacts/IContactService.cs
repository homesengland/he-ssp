using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.CRM.Plugins.Services.Contacts
{
    public interface IContactService : ICrmService
    {

        ContactRolesDto GetContactRoles(string email, string ssid, string portal);
        ContactDto GetUserProfile(string contactExternalId);
    }
}
