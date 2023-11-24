using HE.Investments.Account.Shared;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.Contract.Application.Queries;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.QueryHandlers;

public class IsLoanApplicationExistQueryHandler : IRequestHandler<IsLoanApplicationExistQuery, bool>
{
    private readonly ILoanApplicationRepository _loanApplicationRepository;

    private readonly IAccountUserContext _loanUserContext;

    public IsLoanApplicationExistQueryHandler(ILoanApplicationRepository loanApplicationRepository, IAccountUserContext loanUserContext)
    {
        _loanApplicationRepository = loanApplicationRepository;
        _loanUserContext = loanUserContext;
    }

    public async Task<bool> Handle(IsLoanApplicationExistQuery request, CancellationToken cancellationToken)
    {
        return await _loanApplicationRepository.IsExist(request.LoanApplicationId, await _loanUserContext.GetSelectedAccount(), cancellationToken);
    }
}
