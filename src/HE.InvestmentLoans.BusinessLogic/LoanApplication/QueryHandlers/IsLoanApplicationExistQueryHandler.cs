using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Application.Queries;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.QueryHandlers;

public class IsLoanApplicationExistQueryHandler : IRequestHandler<IsLoanApplicationExistQuery, bool>
{
    private readonly ILoanApplicationRepository _loanApplicationRepository;

    private readonly ILoanUserContext _loanUserContext;

    public IsLoanApplicationExistQueryHandler(ILoanApplicationRepository loanApplicationRepository, ILoanUserContext loanUserContext)
    {
        _loanApplicationRepository = loanApplicationRepository;
        _loanUserContext = loanUserContext;
    }

    public async Task<bool> Handle(IsLoanApplicationExistQuery request, CancellationToken cancellationToken)
    {
        return await _loanApplicationRepository.IsExist(request.LoanApplicationId, await _loanUserContext.GetSelectedAccount(), cancellationToken);
    }
}
