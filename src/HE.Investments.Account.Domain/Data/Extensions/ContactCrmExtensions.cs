using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.Account.Domain.Data.Extensions;

public static class ContactCrmExtensions
{
    public static bool IsConnectedWithExternalIdentity(this ContactDto contact)
    {
        return !string.IsNullOrWhiteSpace(contact.contactExternalId)
               && !contact.contactExternalId.StartsWith('_');
    }
}
