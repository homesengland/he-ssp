using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Mappers;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Consortium.Shared.UserContext;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class GetDesignPlansQueryHandler : GetHomeTypeSegmentQueryHandlerBase<GetDesignPlansQuery, DesignPlansSegmentEntity, DesignPlans>
{
    public GetDesignPlansQueryHandler(
        IHomeTypeRepository repository,
        IHomeTypeSegmentContractMapper<DesignPlansSegmentEntity, DesignPlans> mapper,
        IConsortiumUserContext accountUserContext)
        : base(repository, mapper, accountUserContext)
    {
    }

    protected override bool LoadFiles => true;

    protected override DesignPlansSegmentEntity GetSegment(IHomeTypeEntity homeType) => homeType.DesignPlans;
}
