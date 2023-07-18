using HE.InvestmentLoans.BusinessLogic.Application.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Application;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Application.QueryHandlers;

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
        var userLoanApplications = await _loanApplicationRepository.LoadAllLoanApplications(selectedAccount).OrderByDescending(application => application.LastModificationDate).ToList();

        return new GetDashboardDataQueryResponse(userLoanApplications, selectedAccount.Name);
    }
}
