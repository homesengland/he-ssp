using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.HomeTypes.Entities;

namespace HE.Investment.AHP.Domain.HomeTypes.Crm.Segments;

public interface IHomeTypeCrmSegmentMapper
{
    IEnumerable<string> CrmFieldNames { get; }

    HomeTypeSegmentType SegmentType { get; }

    IHomeTypeSegmentEntity MapToEntity(ApplicationBasicInfo application, HomeTypeDto dto, IReadOnlyCollection<UploadedFile> uploadedFiles);

    HomeTypeDto MapToDto(HomeTypeDto dto, HomeTypeEntity entity);
}
