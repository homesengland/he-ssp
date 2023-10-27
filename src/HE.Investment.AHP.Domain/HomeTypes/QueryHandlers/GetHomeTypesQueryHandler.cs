using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class GetHomeTypesQueryHandler : IRequestHandler<GetHomeTypesQuery, IList<HomeTypeBasicDetails>>
{
    private readonly IHomeTypesRepository _repository;

    public GetHomeTypesQueryHandler(IHomeTypesRepository repository)
    {
        _repository = repository;
    }

    public async Task<IList<HomeTypeBasicDetails>> Handle(GetHomeTypesQuery request, CancellationToken cancellationToken)
    {
        var homeTypes = await _repository.GetBySchemeId(request.SchemeId, cancellationToken);

        return homeTypes.HomeTypes.Select(Map).ToList();
    }

    private static HomeTypeBasicDetails Map(HomeTypeBasicDetailsEntity entity)
    {
        return new HomeTypeBasicDetails(
            entity.Id.Value,
            entity.Name?.Value,
            entity.NumberOfHomes,
            entity.HousingType);
    }
}
