using HE.InvestmentLoans.BusinessLogic.CompanyStructure.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Services.Interfaces;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.Commands;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.CommandHandlers;

public class WithdrawLoanApplicationCommandHandler : IRequestHandler<WithdrawLoanApplicationCommand, OperationResult>
{
    private readonly ILoanApplicationRepository _loanApplicationRepository;
    private readonly ILoanUserContext _loanUserContext;
    private readonly ILogger<CompanyStructureBaseCommandHandler> _logger;
    private readonly INotificationService _notificationService;

    public WithdrawLoanApplicationCommandHandler(
        ILoanApplicationRepository loanApplicationRepository,
        ILoanUserContext loanUserContext,
        ILogger<CompanyStructureBaseCommandHandler> logger,
        INotificationService notificationService)
    {
        _loanApplicationRepository = loanApplicationRepository;
        _loanUserContext = loanUserContext;
        _logger = logger;
        _notificationService = notificationService;
    }

    public async Task<OperationResult> Handle(WithdrawLoanApplicationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var loanApplication = await _loanApplicationRepository
                                .GetLoanApplication(request.LoanApplicationId, await _loanUserContext.GetSelectedAccount(), cancellationToken);
            var withdrawReason = WithdrawReason.New(request.WithdrawReason);

            await loanApplication.Withdraw(_loanApplicationRepository, withdrawReason, cancellationToken);

            _notificationService.Add(NotificationServiceKey.LoanApplicationWithdraw, loanApplication.Name);

            return OperationResult.Success();
        }
        catch (DomainValidationException domainValidationException)
        {
            _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);
            return domainValidationException.OperationResult;
        }
    }
}
