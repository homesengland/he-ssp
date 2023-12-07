using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Mappers;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;

namespace HE.Investment.AHP.Domain.HomeTypes.QueryHandlers;

internal sealed class GetSupportedHousingInformationQueryHandler
    : GetHomeTypeSegmentQueryHandlerBase<GetSupportedHousingInformationQuery, SupportedHousingInformationSegmentEntity, SupportedHousingInformation>
{
    public GetSupportedHousingInformationQueryHandler(
        IHomeTypeRepository repository,
        IHomeTypeSegmentContractMapper<SupportedHousingInformationSegmentEntity, SupportedHousingInformation> mapper)
        : base(repository, mapper)
    {
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> Segments => new[] { HomeTypeSegmentType.SupportedHousingInformation };

    protected override SupportedHousingInformationSegmentEntity GetSegment(IHomeTypeEntity homeType) => homeType.SupportedHousingInformation;
}
