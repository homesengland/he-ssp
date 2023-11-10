using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Extensions;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class GetHomeTypeQueryHandler : IRequestHandler<GetHomeTypeQuery, HomeType>
{
    private readonly IHomeTypeRepository _repository;

    public GetHomeTypeQueryHandler(IHomeTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<HomeType> Handle(GetHomeTypeQuery request, CancellationToken cancellationToken)
    {
        var homeType = await _repository.GetById(
            new Domain.Application.ValueObjects.ApplicationId(request.ApplicationId),
            new HomeTypeId(request.HomeTypeId),
            HomeTypeSegmentTypes.None,
            cancellationToken);

        return new HomeType
        {
            HomeTypeId = request.HomeTypeId,
            HomeTypeName = homeType.Name?.Value,
            HousingType = homeType.HousingType.MapTo<HousingType>(),
        };
    }
}
