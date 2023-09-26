using HE.InvestmentLoans.BusinessLogic.Security.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Security.CommandHandler;
public class SecurityBaseCommandHandler
{
    private readonly ISecurityRepository _repository;

    private readonly ILoanUserContext _loanUserContext;

    private readonly ILogger<SecurityBaseCommandHandler> _logger;

    public SecurityBaseCommandHandler(ISecurityRepository repository, ILoanUserContext loanUserContext, ILogger<SecurityBaseCommandHandler> logger)
    {
        _repository = repository;
        _loanUserContext = loanUserContext;
        _logger = logger;
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
            _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);
            return domainValidationException.OperationResult;
        }

        await _repository.SaveAsync(security, userAccount, cancellationToken);
        return OperationResult.Success();
    }
}
