using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.Contract.Application.Queries;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Account.Shared;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.QueryHandlers;

public class GetSubmitLoanApplicationQueryHandler : IRequestHandler<GetSubmitLoanApplicationQuery, GetSubmitLoanApplicationQueryResponse>
{
    private readonly IAccountUserContext _loanUserContext;
    private readonly ILoanApplicationRepository _loanApplicationRepository;

    public GetSubmitLoanApplicationQueryHandler(
        IAccountUserContext loanUserContext,
        ILoanApplicationRepository loanApplicationRepository)
    {
        _loanUserContext = loanUserContext;
        _loanApplicationRepository = loanApplicationRepository;
    }

    public async Task<GetSubmitLoanApplicationQueryResponse> Handle(GetSubmitLoanApplicationQuery request, CancellationToken cancellationToken)
    {
        var selectedAccount = await _loanUserContext.GetSelectedAccount();
        var loanApplication = await _loanApplicationRepository.GetLoanApplication(request.LoanApplicationId, selectedAccount, cancellationToken);

        var response = new SubmitLoanApplication(
                            loanApplication.Id,
                            loanApplication.ReferenceNumber,
                            loanApplication.ExternalStatus,
                            loanApplication.IsEnoughHomesToBuild());

        return new GetSubmitLoanApplicationQueryResponse(response);
    }
}
