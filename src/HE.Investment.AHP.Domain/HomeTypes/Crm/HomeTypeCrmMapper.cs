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
    private static readonly IList<string> BasicCrmFields = new[]
    {
        nameof(invln_HomeType.invln_typeofhousing),
        nameof(invln_HomeType.invln_hometypename),
        nameof(invln_HomeType.CreatedOn),
        nameof(invln_HomeType.invln_ishometypecompleted),
    };

    private readonly IList<IHomeTypeCrmSegmentMapper> _segmentMappers;

    public HomeTypeCrmMapper(IEnumerable<IHomeTypeCrmSegmentMapper> segmentMappers)
    {
        _segmentMappers = segmentMappers.ToList();
    }

    public IEnumerable<string> GetCrmFields(IEnumerable<HomeTypeSegmentType> segments)
    {
        return GetSegmentMappers(segments).SelectMany(x => x.CrmFieldNames).Concat(BasicCrmFields);
    }

    public IEnumerable<string> SaveCrmFields(HomeTypeEntity entity, IEnumerable<HomeTypeSegmentType> segments)
    {
        return GetSegmentMappers(segments.Where(entity.HasSegment)).SelectMany(x => x.CrmFieldNames).Concat(BasicCrmFields);
    }

    public HomeTypeEntity MapToDomain(
        ApplicationBasicInfo application,
        HomeTypeDto dto,
        IEnumerable<HomeTypeSegmentType> segments,
        IDictionary<HomeTypeSegmentType, IReadOnlyCollection<UploadedFile>> uploadedFiles)
    {
        var segmentEntities = GetSegmentMappers(segments)
            .Select(x => x.MapToEntity(
                application,
                dto,
                uploadedFiles.TryGetValue(x.SegmentType, out var file) ? file : Array.Empty<UploadedFile>()));

        return new HomeTypeEntity(
            application,
            dto.homeTypeName,
            MapHousingType(dto.housingType),
            dto.isCompleted == true ? SectionStatus.Completed : SectionStatus.InProgress,
            new HomeTypeId(dto.id),
            dto.createdOn,
            segments: segmentEntities.ToArray());
    }

    public HomeTypeDto MapToDto(HomeTypeEntity entity, IEnumerable<HomeTypeSegmentType> segments)
    {
        var homeTypeDto = new HomeTypeDto
        {
            id = entity.Id.IsNew ? null : entity.Id.Value,
            applicationId = entity.Application.Id.Value,
            homeTypeName = entity.Name.Value,
            housingType = MapHousingType(entity.HousingType),
            isCompleted = entity.Status == SectionStatus.Completed,
        };

        return GetSegmentMappers(segments).Aggregate(homeTypeDto, (dto, segmentMapper) => segmentMapper.MapToDto(dto, entity));
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

    private IEnumerable<IHomeTypeCrmSegmentMapper> GetSegmentMappers(IEnumerable<HomeTypeSegmentType> segments)
    {
        return segments.Select(x => _segmentMappers.Single(y => y.SegmentType == x));
    }
}
