using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class GetHomeTypesQueryHandler : IRequestHandler<GetHomeTypesQuery, IList<HomeTypeDetails>>
{
    private readonly IHomeTypeRepository _repository;

    public GetHomeTypesQueryHandler(IHomeTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IList<HomeTypeDetails>> Handle(GetHomeTypesQuery request, CancellationToken cancellationToken)
    {
        var homeTypes = await _repository.GetByApplicationId(
            new Domain.Application.ValueObjects.ApplicationId(request.ApplicationId),
            new[] { HomeTypeSegmentType.HomeInformation },
            cancellationToken);

        return homeTypes.HomeTypes.Select(Map).OrderBy(x => x.Name).ToList();
    }

    private static HomeTypeDetails Map(IHomeTypeEntity entity)
    {
        return new HomeTypeDetails(
            entity.Id!.Value,
            entity.Name?.Value,
            entity.HomeInformation.NumberOfHomes?.Value,
            entity.HousingType);
    }
}
