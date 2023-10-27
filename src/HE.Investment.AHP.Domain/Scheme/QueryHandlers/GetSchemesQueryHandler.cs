using HE.Investment.AHP.Contract.Scheme.Queries;
using MediatR;

namespace HE.Investment.AHP.Domain.Scheme.QueryHandlers;

public class GetSchemesQueryHandler : IRequestHandler<GetSchemesQuery, IList<Contract.Scheme.Scheme>>
{
    private readonly ISchemeRepository _repository;

    public GetSchemesQueryHandler(ISchemeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IList<Contract.Scheme.Scheme>> Handle(GetSchemesQuery request, CancellationToken cancellationToken)
    {
        var schemes = await _repository.GetAll(cancellationToken);

        return schemes.Select(s => new Contract.Scheme.Scheme(s.Id.Value, s.Name.Name, s.Tenure?.ToString())).ToList();
    }
}
