using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Application.Queries;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.QueryHandlers;

public class GetApplicationDashboardQueryHandler : IRequestHandler<GetApplicationDashboardQuery, GetApplicationDashboardQueryResponse>
{
    private readonly ILoanApplicationRepository _loanApplicationRepository;

    private readonly ILoanUserContext _loanUserContext;

    public GetApplicationDashboardQueryHandler(ILoanApplicationRepository loanApplicationRepository, ILoanUserContext loanUserContext)
    {
        _loanApplicationRepository = loanApplicationRepository;
        _loanUserContext = loanUserContext;
    }

    public async Task<GetApplicationDashboardQueryResponse> Handle(GetApplicationDashboardQuery request, CancellationToken cancellationToken)
    {
        var account = await _loanUserContext.GetSelectedAccount();
        var loanApplication = await _loanApplicationRepository.GetLoanApplication(request.ApplicationId, account, cancellationToken);

        return new GetApplicationDashboardQueryResponse(
            request.ApplicationId,
            loanApplication.Name,
            loanApplication.ExternalStatus,
            loanApplication.ReferenceNumber,
            account.AccountName,
            loanApplication.LastModificationDate);
    }
}
