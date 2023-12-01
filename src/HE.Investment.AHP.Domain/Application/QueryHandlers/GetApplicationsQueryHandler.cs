using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Domain.Application.Repositories;
using MediatR;

namespace HE.Investment.AHP.Domain.Application.QueryHandlers;

public class GetApplicationsQueryHandler : IRequestHandler<GetApplicationsQuery, IList<ApplicationBasicDetails>>
{
    private readonly IApplicationRepository _repository;

    public GetApplicationsQueryHandler(IApplicationRepository repository)
    {
        _repository = repository;
    }

    public async Task<IList<ApplicationBasicDetails>> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
    {
        var applications = await _repository.GetAll(cancellationToken);

        return applications.Select(s => new ApplicationBasicDetails(
                s.Id.Value,
                s.Name.Name,
                s.Tenure?.Value ?? default))
            .ToList();
    }
}
