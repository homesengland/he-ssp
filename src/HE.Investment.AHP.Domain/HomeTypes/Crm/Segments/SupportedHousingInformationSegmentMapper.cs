using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.Common;
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
            MapLocalCommissioningBodiesConsulted(dto.localComissioningBodies),
            MapShortStayAccommodation(dto.shortStayAccommodation),
            MapRevenueFunding(dto.revenueFunding));
    }

    protected override SupportedHousingInformationEntity GetSegment(HomeTypeEntity entity) => entity.SupportedHousingInformation;

    protected override void MapToDto(HomeTypeDto dto, SupportedHousingInformationEntity segment)
    {
        dto.localComissioningBodies = MapLocalCommissioningBodiesConsulted(segment.LocalCommissioningBodiesConsulted);
        dto.shortStayAccommodation = MapShortStayAccommodation(segment.ShortStayAccommodation);
        dto.revenueFunding = MapRevenueFunding(segment.RevenueFundingType);
    }

    private static bool? MapLocalCommissioningBodiesConsulted(YesNoType? value)
    {
        return value switch
        {
            YesNoType.Yes => true,
            YesNoType.No => false,
            _ => null,
        };
    }

    private static YesNoType MapLocalCommissioningBodiesConsulted(bool? value)
    {
        return value switch
        {
            true => YesNoType.Yes,
            false => YesNoType.No,
            _ => YesNoType.Undefined,
        };
    }

    private static bool? MapShortStayAccommodation(YesNoType? value)
    {
        return value switch
        {
            YesNoType.Yes => true,
            YesNoType.No => false,
            _ => null,
        };
    }

    private static YesNoType MapShortStayAccommodation(bool? value)
    {
        return value switch
        {
            true => YesNoType.Yes,
            false => YesNoType.No,
            _ => YesNoType.Undefined,
        };
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
