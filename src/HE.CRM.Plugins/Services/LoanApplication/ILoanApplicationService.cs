using DataverseModel;
using HE.Base.Services;
using DataverseModel;

namespace HE.CRM.Plugins.Services.LoanApplication
{
    public interface ILoanApplicationService : ICrmService
    {
        string CreateRecordFromPortal(string contactExternalId, string accountId, string loanApplicationId, string loanApplicationPayload);
        void ChangeLoanApplicationStatusOnOwnerChange(invln_Loanapplication target, invln_Loanapplication preImage, invln_Loanapplication postImage);
        string GetLoanApplicationsForAccountAndContact(string externalContactId, string accountId, string loanApplicationId = null);
        void ChangeLoanApplicationExternalStatus(int externalStatus, string loanApplicationId);
        void UpdateLoanApplication(string loanApplicationId, string loanApplication, string fieldsToUpdate, string accountId, string contactExternalId);
        void DeleteLoanApplication(string loanApplicationId);
        void CheckIfOwnerCanBeChanged(invln_Loanapplication target, invln_Loanapplication preImage);
    }
}
