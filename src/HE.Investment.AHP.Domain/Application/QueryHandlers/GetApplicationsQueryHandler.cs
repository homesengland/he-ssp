using HE.Investment.AHP.Contract.Application.Queries;
using MediatR;

namespace HE.Investment.AHP.Domain.Application.QueryHandlers;

public class GetApplicationsQueryHandler : IRequestHandler<GetApplicationsQuery, IList<Contract.Application.Application>>
{
    private readonly IApplicationRepository _repository;

    public GetApplicationsQueryHandler(IApplicationRepository repository)
    {
        _repository = repository;
    }

    public async Task<IList<Contract.Application.Application>> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
    {
        var schemes = await _repository.GetAll(cancellationToken);

        return schemes.Select(s => new Contract.Application.Application(s.Id.Value, s.Name.Name, s.Tenure?.ToString())).ToList();
    }
}
