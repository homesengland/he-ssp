using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.FrontDoor.Domain.Project.Api.Contract.Responses;

namespace HE.Investments.FrontDoor.Domain.Site.Api.Mappers;

internal static class GetSiteResponseMapper
{
    public static FrontDoorProjectSiteDto Map(GetSiteResponse response)
    {
        return new FrontDoorProjectSiteDto
        {
            SiteId = response.SiteId,
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
