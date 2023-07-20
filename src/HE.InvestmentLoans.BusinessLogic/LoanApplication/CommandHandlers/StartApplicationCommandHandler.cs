using HE.InvestmentLoans.BusinessLogic.Application.Entities;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Application.Commands;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.CommandHandlers;

public class StartApplicationCommandHandler : IRequestHandler<StartApplicationCommand, LoanApplicationId>
{
    private readonly ILoanUserContext _loanUserContext;

    private readonly ILoanApplicationRepository _applicationRepository;

    public StartApplicationCommandHandler(ILoanUserContext loanUserContext, ILoanApplicationRepository applicationRepository)
    {
        _loanUserContext = loanUserContext;
        _applicationRepository = applicationRepository;
    }

    public async Task<LoanApplicationId> Handle(StartApplicationCommand request, CancellationToken cancellationToken)
    {
        var userAccount = new UserAccount(_loanUserContext.UserGlobalId, await _loanUserContext.GetSelectedAccountId());
        var newLoanApplication = LoanApplicationEntity.New(userAccount);

        await _applicationRepository.Save(newLoanApplication);

        return newLoanApplication.Id;
    }
}
