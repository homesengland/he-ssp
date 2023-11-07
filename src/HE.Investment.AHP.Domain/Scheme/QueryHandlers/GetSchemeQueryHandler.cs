using HE.Investment.AHP.Contract.Scheme.Queries;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using MediatR;

namespace HE.Investment.AHP.Domain.Scheme.QueryHandlers;

public class GetSchemeQueryHandler : IRequestHandler<GetSchemeQuery, Contract.Scheme.Scheme>
{
    private readonly ISchemeRepository _repository;

    public GetSchemeQueryHandler(ISchemeRepository repository)
    {
        _repository = repository;
    }

    public async Task<Contract.Scheme.Scheme> Handle(GetSchemeQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetById(new(request.SchemeId), cancellationToken);

        return new Contract.Scheme.Scheme(
            entity.Id.Value,
            entity.Funding.RequiredFunding,
            entity.Funding.HousesToDeliver,
            entity.AffordabilityEvidence?.Evidence,
            entity.SalesRisk?.Value,
            entity.HousingNeeds?.TypeAndTenureJustification,
            entity.HousingNeeds?.SchemeAndProposalJustification);
    }
}
