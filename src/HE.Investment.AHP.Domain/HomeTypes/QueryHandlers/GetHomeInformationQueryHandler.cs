using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Mappers;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class GetHomeInformationQueryHandler :
    GetHomeTypeSegmentQueryHandlerBase<GetHomeInformationQuery, HomeInformationSegmentEntity, HomeInformation>
{
    public GetHomeInformationQueryHandler(IHomeTypeRepository repository, IHomeTypeSegmentContractMapper<HomeInformationSegmentEntity, HomeInformation> mapper)
        : base(repository, mapper)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> Segments => new[] { HomeTypeSegmentType.HomeInformation };

    protected override HomeInformationSegmentEntity GetSegment(IHomeTypeEntity homeType) => homeType.HomeInformation;
}
