using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Contract.Services.Interfaces;
using HE.InvestmentLoans.Common.Utils.Constants.Notification;
using HE.InvestmentLoans.Contract.Application.Commands;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.CommandHandlers;

public class SubmitLoanApplicationCommandHandler : IRequestHandler<SubmitLoanApplicationCommand>
{
    private readonly ILoanApplicationRepository _loanApplicationRepository;
    private readonly ICanSubmitLoanApplication _canSubmitLoanApplication;

    private readonly ILoanUserContext _loanUserContext;
    private readonly INotificationService _notificationService;

    public SubmitLoanApplicationCommandHandler(ILoanApplicationRepository loanApplicationRepository, ILoanUserContext loanUserContext, ICanSubmitLoanApplication canSubmitLoanApplication, INotificationService notificationService)
    {
        _loanApplicationRepository = loanApplicationRepository;
        _loanUserContext = loanUserContext;
        _canSubmitLoanApplication = canSubmitLoanApplication;
        _notificationService = notificationService;
    }

    public async Task Handle(SubmitLoanApplicationCommand request, CancellationToken cancellationToken)
    {
        var loanApplication = await _loanApplicationRepository
                                .GetLoanApplication(request.LoanApplicationId, await _loanUserContext.GetSelectedAccount(), cancellationToken);

        var applicationWasSubmittedPreviously = loanApplication.WasSubmitted();

        await loanApplication.Submit(_canSubmitLoanApplication, cancellationToken);

        if (applicationWasSubmittedPreviously)
        {
            await _notificationService.NotifySuccess(NotificationBodyType.ApplicationResubmitted);
        }
    }
}
