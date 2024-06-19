using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Mappers;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Consortium.Shared.UserContext;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class GetTenureDetailsQueryHandler
    : GetHomeTypeSegmentQueryHandlerBase<GetTenureDetailsQuery, TenureDetailsSegmentEntity, TenureDetails>
{
    public GetTenureDetailsQueryHandler(
        IHomeTypeRepository repository,
        IHomeTypeSegmentContractMapper<TenureDetailsSegmentEntity, TenureDetails> mapper,
        IConsortiumUserContext accountUserContext)
        : base(repository, mapper, accountUserContext)
    {
    }

    protected override TenureDetailsSegmentEntity GetSegment(IHomeTypeEntity homeType) => homeType.TenureDetails;
}
