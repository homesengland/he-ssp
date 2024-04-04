using DataverseModel;
using HE.Base.Services;

namespace HE.CRM.Plugins.Services.LoanApplication
{
    public interface ILoanApplicationService : ICrmService
    {
        string CreateRecordFromPortal(bool useHeTables, string contactExternalId, string accountId, string loanApplicationId, string loanApplicationPayload);
        void ChangeLoanApplicationStatusOnOwnerChange(invln_Loanapplication target, invln_Loanapplication preImage, invln_Loanapplication postImage);
        string GetLoanApplicationsForAccountAndContact(bool useHeTables, string externalContactId, string accountId, string loanApplicationId = null, string fieldsToRetrieve = null);
        void ChangeLoanApplicationExternalStatus(int externalStatus, string loanApplicationId, string changeReason);
        void UpdateLoanApplication(bool useHeTables, string loanApplicationId, string loanApplication, string fieldsToUpdate, string accountId, string contactExternalId);
        void DeleteLoanApplication(string loanApplicationId);
        void CheckIfOwnerCanBeChanged(invln_Loanapplication target, invln_Loanapplication preImage);
        void SetFieldsWhenChangingStatusFromDraft(invln_Loanapplication target, invln_Loanapplication preImage);
        void ChangeExternalStatusOnInternalStatusChange(invln_Loanapplication target, invln_Loanapplication preImage);
        void SendEmailToNewOwner(invln_Loanapplication target, invln_Loanapplication preImage);
        string GetFileLocationForApplicationLoan(string loanApplicationId);
        void SetLastModificationDate(invln_Loanapplication target);
        void CreateDocumentLocation(invln_Loanapplication target);
        bool CheckIfLoanApplicationWithGivenNameExists(string loanName, string organisationId);
        void AssignLoanToTmTeam(invln_Loanapplication target);
        void CreateStandardConditions(invln_Loanapplication target);
    }
}
