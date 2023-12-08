using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Mappers;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class GetDesignPlansQueryHandler : GetHomeTypeSegmentQueryHandlerBase<GetDesignPlansQuery, DesignPlansSegmentEntity, DesignPlans>
{
    public GetDesignPlansQueryHandler(IHomeTypeRepository repository, IHomeTypeSegmentContractMapper<DesignPlansSegmentEntity, DesignPlans> mapper)
        : base(repository, mapper)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> Segments => new[] { HomeTypeSegmentType.DesignPlans };

    protected override DesignPlansSegmentEntity GetSegment(IHomeTypeEntity homeType) => homeType.DesignPlans;
}
