using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Api.FrontDoor.Contract.Responses;

namespace HE.CRM.Common.Api.FrontDoor.Mappers
{
    public static class GetSiteResponseMapper
    {
        public static FrontDoorProjectSiteDto Map(GetSiteResponse response)
        {
            return new FrontDoorProjectSiteDto
            {
                SiteId = response.SiteId.ToString(),
                CreatedOn = response.CreatedOn.UtcDateTime,
                LocalAuthority = response.LocalAuthority ?? string.Empty,
                LocalAuthorityCode = response.LocalAuthorityCode ?? string.Empty,
                LocalAuthorityName = response.LocalAuthorityName ?? string.Empty,
                NumberofHomesEnabledBuilt = response.NumberOfHomesEnabledBuilt,
                PlanningStatus = response.PlanningStatus,
                SiteName = response.SiteName,
            };
        }
    }
}
