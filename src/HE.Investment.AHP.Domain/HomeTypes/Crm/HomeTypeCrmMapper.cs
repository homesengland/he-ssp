using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.HomeTypes.Crm.Segments;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.HomeTypes.Crm;

public class HomeTypeCrmMapper : IHomeTypeCrmMapper
{
    private static readonly IList<string> BasicCrmFields = new[]
    {
        nameof(invln_HomeType.invln_typeofhousing),
        nameof(invln_HomeType.invln_hometypename),
        nameof(invln_HomeType.CreatedOn),
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

    public HomeTypeEntity MapToDomain(ApplicationBasicInfo application, HomeTypeDto dto, IEnumerable<HomeTypeSegmentType> segments)
    {
        var segmentEntities = GetSegmentMappers(segments).Select(x => x.MapToEntity(application, dto));

        return new HomeTypeEntity(
            application,
            dto.homeTypeName,
            MapHousingType(dto.housingType),
            new HomeTypeId(dto.id),
            dto.createdOn,
            segments: segmentEntities.ToArray());
    }

    public HomeTypeDto MapToDto(HomeTypeEntity entity, IEnumerable<HomeTypeSegmentType> segments)
    {
        var homeTypeDto = new HomeTypeDto
        {
            id = entity.Id?.Value,
            applicationId = entity.Application.Id.Value,
            homeTypeName = entity.Name.Value,
            housingType = MapHousingType(entity.HousingType),
        };

        return GetSegmentMappers(segments).Aggregate(homeTypeDto, (dto, segmentMapper) => segmentMapper.MapToDto(dto, entity));
    }

    private static int? MapHousingType(HousingType value)
    {
        return value switch
        {
            HousingType.General => (int)invln_typeofhousing.General,
            HousingType.HomesForOlderPeople => (int)invln_typeofhousing.Housingforolderpeople,
            HousingType.HomesForDisabledAndVulnerablePeople => (int)invln_typeofhousing.Housingfordisabledandvulnerablepeople,
            _ => null,
        };
    }

    private static HousingType MapHousingType(int? value)
    {
        return value switch
        {
            (int)invln_typeofhousing.General => HousingType.General,
            (int)invln_typeofhousing.Housingforolderpeople => HousingType.HomesForOlderPeople,
            (int)invln_typeofhousing.Housingfordisabledandvulnerablepeople => HousingType.HomesForDisabledAndVulnerablePeople,
            _ => HousingType.Undefined,
        };
    }

    private IEnumerable<IHomeTypeCrmSegmentMapper> GetSegmentMappers(IEnumerable<HomeTypeSegmentType> segments)
    {
        return segments.Select(x => _segmentMappers.Single(y => y.SegmentType == x));
    }
}
