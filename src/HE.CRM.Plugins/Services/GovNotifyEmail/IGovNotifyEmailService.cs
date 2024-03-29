using DataverseModel;
using HE.Base.Services;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Plugins.Services.GovNotifyEmail
{
    public interface IGovNotifyEmailService : ICrmService
    {
        void SendNotifications_EXTERNAL_APPLICATION_ACTION_REQUIRED(invln_Loanstatuschange statusChange, invln_Loanapplication loanApplication, string actionRequired);
        void SendNotifications_EXTERNAL_APPLICATION_STATUS_CONFIRMATION(invln_Loanstatuschange statusChange, invln_Loanapplication loanApplication, string actionCompleted);
        void SendNotifications_EXTERNAL_APPLICATION_STATUS_INFORMATION(invln_Loanstatuschange statusChange, invln_Loanapplication loanApplication);
        void SendNotifications_INTERNAL_LOAN_APP_STATUS_CHANGE(invln_Loanstatuschange statusChange, invln_Loanapplication loanApplication, string statusLabel, string pastFormStatus);
        void SendNotifications_INTERNAL_LOAN_APP_OWNER_CHANGE(invln_Loanapplication loanApplication, string subject, string appId);
        void SendNotifications_EXTERNAL_KYC_STATUS_CHANGE(Contact contact, string subject, Account organisation);
        void SendNotifications_EXTERNAL_APPLICATION_CASHFLOW_REQUESTED(invln_Loanapplication loanApplication);
        void SendNotifications_COMMON_REQUEST_TO_ASSIGN_CONTACT_TO_EXISTING_ORGANISATION(EntityReference contactId, EntityReference associatingContactId);
    }
}
