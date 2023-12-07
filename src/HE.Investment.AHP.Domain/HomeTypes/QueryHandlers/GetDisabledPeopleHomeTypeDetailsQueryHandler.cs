using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Mappers;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class GetDisabledPeopleHomeTypeDetailsQueryHandler :
    GetHomeTypeSegmentQueryHandlerBase<GetDisabledPeopleHomeTypeDetailsQuery, DisabledPeopleHomeTypeDetailsSegmentEntity, DisabledPeopleHomeTypeDetails>
{
    public GetDisabledPeopleHomeTypeDetailsQueryHandler(
        IHomeTypeRepository repository,
        IHomeTypeSegmentContractMapper<DisabledPeopleHomeTypeDetailsSegmentEntity, DisabledPeopleHomeTypeDetails> mapper)
        : base(repository, mapper)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> Segments => new[] { HomeTypeSegmentType.DisabledAndVulnerablePeople };

    protected override DisabledPeopleHomeTypeDetailsSegmentEntity GetSegment(IHomeTypeEntity homeType) => homeType.DisabledPeopleHomeTypeDetails;
}
