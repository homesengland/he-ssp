using HE.InvestmentLoans.BusinessLogic.Security.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Security.CommandHandler;
public class SecurityBaseCommandHandler
{
    private readonly ISecurityRepository _repository;

    private readonly ILoanUserContext _loanUserContext;

    public SecurityBaseCommandHandler(ISecurityRepository repository, ILoanUserContext loanUserContext)
    {
        _repository = repository;
        _loanUserContext = loanUserContext;
    }

    protected async Task<OperationResult> Perform(Action<SecurityEntity> action, LoanApplicationId loanApplicationId, CancellationToken cancellationToken)
    {
        var userAccount = await _loanUserContext.GetSelectedAccount();
        var security = await _repository.GetAsync(loanApplicationId, userAccount, cancellationToken);

        try
        {
            action(security);
        }
        catch (DomainValidationException domainValidationException)
        {
            return domainValidationException.OperationResult;
        }

        await _repository.SaveAsync(security, userAccount, cancellationToken);
        return OperationResult.Success();
    }
}
