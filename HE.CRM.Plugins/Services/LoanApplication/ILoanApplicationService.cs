using HE.Base.Services;

namespace HE.CRM.Plugins.Services.LoanApplication
{
    public interface ILoanApplicationService : ICrmService
    {
        void CreateRecordFromPortal(string payload);
    }
}
