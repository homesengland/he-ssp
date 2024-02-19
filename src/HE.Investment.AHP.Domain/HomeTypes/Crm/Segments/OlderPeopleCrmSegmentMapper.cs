using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.HomeTypes.Crm.Segments;

public class OlderPeopleCrmSegmentMapper : HomeTypeCrmSegmentMapperBase<OlderPeopleHomeTypeDetailsSegmentEntity>
{
    public OlderPeopleCrmSegmentMapper()
        : base(new[] { nameof(invln_HomeType.invln_typeofolderpeopleshousing) })
    {
    }

    public override HomeTypeSegmentType SegmentType => HomeTypeSegmentType.OlderPeople;

    public override IHomeTypeSegmentEntity MapToEntity(ApplicationBasicInfo application, HomeTypeDto dto, IReadOnlyCollection<UploadedFile> uploadedFiles)
    {
        return new OlderPeopleHomeTypeDetailsSegmentEntity(MapHousingType(dto.housingTypeForOlderPeople));
    }

    protected override OlderPeopleHomeTypeDetailsSegmentEntity GetSegment(HomeTypeEntity entity) => entity.OlderPeopleHomeTypeDetails;

    protected override void MapToDto(HomeTypeDto dto, OlderPeopleHomeTypeDetailsSegmentEntity segment)
    {
        dto.housingTypeForOlderPeople = MapHousingType(segment.HousingType);
    }

    private static int? MapHousingType(OlderPeopleHousingType value)
    {
        return value switch
        {
            OlderPeopleHousingType.DesignatedSupportedHomes => (int)invln_OlderpeoplehousingtypeSet.Designatedsupportedhousing,
            OlderPeopleHousingType.HomesWithAccessToSupport => (int)invln_OlderpeoplehousingtypeSet.Housingforolderpeoplewithaccesstosupportasrequired,
            OlderPeopleHousingType.HomesWithSomeSpecialDesignFeatures => (int)invln_OlderpeoplehousingtypeSet.Housingforolderpeoplewithsomespecialdesignfeatures,
            OlderPeopleHousingType.HomesWithAllSpecialDesignFeatures => (int)invln_OlderpeoplehousingtypeSet.Housingforolderpeoplewithallspecialdesignfeatures,
            _ => null,
        };
    }

    private static OlderPeopleHousingType MapHousingType(int? value)
    {
        return value switch
        {
            (int)invln_OlderpeoplehousingtypeSet.Designatedsupportedhousing => OlderPeopleHousingType.DesignatedSupportedHomes,
            (int)invln_OlderpeoplehousingtypeSet.Housingforolderpeoplewithaccesstosupportasrequired => OlderPeopleHousingType.HomesWithAccessToSupport,
            (int)invln_OlderpeoplehousingtypeSet.Housingforolderpeoplewithsomespecialdesignfeatures => OlderPeopleHousingType.HomesWithSomeSpecialDesignFeatures,
            (int)invln_OlderpeoplehousingtypeSet.Housingforolderpeoplewithallspecialdesignfeatures => OlderPeopleHousingType.HomesWithAllSpecialDesignFeatures,
            _ => OlderPeopleHousingType.Undefined,
        };
    }
}
