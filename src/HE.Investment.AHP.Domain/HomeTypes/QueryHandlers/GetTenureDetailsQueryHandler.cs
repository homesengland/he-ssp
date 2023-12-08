using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Mappers;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class GetTenureDetailsQueryHandler
    : GetHomeTypeSegmentQueryHandlerBase<GetTenureDetailsQuery, TenureDetailsSegmentEntity, TenureDetails>
{
    public GetTenureDetailsQueryHandler(
        IHomeTypeRepository repository,
        IHomeTypeSegmentContractMapper<TenureDetailsSegmentEntity, TenureDetails> mapper)
        : base(repository, mapper)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> Segments => new[] { HomeTypeSegmentType.TenureDetails };

    protected override TenureDetailsSegmentEntity GetSegment(IHomeTypeEntity homeType) => homeType.TenureDetails;
}
