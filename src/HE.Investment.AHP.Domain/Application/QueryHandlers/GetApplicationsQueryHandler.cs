using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investments.Account.Shared;
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
        var applications = await _repository.GetAll(cancellationToken);
        var organisationName = (await _accountUserContext.GetSelectedAccount()).AccountName;

        var applicationsBasicDetails = applications.Select(s => new ApplicationBasicDetails(
                s.Id.Value,
                s.Name.Name,
                s.Status,
                null,
                null,
                null))
            .ToList();

        return new GetApplicationsQueryResult(organisationName, applicationsBasicDetails);
    }
}
