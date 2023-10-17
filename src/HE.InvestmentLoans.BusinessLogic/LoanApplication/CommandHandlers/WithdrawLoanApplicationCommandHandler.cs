using HE.InvestmentLoans.BusinessLogic.CompanyStructure.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Contract.Services.Interfaces;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Models.App;
using HE.InvestmentLoans.Common.Utils.Constants.Notification;
using HE.InvestmentLoans.Common.Utils.Enums;
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
    private readonly IAppConfig _appConfig;

    public WithdrawLoanApplicationCommandHandler(
        ILoanApplicationRepository loanApplicationRepository,
        ILoanUserContext loanUserContext,
        ILogger<CompanyStructureBaseCommandHandler> logger,
        INotificationService notificationService,
        IAppConfig appConfig)
    {
        _loanApplicationRepository = loanApplicationRepository;
        _loanUserContext = loanUserContext;
        _logger = logger;
        _notificationService = notificationService;
        _appConfig = appConfig;
    }

    public async Task<OperationResult> Handle(WithdrawLoanApplicationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var loanApplication = await _loanApplicationRepository
                                .GetLoanApplication(request.LoanApplicationId, await _loanUserContext.GetSelectedAccount(), cancellationToken);
            var withdrawReason = WithdrawReason.New(request.WithdrawReason);
            var valuesToDisplay = new Dictionary<NotificationServiceKeys, string>
            {
                { NotificationServiceKeys.Name, loanApplication.Name },
                { NotificationServiceKeys.Email, _appConfig.FundingSupportEmail ?? "funding support" },
            };

            await loanApplication.Withdraw(_loanApplicationRepository, withdrawReason, cancellationToken);

            _notificationService.NotifySuccess(NotificationBodyType.WithdrawApplication, valuesToDisplay);

            return OperationResult.Success();
        }
        catch (DomainValidationException domainValidationException)
        {
            _logger.LogWarning(domainValidationException, "Validation error(s) occured: {Message}", domainValidationException.Message);
            return domainValidationException.OperationResult;
        }
    }
}
