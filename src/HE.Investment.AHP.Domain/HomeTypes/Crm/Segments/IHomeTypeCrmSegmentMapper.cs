using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.HomeTypes.Entities;

namespace HE.Investment.AHP.Domain.HomeTypes.Crm.Segments;

public interface IHomeTypeCrmSegmentMapper
{
    HomeTypeSegmentType SegmentType { get; }

    IHomeTypeSegmentEntity MapToEntity(ApplicationBasicInfo application, SiteBasicInfo site, HomeTypeDto dto, IReadOnlyCollection<UploadedFile> uploadedFiles);

    HomeTypeDto MapToDto(HomeTypeDto dto, HomeTypeEntity entity);
}
