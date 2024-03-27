using HE.Investments.Account.Shared.User;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.Loans.BusinessLogic.PrefillData.Data;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.PrefillData.Repositories;

public interface ILoanPrefillDataRepository
{
    Task<LoanApplicationPrefillData> GetLoanApplicationPrefillData(
        FrontDoorProjectId projectId,
        UserAccount userAccount,
        CancellationToken cancellationToken);

    Task<LoanProjectPrefillData> GetLoanProjectPrefillData(
        LoanApplicationId applicationId,
        FrontDoorSiteId siteId,
        UserAccount userAccount,
        CancellationToken cancellationToken);
}
