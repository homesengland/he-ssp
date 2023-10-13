using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Contract.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.CommandHandlers;

public class SubmitLoanApplicationCommandHandler : IRequestHandler<SubmitLoanApplicationCommand>
{
    private readonly ILoanApplicationRepository _loanApplicationRepository;
    private readonly ICanSubmitLoanApplication _canSubmitLoanApplication;

    private readonly ILoanUserContext _loanUserContext;

    public SubmitLoanApplicationCommandHandler(ILoanApplicationRepository loanApplicationRepository, ILoanUserContext loanUserContext, ICanSubmitLoanApplication canSubmitLoanApplication)
    {
        _loanApplicationRepository = loanApplicationRepository;
        _loanUserContext = loanUserContext;
        _canSubmitLoanApplication = canSubmitLoanApplication;
    }

    public async Task Handle(SubmitLoanApplicationCommand request, CancellationToken cancellationToken)
    {
        var loanApplication = await _loanApplicationRepository
                                .GetLoanApplication(request.LoanApplicationId, await _loanUserContext.GetSelectedAccount(), cancellationToken);

        await loanApplication.Submit(_canSubmitLoanApplication, cancellationToken);
    }
}
