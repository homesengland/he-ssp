using System.Collections.Generic;
using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;


namespace HE.CRM.Plugins.Services.FrontDoorProjectSite
{
    public interface IFrontDoorProjectSiteService : ICrmService
    {
        PagedResponseDto<FrontDoorProjectSiteDto> GetFrontDoorProjectSites(PagingRequestDto pagingRequestDto, string frontDoorProjectId, bool useHeTables, string fieldsToRetrieve = null);
        FrontDoorProjectSiteDto GetFrontDoorProjectSite(string frontDoorProjectId, bool useHeTables, string fieldsToRetrieve = null, string frontDoorProjectSiteId = null);
        string CreateRecordFromPortal(string frontDoorProjectId, string entityFieldsParameters, bool useHeTables, string frontDoorSiteId = null);
        bool DeactivateFrontDoorSite(string frontDoorSiteId, bool useHeTables);
    }
}
