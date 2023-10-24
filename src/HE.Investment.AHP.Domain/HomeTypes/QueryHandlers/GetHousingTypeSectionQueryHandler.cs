using HE.Investment.AHP.BusinessLogic.HomeTypes.Entities;
using HE.Investment.AHP.BusinessLogic.HomeTypes.Mappers;
using HE.Investment.AHP.BusinessLogic.HomeTypes.ValueObjects;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using MediatR;

namespace HE.Investment.AHP.BusinessLogic.HomeTypes.QueryHandlers;

internal sealed class GetHousingTypeSectionQueryHandler : IRequestHandler<GetHousingTypeSectionQuery, HousingTypeSection>
{
    private readonly IHomeTypeRepository _repository;

    private readonly IHomeTypeSectionMapper<HousingTypeSection> _mapper;

    public GetHousingTypeSectionQueryHandler(IHomeTypeRepository repository, IHomeTypeSectionMapper<HousingTypeSection> mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<HousingTypeSection> Handle(GetHousingTypeSectionQuery request, CancellationToken cancellationToken)
    {
        var homeType = await _repository.Get(
            new HomeTypeId(request.HomeTypeId),
            HomeTypeSectionType.HousingType,
            cancellationToken);

        return _mapper.Map(homeType);
    }
}
