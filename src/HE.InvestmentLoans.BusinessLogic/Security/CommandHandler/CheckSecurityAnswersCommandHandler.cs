using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.Security.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.Events;
using HE.InvestmentLoans.Contract.Application.Extensions;
using HE.InvestmentLoans.Contract.Security.Commands;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Security.CommandHandler;
public class CheckSecurityAnswersCommandHandler : IRequestHandler<ConfirmSecuritySectionCommand, OperationResult>
{
    private readonly ISecurityRepository _securityRepository;

    private readonly ILoanApplicationRepository _loanApplicationRepository;

    private readonly ILoanUserContext _loanUserContext;

    private readonly ILogger<SecurityBaseCommandHandler> _logger;

    public CheckSecurityAnswersCommandHandler(
        ISecurityRepository securityRepository,
        ILoanApplicationRepository loanApplicationRepository,
        ILoanUserContext loanUserContext,
        ILogger<SecurityBaseCommandHandler> logger)
    {
        _securityRepository = securityRepository;
        _loanApplicationRepository = loanApplicationRepository;
        _loanUserContext = loanUserContext;
        _logger = logger;
    }

    public async Task<OperationResult> Handle(ConfirmSecuritySectionCommand request, CancellationToken cancellationToken)
    {
        var userAccount = await _loanUserContext.GetSelectedAccount();
        var security = await _securityRepository.GetAsync(request.Id, userAccount, SecurityFieldsSet.GetAllFields, cancellationToken);

        try
        {
            security.CheckAnswers(request.Answer.ToYesNoAnswer());

            if (security.Status == SectionStatus.Completed)
            {
                security.Publish(new LoanApplicationSectionHasBeenCompletedAgainEvent(request.Id));
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
