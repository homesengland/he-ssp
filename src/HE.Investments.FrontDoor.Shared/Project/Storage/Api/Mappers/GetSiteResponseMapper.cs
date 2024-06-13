using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.FrontDoor.Shared.Project.Storage.Api.Contract.Responses;

namespace HE.Investments.FrontDoor.Shared.Project.Storage.Api.Mappers;

public static class GetSiteResponseMapper
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
