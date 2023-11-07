using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.Security.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.Events;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Security.CommandHandler;
public class SecurityBaseCommandHandler
{
    private readonly ISecurityRepository _securityRepository;

    private readonly ILoanApplicationRepository _loanApplicationRepository;

    private readonly ILoanUserContext _loanUserContext;

    private readonly ILogger<SecurityBaseCommandHandler> _logger;

    public SecurityBaseCommandHandler(ISecurityRepository securityRepository, ILoanApplicationRepository loanApplicationRepository, ILoanUserContext loanUserContext, ILogger<SecurityBaseCommandHandler> logger)
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
