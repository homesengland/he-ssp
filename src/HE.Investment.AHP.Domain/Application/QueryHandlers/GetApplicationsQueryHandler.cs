using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Domain.Application.Repositories.Interfaces;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Pagination;
using MediatR;

namespace HE.Investment.AHP.Domain.Application.QueryHandlers;

public class GetApplicationsQueryHandler : IRequestHandler<GetApplicationsQuery, GetApplicationsQueryResult>
{
    private readonly IAccountUserContext _accountUserContext;

    private readonly IApplicationRepository _repository;

    public GetApplicationsQueryHandler(IApplicationRepository repository, IAccountUserContext accountUserContext)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task<GetApplicationsQueryResult> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var applicationsWithPagination = await _repository.GetApplicationsWithFundingDetails(account, request.PaginationRequest, cancellationToken);

        var applicationsBasicDetails = applicationsWithPagination.Items
            .Select(s => new ApplicationBasicDetails(
                s.ApplicationId,
                s.ApplicationName,
                s.Status,
                null,
                s.RequiredFunding,
                s.HousesToDeliver))
            .ToList();

        return new GetApplicationsQueryResult(
            account.Organisation?.RegisteredCompanyName ?? string.Empty,
            new PaginationResult<ApplicationBasicDetails>(
                applicationsBasicDetails,
                applicationsWithPagination.CurrentPage,
                applicationsWithPagination.ItemsPerPage,
                applicationsWithPagination.TotalItems));
    }
}
