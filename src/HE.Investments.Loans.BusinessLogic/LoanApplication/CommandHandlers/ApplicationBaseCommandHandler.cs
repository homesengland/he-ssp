using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Entities;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.CommandHandlers;

public class ApplicationBaseCommandHandler
{
    private readonly ILoanApplicationRepository _loanApplicationRepository;

    private readonly IAccountUserContext _loanUserContext;

    public ApplicationBaseCommandHandler(
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext)
    {
        _loanApplicationRepository = loanApplicationRepository;
        _loanUserContext = loanUserContext;
    }

    protected async Task<OperationResult> Perform(
        Func<LoanApplicationEntity, Task> action,
        LoanApplicationId loanApplicationId,
        CancellationToken cancellationToken)
    {
        var userAccount = await _loanUserContext.GetSelectedAccount();
        var loanApplication =
            await _loanApplicationRepository.GetLoanApplication(loanApplicationId, userAccount, cancellationToken);

        await action(loanApplication);

        return OperationResult.Success();
    }
}
