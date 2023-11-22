using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Common.Mappers;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.HomeTypes.Crm.Segments;

public class SupportedHousingInformationSegmentMapper : HomeTypeCrmSegmentMapperBase<SupportedHousingInformationEntity>
{
    public SupportedHousingInformationSegmentMapper()
        : base(new[]
        {
            nameof(invln_HomeType.invln_localcommissioningbodiesconsulted),
            nameof(invln_HomeType.invln_homesusedforshortstay),
            nameof(invln_HomeType.invln_revenuefunding),
        })
    {
    }

    public override HomeTypeSegmentType SegmentType => HomeTypeSegmentType.SupportedHousingInformation;

    public override IHomeTypeSegmentEntity MapToEntity(ApplicationBasicInfo application, HomeTypeDto dto)
    {
        return new SupportedHousingInformationEntity(
            YesNoTypeMapper.Map(dto.localComissioningBodies),
            YesNoTypeMapper.Map(dto.shortStayAccommodation),
            MapRevenueFunding(dto.revenueFunding));
    }

    protected override SupportedHousingInformationEntity GetSegment(HomeTypeEntity entity) => entity.SupportedHousingInformation;

    protected override void MapToDto(HomeTypeDto dto, SupportedHousingInformationEntity segment)
    {
        dto.localComissioningBodies = YesNoTypeMapper.Map(segment.LocalCommissioningBodiesConsulted);
        dto.shortStayAccommodation = YesNoTypeMapper.Map(segment.ShortStayAccommodation);
        dto.revenueFunding = MapRevenueFunding(segment.RevenueFundingType);
    }

    private static int? MapRevenueFunding(RevenueFundingType? value)
    {
        return value switch
        {
            RevenueFundingType.RevenueFundingNeededAndIdentified => (int)invln_revenuefunding.Yesrevenuefundingisneededandhasbeenidentified,
            RevenueFundingType.RevenueFundingNeededButNotIdentified => (int)invln_revenuefunding.Revenuefundingisneededbuthasnotyetbeenidentified,
            RevenueFundingType.RevenueFundingNotNeeded => (int)invln_revenuefunding.Norevenuefundingisnotneeded,
            _ => null,
        };
    }

    private static RevenueFundingType MapRevenueFunding(int? value)
    {
        return value switch
        {
            (int)invln_revenuefunding.Yesrevenuefundingisneededandhasbeenidentified => RevenueFundingType.RevenueFundingNeededAndIdentified,
            (int)invln_revenuefunding.Revenuefundingisneededbuthasnotyetbeenidentified => RevenueFundingType.RevenueFundingNeededButNotIdentified,
            (int)invln_revenuefunding.Norevenuefundingisnotneeded => RevenueFundingType.RevenueFundingNotNeeded,
            _ => RevenueFundingType.Undefined,
        };
    }
}
