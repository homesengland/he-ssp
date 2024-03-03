using System.Collections.Generic;
using DataverseModel;
using HE.Base.Services;
using HE.Common.IntegrationModel.PortalIntegrationModel;


namespace HE.CRM.Plugins.Services.FrontDoorProjectSite
{
    public interface IFrontDoorProjectSiteService : ICrmService
    {
        FrontDoorProjectSiteDto GetSingleFrontDoorProjectSite(string frontDoorSiteId);
        List<FrontDoorProjectSiteDto> GetMultipleSiteRelatedToFrontDoorProjectForGivenAccountAndContact(string frontDoorProjectId, string organisationId, string externalContactId);
    }
}
