using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Mappers;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class GetModernMethodsConstructionQueryHandler :
    GetHomeTypeSegmentQueryHandlerBase<GetModernMethodsConstructionQuery, ModernMethodsConstructionSegmentEntity, ModernMethodsConstruction>
{
    public GetModernMethodsConstructionQueryHandler(
        IHomeTypeRepository repository,
        IHomeTypeSegmentContractMapper<ModernMethodsConstructionSegmentEntity, ModernMethodsConstruction> mapper,
        IAccountUserContext accountUserContext)
        : base(repository, mapper, accountUserContext)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> Segments => new[] { HomeTypeSegmentType.ModernMethodsConstruction };

    protected override ModernMethodsConstructionSegmentEntity GetSegment(IHomeTypeEntity homeType) => homeType.ModernMethodsConstruction;
}
