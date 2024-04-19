using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.HomeTypes.Crm.Segments;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investments.Common.Contract;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.HomeTypes.Crm;

public class HomeTypeCrmMapper : IHomeTypeCrmMapper
{
    private readonly IList<IHomeTypeCrmSegmentMapper> _segmentMappers;

    public HomeTypeCrmMapper(IEnumerable<IHomeTypeCrmSegmentMapper> segmentMappers)
    {
        _segmentMappers = segmentMappers.ToList();
    }

    public HomeTypeEntity MapToDomain(
        ApplicationBasicInfo application,
        SiteBasicInfo site,
        HomeTypeDto dto,
        IDictionary<HomeTypeSegmentType, IReadOnlyCollection<UploadedFile>> uploadedFiles)
    {
        var segments = _segmentMappers
            .ToDictionary(
                x => x.SegmentType,
                x => x.MapToEntity(
                    application,
                    site,
                    dto,
                    uploadedFiles.TryGetValue(x.SegmentType, out var file) ? file : Array.Empty<UploadedFile>()));

        return new HomeTypeEntity(
            application,
            site,
            dto.homeTypeName,
            MapHousingType(dto.housingType),
            dto.isCompleted == true ? SectionStatus.Completed : SectionStatus.InProgress,
            new HomeTypeId(dto.id),
            dto.createdOn,
            (HomeInformationSegmentEntity)segments[HomeTypeSegmentType.HomeInformation],
            (DisabledPeopleHomeTypeDetailsSegmentEntity)segments[HomeTypeSegmentType.DisabledAndVulnerablePeople],
            (OlderPeopleHomeTypeDetailsSegmentEntity)segments[HomeTypeSegmentType.OlderPeople],
            (DesignPlansSegmentEntity)segments[HomeTypeSegmentType.DesignPlans],
            (SupportedHousingInformationSegmentEntity)segments[HomeTypeSegmentType.SupportedHousingInformation],
            (TenureDetailsSegmentEntity)segments[HomeTypeSegmentType.TenureDetails],
            (ModernMethodsConstructionSegmentEntity)segments[HomeTypeSegmentType.ModernMethodsConstruction]);
    }

    public HomeTypeDto MapToDto(HomeTypeEntity entity)
    {
        var homeTypeDto = new HomeTypeDto
        {
            id = entity.Id.IsNew ? null : entity.Id.Value,
            applicationId = entity.Application.Id.Value,
            homeTypeName = entity.Name.Value,
            housingType = MapHousingType(entity.HousingType),
            isCompleted = entity.Status == SectionStatus.Completed,
        };

        return _segmentMappers.Aggregate(homeTypeDto, (dto, segmentMapper) => segmentMapper.MapToDto(dto, entity));
    }

    private static int? MapHousingType(HousingType value)
    {
        return value switch
        {
            HousingType.General => (int)invln_Typeofhousing.General,
            HousingType.HomesForOlderPeople => (int)invln_Typeofhousing.Housingforolderpeople,
            HousingType.HomesForDisabledAndVulnerablePeople => (int)invln_Typeofhousing.Housingfordisabledandvulnerablepeople,
            _ => null,
        };
    }

    private static HousingType MapHousingType(int? value)
    {
        return value switch
        {
            (int)invln_Typeofhousing.General => HousingType.General,
            (int)invln_Typeofhousing.Housingforolderpeople => HousingType.HomesForOlderPeople,
            (int)invln_Typeofhousing.Housingfordisabledandvulnerablepeople => HousingType.HomesForDisabledAndVulnerablePeople,
            _ => HousingType.Undefined,
        };
    }
}
