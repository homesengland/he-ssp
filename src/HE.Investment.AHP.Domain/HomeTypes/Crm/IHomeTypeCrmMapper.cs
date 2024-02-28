using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.HomeTypes.Entities;

namespace HE.Investment.AHP.Domain.HomeTypes.Crm;

public interface IHomeTypeCrmMapper
{
    IEnumerable<string> GetCrmFields(IEnumerable<HomeTypeSegmentType> segments);

    IEnumerable<string> SaveCrmFields(HomeTypeEntity entity, IEnumerable<HomeTypeSegmentType> segments);

    HomeTypeEntity MapToDomain(
        ApplicationBasicInfo application,
        SiteBasicInfo site,
        HomeTypeDto dto,
        IEnumerable<HomeTypeSegmentType> segments,
        IDictionary<HomeTypeSegmentType, IReadOnlyCollection<UploadedFile>> uploadedFiles);

    HomeTypeDto MapToDto(HomeTypeEntity entity, IEnumerable<HomeTypeSegmentType> segments);
}
