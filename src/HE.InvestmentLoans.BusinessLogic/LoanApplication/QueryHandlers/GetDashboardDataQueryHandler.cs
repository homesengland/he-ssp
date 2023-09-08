using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Application.Queries;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.QueryHandlers;

public class GetDashboardDataQueryHandler : IRequestHandler<GetDashboardDataQuery, GetDashboardDataQueryResponse>
{
    private readonly ILoanUserContext _loanUserContext;

    private readonly ILoanApplicationRepository _loanApplicationRepository;

    public GetDashboardDataQueryHandler(ILoanUserContext loanUserContext, ILoanApplicationRepository loanApplicationRepository)
    {
        _loanUserContext = loanUserContext;
        _loanApplicationRepository = loanApplicationRepository;
    }

    public async Task<GetDashboardDataQueryResponse> Handle(GetDashboardDataQuery request, CancellationToken cancellationToken)
    {
        var selectedAccount = await _loanUserContext.GetSelectedAccount();
        var userLoanApplications = (await _loanApplicationRepository.LoadAllLoanApplications(selectedAccount, cancellationToken)).OrderByDescending(application => application.LastModificationDate ?? DateTime.MinValue).ToList();

        return new GetDashboardDataQueryResponse(userLoanApplications, selectedAccount?.AccountName);
    }
}
