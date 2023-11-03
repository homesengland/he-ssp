using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

public class GetDesignPlansQueryHandler : IRequestHandler<GetDesignPlansQuery, DesignPlans>
{
    private readonly IHomeTypeRepository _repository;

    public GetDesignPlansQueryHandler(IHomeTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<DesignPlans> Handle(GetDesignPlansQuery request, CancellationToken cancellationToken)
    {
        var homeType = await _repository.GetById(
            request.ApplicationId,
            new HomeTypeId(request.HomeTypeId),
            new[] { HomeTypeSegmentType.DesignPlans },
            cancellationToken);

        return new DesignPlans(homeType.Name?.Value, homeType.DesignPlans.DesignPrinciples.ToList());
    }
}
