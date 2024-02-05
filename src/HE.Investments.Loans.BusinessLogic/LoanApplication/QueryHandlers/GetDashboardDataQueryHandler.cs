using HE.Investments.Account.Shared;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.Contract.Application.Queries;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.QueryHandlers;

public class GetDashboardDataQueryHandler : IRequestHandler<GetDashboardDataQuery, GetDashboardDataQueryResponse>
{
    private readonly IAccountUserContext _loanUserContext;

    private readonly ILoanApplicationRepository _loanApplicationRepository;

    public GetDashboardDataQueryHandler(IAccountUserContext loanUserContext, ILoanApplicationRepository loanApplicationRepository)
    {
        _loanUserContext = loanUserContext;
        _loanApplicationRepository = loanApplicationRepository;
    }

    public async Task<GetDashboardDataQueryResponse> Handle(GetDashboardDataQuery request, CancellationToken cancellationToken)
    {
        var selectedAccount = await _loanUserContext.GetSelectedAccount();
        var userLoanApplications = (await _loanApplicationRepository.LoadAllLoanApplications(selectedAccount, cancellationToken))
            .OrderByDescending(application => application.CreatedOn ?? DateTime.MinValue)
            .ThenByDescending(x => x.LastModificationDate)
            .ToList();

        return new GetDashboardDataQueryResponse(userLoanApplications, selectedAccount.Organisation?.RegisteredCompanyName ?? string.Empty);
    }
}
