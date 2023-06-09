using HE.InvestmentLoans.BusinessLogic.Application.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Application.CommandHandlers;

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
        _loanApplicationRepository.Save(request.Model, new UserAccount(_loanUserContext.UserGlobalId, await _loanUserContext.GetSelectedAccountId()));
    }
}
