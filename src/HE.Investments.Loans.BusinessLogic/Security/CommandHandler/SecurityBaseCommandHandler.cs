using HE.Investments.Account.Shared;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Validators;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Security.Repositories;
using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.Application.Enums;
using HE.Investments.Loans.Contract.Application.Events;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Loans.BusinessLogic.Security.CommandHandler;
public class SecurityBaseCommandHandler
{
    private readonly ISecurityRepository _securityRepository;

    private readonly ILoanApplicationRepository _loanApplicationRepository;

    private readonly IAccountUserContext _loanUserContext;

    private readonly ILogger<SecurityBaseCommandHandler> _logger;

    public SecurityBaseCommandHandler(ISecurityRepository securityRepository, ILoanApplicationRepository loanApplicationRepository, IAccountUserContext loanUserContext, ILogger<SecurityBaseCommandHandler> logger)
    {
        _securityRepository = securityRepository;
        _loanApplicationRepository = loanApplicationRepository;
        _loanUserContext = loanUserContext;
        _logger = logger;
    }

    protected async Task<OperationResult> Perform(Action<SecurityEntity> action, LoanApplicationId loanApplicationId, CancellationToken cancellationToken)
    {
        var userAccount = await _loanUserContext.GetSelectedAccount();
        var security = await _securityRepository.GetAsync(loanApplicationId, userAccount, SecurityFieldsSet.GetAllFields, cancellationToken);

        try
        {
            action(security);

            if (security.LoanApplicationStatus != ApplicationStatus.Draft)
            {
                security.Publish(new LoanApplicationChangeToDraftStatusEvent(loanApplicationId));
                await _loanApplicationRepository.DispatchEvents(security, cancellationToken);
            }
        }
        catch (DomainValidationException domainValidationException)
        {
            _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);
            return domainValidationException.OperationResult;
        }

        await _securityRepository.SaveAsync(security, userAccount, cancellationToken);
        return OperationResult.Success();
    }
}
