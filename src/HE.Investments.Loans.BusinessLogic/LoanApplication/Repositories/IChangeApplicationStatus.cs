using HE.Investments.Common.Contract;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;

public interface IChangeApplicationStatus
{
    Task ChangeApplicationStatus(LoanApplicationId loanApplicationId, ApplicationStatus applicationStatus, CancellationToken cancellationToken);
}
