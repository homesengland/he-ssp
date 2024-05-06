using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.HomeTypes.Entities;

namespace HE.Investment.AHP.Domain.HomeTypes.Crm.Segments;

public abstract class HomeTypeCrmSegmentMapperBase<TSegment> : IHomeTypeCrmSegmentMapper
    where TSegment : IHomeTypeSegmentEntity
{
    public abstract HomeTypeSegmentType SegmentType { get; }

    public abstract IHomeTypeSegmentEntity MapToEntity(
        ApplicationBasicInfo application,
        SiteBasicInfo site,
        HomeTypeDto dto,
        IReadOnlyCollection<UploadedFile> uploadedFiles);

    public HomeTypeDto MapToDto(HomeTypeDto dto, HomeTypeEntity entity)
    {
        MapToDto(dto, GetSegment(entity));

        return dto;
    }

    protected abstract TSegment GetSegment(HomeTypeEntity entity);

    protected abstract void MapToDto(HomeTypeDto dto, TSegment segment);
}
