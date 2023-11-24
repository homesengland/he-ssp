using HE.Investments.Account.Shared;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.Contract.Application.Queries;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.QueryHandlers;

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
