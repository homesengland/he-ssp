using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.Contract.Application.Commands;
using HE.InvestmentLoans.Contract.Application.Events;
using HE.Investments.Account.Shared;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.CommandHandlers;

public class SubmitLoanApplicationCommandHandler : IRequestHandler<SubmitLoanApplicationCommand>
{
    private readonly ILoanApplicationRepository _loanApplicationRepository;
    private readonly ICanSubmitLoanApplication _canSubmitLoanApplication;
    private readonly IAccountUserContext _loanUserContext;

    public SubmitLoanApplicationCommandHandler(ILoanApplicationRepository loanApplicationRepository, IAccountUserContext loanUserContext, ICanSubmitLoanApplication canSubmitLoanApplication)
    {
        _loanApplicationRepository = loanApplicationRepository;
        _loanUserContext = loanUserContext;
        _canSubmitLoanApplication = canSubmitLoanApplication;
    }

    public async Task Handle(SubmitLoanApplicationCommand request, CancellationToken cancellationToken)
    {
        var loanApplication = await _loanApplicationRepository
                                .GetLoanApplication(request.LoanApplicationId, await _loanUserContext.GetSelectedAccount(), cancellationToken);

        var applicationWasSubmittedPreviously = loanApplication.WasSubmitted();

        await loanApplication.Submit(_canSubmitLoanApplication, cancellationToken);

        if (applicationWasSubmittedPreviously)
        {
            loanApplication.Publish(new LoanApplicationHasBeenResubmittedEvent());
            await _loanApplicationRepository.DispatchEvents(loanApplication, cancellationToken);
        }
    }
}
