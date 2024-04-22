using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.HomeTypes.Entities;

namespace HE.Investment.AHP.Domain.HomeTypes.Crm;

public interface IHomeTypeCrmMapper
{
    HomeTypeEntity MapToDomain(
        ApplicationBasicInfo application,
        SiteBasicInfo site,
        HomeTypeDto dto,
        IDictionary<HomeTypeSegmentType, IReadOnlyCollection<UploadedFile>> uploadedFiles);

    HomeTypeDto MapToDto(HomeTypeEntity entity);
}
