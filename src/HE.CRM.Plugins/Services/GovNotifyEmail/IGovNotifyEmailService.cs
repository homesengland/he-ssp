using DataverseModel;
using HE.Base.Services;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Plugins.Services.GovNotifyEmail
{
    public interface IGovNotifyEmailService : ICrmService
    {
        void SendGovNotifyEmail(EntityReference ownerId, EntityReference regardingObjectId, string subject, string applicationId, string statusAtBody, string entityLogicalName, string recordId);
    }
}
