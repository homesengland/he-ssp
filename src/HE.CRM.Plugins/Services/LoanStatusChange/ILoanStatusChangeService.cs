using DataverseModel;
using HE.Base.Services;

namespace HE.CRM.Plugins.Services.LoanStatusChange
{
    public interface ILoanStatusChangeService : ICrmService
    {
        void SendNotificationOnLoanStatusChangeCreate(invln_Loanstatuschange target);
        void SendInternalCrmNotification(invln_Loanstatuschange statusChange, invln_Loanapplication loanApplication, string statusLabel);
    }
}
