using HE.Investments.Account.Shared.User;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.PrefillData.Crm;

public interface ILoanPrefillDataCrmContext
{
    Task<FrontDoorProjectId?> GetFrontDoorProjectId(
        LoanApplicationId loanApplicationId,
        UserAccount userAccount,
        CancellationToken cancellationToken);
}
