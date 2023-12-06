using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Mappers;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class GetOlderPeopleHomeTypeDetailsQueryHandler
    : GetHomeTypeSegmentQueryHandlerBase<GetOlderPeopleHomeTypeDetailsQuery, OlderPeopleHomeTypeDetailsSegmentEntity, OlderPeopleHomeTypeDetails>
{
    public GetOlderPeopleHomeTypeDetailsQueryHandler(
        IHomeTypeRepository repository,
        IHomeTypeSegmentContractMapper<OlderPeopleHomeTypeDetailsSegmentEntity, OlderPeopleHomeTypeDetails> mapper)
        : base(repository, mapper)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> Segments => new[] { HomeTypeSegmentType.OlderPeople };

    protected override OlderPeopleHomeTypeDetailsSegmentEntity GetSegment(IHomeTypeEntity homeType) => homeType.OlderPeopleHomeTypeDetails;
}
