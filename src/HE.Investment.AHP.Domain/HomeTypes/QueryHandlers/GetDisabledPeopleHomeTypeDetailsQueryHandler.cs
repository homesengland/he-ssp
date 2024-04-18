using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Mappers;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class GetDisabledPeopleHomeTypeDetailsQueryHandler :
    GetHomeTypeSegmentQueryHandlerBase<GetDisabledPeopleHomeTypeDetailsQuery, DisabledPeopleHomeTypeDetailsSegmentEntity, DisabledPeopleHomeTypeDetails>
{
    public GetDisabledPeopleHomeTypeDetailsQueryHandler(
        IHomeTypeRepository repository,
        IHomeTypeSegmentContractMapper<DisabledPeopleHomeTypeDetailsSegmentEntity, DisabledPeopleHomeTypeDetails> mapper,
        IAccountUserContext accountUserContext)
        : base(repository, mapper, accountUserContext)
    {
    }

    protected override DisabledPeopleHomeTypeDetailsSegmentEntity GetSegment(IHomeTypeEntity homeType) => homeType.DisabledPeopleHomeTypeDetails;
}
