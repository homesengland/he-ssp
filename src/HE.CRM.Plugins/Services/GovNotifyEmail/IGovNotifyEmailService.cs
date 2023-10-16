using DataverseModel;
using HE.Base.Services;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Plugins.Services.GovNotifyEmail
{
    public interface IGovNotifyEmailService : ICrmService
    {
        void SendNotifications_INTERNAL_LOAN_APP_STATUS_CHANGE(invln_Loanstatuschange statusChange, invln_Loanapplication loanApplication, string statusLabel, string pastFormStatus);
        void SendNotifications_INTERNAL_LOAN_APP_OWNER_CHANGE(invln_Loanapplication loanApplication, string subject, string appId);
    }
}
