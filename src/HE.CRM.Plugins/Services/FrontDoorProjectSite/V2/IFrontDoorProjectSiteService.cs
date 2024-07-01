using System;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;


namespace HE.CRM.Plugins.Services.FrontDoorProjectSite.V2
{
    public interface IFrontDoorProjectSiteService : ICrmService
    {
        PagedResponseDto<FrontDoorProjectSiteDto> GetFrontDoorProjectSites(PagingRequestDto pagingRequestDto, Guid frontDoorProjectId, string fieldsToRetrieve = null);
        FrontDoorProjectSiteDto GetFrontDoorProjectSite(Guid frontDoorProjectId, Guid frontDoorProjectSiteId);
        string CreateRecordFromPortal(Guid frontDoorProjectId, string entityFieldsParameters, string frontDoorSiteId = null);
        bool DeactivateFrontDoorSite(Guid frontDoorSiteId);
    }
}
