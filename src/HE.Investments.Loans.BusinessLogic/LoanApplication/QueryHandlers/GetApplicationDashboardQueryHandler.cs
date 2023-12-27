using HE.Investments.Account.Shared;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.Contract.Application.Queries;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication.QueryHandlers;

public class GetApplicationDashboardQueryHandler : IRequestHandler<GetApplicationDashboardQuery, GetApplicationDashboardQueryResponse>
{
    private readonly ILoanApplicationRepository _loanApplicationRepository;

    private readonly IAccountUserContext _loanUserContext;

    public GetApplicationDashboardQueryHandler(ILoanApplicationRepository loanApplicationRepository, IAccountUserContext loanUserContext)
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
            account.OrganisationName,
            loanApplication.LastModificationDate,
            loanApplication.LastModifiedBy);
    }
}
