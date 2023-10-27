using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Mappers;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

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
        var homeType = await _repository.GetById(
            request.SchemeId,
            new HomeTypeId(request.HomeTypeId),
            new[] { HomeTypeSectionType.HousingType },
            cancellationToken);

        return _mapper.Map(homeType);
    }
}
