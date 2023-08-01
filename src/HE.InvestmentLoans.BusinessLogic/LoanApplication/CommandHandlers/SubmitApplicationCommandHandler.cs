using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Contract;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.CommandHandlers;

public class SubmitApplicationCommandHandler : IRequestHandler<SubmitApplicationCommand>
{
    private readonly ILoanApplicationRepository _loanApplicationRepository;

    private readonly ILoanUserContext _loanUserContext;

    public SubmitApplicationCommandHandler(ILoanApplicationRepository loanApplicationRepository, ILoanUserContext loanUserContext)
    {
        _loanApplicationRepository = loanApplicationRepository;
        _loanUserContext = loanUserContext;
    }

    public async Task Handle(SubmitApplicationCommand request, CancellationToken cancellationToken)
    {
        _loanApplicationRepository.Save(request.LoanApplication.LegacyModel, await _loanUserContext.GetSelectedAccount());

        if (request.LoanApplication.IsReadyToSubmit())
        {
            _loanApplicationRepository.Submit(request.LoanApplication, cancellationToken);
        }
        else
        {
            throw new DomainException("Loan application is not ready to be submitted", CommonErrorCodes.LoanApplicationNotReadyToSubmit);
        }
    }
}
