using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.HomeTypes.Crm.Segments;

public class HomeInformationCrmSegmentMapper : HomeTypeCrmSegmentMapperBase<HomeInformationSegmentEntity>
{
    public HomeInformationCrmSegmentMapper()
        : base(new[]
        {
            nameof(invln_HomeType.invln_numberofhomeshometype),
            nameof(invln_HomeType.invln_numberofbedrooms),
            nameof(invln_HomeType.invln_maxoccupancy),
            nameof(invln_HomeType.invln_numberofstoreys),
        })
    {
    }

    public override HomeTypeSegmentType SegmentType => HomeTypeSegmentType.HomeInformation;

    public override IHomeTypeSegmentEntity MapToEntity(ApplicationBasicInfo application, HomeTypeDto dto)
    {
        // TODO: map rest properties
        return new HomeInformationSegmentEntity(dto.numberOfHomes.IsProvided() ? new NumberOfHomes(dto.numberOfHomes!.Value) : null);
    }

    protected override HomeInformationSegmentEntity GetSegment(HomeTypeEntity entity) => entity.HomeInformation;

    protected override void MapToDto(HomeTypeDto dto, HomeInformationSegmentEntity segment)
    {
        // TODO: map rest properties
        dto.numberOfHomes = segment.NumberOfHomes?.Value;
    }
}
