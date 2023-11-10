using HE.Investment.AHP.Contract.Scheme.Queries;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.InvestmentLoans.Common.Exceptions;
using MediatR;

namespace HE.Investment.AHP.Domain.Scheme.QueryHandlers;

public class GetSchemeQueryHandler : IRequestHandler<GetApplicationSchemeQuery, Contract.Scheme.Scheme?>
{
    private readonly ISchemeRepository _repository;

    public GetSchemeQueryHandler(ISchemeRepository repository)
    {
        _repository = repository;
    }

    public async Task<Contract.Scheme.Scheme?> Handle(GetApplicationSchemeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _repository.GetById(new(request.ApplicationId), cancellationToken);

            return new Contract.Scheme.Scheme(
                entity!.Funding.RequiredFunding,
                entity.Funding.HousesToDeliver,
                entity.AffordabilityEvidence?.Evidence,
                entity.SalesRisk?.Value,
                entity.HousingNeeds?.TypeAndTenureJustification,
                entity.HousingNeeds?.SchemeAndProposalJustification,
                entity.StakeholderDiscussions?.Report);
        }
        catch (NotFoundException)
        {
            return null;
        }
    }
}
