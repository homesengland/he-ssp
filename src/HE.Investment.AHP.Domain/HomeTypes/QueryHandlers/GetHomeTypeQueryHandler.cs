using HE.Investment.AHP.BusinessLogic.HomeTypes.Entities;
using HE.Investment.AHP.BusinessLogic.HomeTypes.Mappers;
using HE.Investment.AHP.BusinessLogic.HomeTypes.ValueObjects;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using MediatR;

namespace HE.Investment.AHP.BusinessLogic.HomeTypes.QueryHandlers;

internal sealed class GetHomeTypeQueryHandler : IRequestHandler<GetHomeTypeQuery, HomeType>
{
    private readonly IHomeTypeRepository _repository;

    private readonly IHomeTypeSectionMapper<HousingTypeSection> _housingTypeSectionMapper;

    public GetHomeTypeQueryHandler(
        IHomeTypeRepository repository,
        IHomeTypeSectionMapper<HousingTypeSection> housingTypeSectionMapper)
    {
        _repository = repository;
        _housingTypeSectionMapper = housingTypeSectionMapper;
    }

    public async Task<HomeType> Handle(GetHomeTypeQuery request, CancellationToken cancellationToken)
    {
        var homeType = await _repository.Get(
            new HomeTypeId(request.HomeTypeId),
            HomeTypeSectionType.All,
            cancellationToken);

        return new HomeType
        {
            HomeTypeId = request.HomeTypeId,
            HousingTypeSection = _housingTypeSectionMapper.Map(homeType),
        };
    }
}
