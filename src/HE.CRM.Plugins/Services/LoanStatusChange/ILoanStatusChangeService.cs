using DataverseModel;
using HE.Base.Services;

namespace HE.CRM.Plugins.Services.LoanStatusChange
{
    public interface ILoanStatusChangeService : ICrmService
    {
        void SendNotificationOnCreate(invln_Loanstatuschange target);
    }
}
