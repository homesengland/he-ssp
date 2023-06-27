using HE.Base.Services;

namespace HE.CRM.Plugins.Services.Contacts
{
    public interface IContactService : ICrmService
    {

        string GetContactRole(string email, string ssid, string portal);
    }
}
