using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.QueryHandlers;

public class GetLoanApplicationQueryHandler : IRequestHandler<GetLoanApplicationQuery, GetLoanApplicationQueryResponse>
{
    private readonly ILoanApplicationRepository _loanApplicationRepository;

    private readonly ILoanUserContext _loanUserContext;

    public GetLoanApplicationQueryHandler(ILoanApplicationRepository loanApplicationRepository, ILoanUserContext loanUserContext)
    {
        _loanApplicationRepository = loanApplicationRepository;
        _loanUserContext = loanUserContext;
    }

    public async Task<GetLoanApplicationQueryResponse> Handle(GetLoanApplicationQuery request, CancellationToken cancellationToken)
    {
        var loanApplication = await _loanApplicationRepository.GetLoanApplication(request.Id, await _loanUserContext.GetSelectedAccount(), cancellationToken);

        return new GetLoanApplicationQueryResponse(loanApplication.LegacyModel);
    }
}
