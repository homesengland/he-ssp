using HE.Investment.AHP.Contract.Scheme.Queries;
using MediatR;
using ContractScheme = HE.Investment.AHP.Contract.Scheme.Scheme;

namespace HE.Investment.AHP.Domain.Scheme.QueryHandlers;

public class GetSchemeQueryHandler : IRequestHandler<GetSchemeQuery, ContractScheme>
{
    private readonly ISchemeRepository _repository;

    public GetSchemeQueryHandler(ISchemeRepository repository)
    {
        _repository = repository;
    }

    public async Task<ContractScheme> Handle(GetSchemeQuery request, CancellationToken cancellationToken)
    {
        var scheme = await _repository.GetById(new(request.SchemeId), cancellationToken);

        return new ContractScheme(scheme.Id.Value, scheme.Name.Name, scheme.Tenure != null ? scheme.Tenure.Value.ToString() : null);
    }
}
