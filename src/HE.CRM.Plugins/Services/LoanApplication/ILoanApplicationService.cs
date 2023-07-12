using HE.Base.Services;

namespace HE.CRM.Plugins.Services.LoanApplication
{
    public interface ILoanApplicationService : ICrmService
    {
        string CreateRecordFromPortal(string contactExternalId, string accountId, string loanApplicationId, string loanApplicationPayload);
        string GetLoanApplicationsForAccountAndContact(string externalContactId, string accountId, string loanApplicationId = null);
        void ChangeLoanApplicationExternalStatus(int externalStatus, string loanApplicationId);
    }
}
