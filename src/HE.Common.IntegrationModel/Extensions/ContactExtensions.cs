using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.Organisation.Extensions
{
    public static class ContactExtensions
    {
        public static bool IsConnectedWithExternalIdentity(this ContactDto contact)
        {
            return contact != null
                   && !string.IsNullOrWhiteSpace(contact.contactExternalId)
                   && !contact.contactExternalId.StartsWith("_");
        }
    }
}
