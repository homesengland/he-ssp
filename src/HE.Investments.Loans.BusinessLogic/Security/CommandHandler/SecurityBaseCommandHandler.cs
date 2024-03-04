using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Security.Repositories;
using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.Application.Events;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.Security.CommandHandler;

public class SecurityBaseCommandHandler
{
    private readonly ISecurityRepository _securityRepository;

    private readonly ILoanApplicationRepository _loanApplicationRepository;

    private readonly IAccountUserContext _loanUserContext;

    public SecurityBaseCommandHandler(
        ISecurityRepository securityRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext)
    {
        _securityRepository = securityRepository;
        _loanApplicationRepository = loanApplicationRepository;
        _loanUserContext = loanUserContext;
    }

    protected async Task<OperationResult> Perform(Action<SecurityEntity> action, LoanApplicationId loanApplicationId, CancellationToken cancellationToken)
    {
        var userAccount = await _loanUserContext.GetSelectedAccount();
        var security = await _securityRepository.GetAsync(loanApplicationId, userAccount, SecurityFieldsSet.GetAllFields, cancellationToken);

        action(security);

        if (security.LoanApplicationStatus != ApplicationStatus.Draft)
        {
            security.Publish(new LoanApplicationChangeToDraftStatusEvent(loanApplicationId));
            await _loanApplicationRepository.DispatchEvents(security, cancellationToken);
        }

        await _securityRepository.SaveAsync(security, userAccount, cancellationToken);
        return OperationResult.Success();
    }
}
