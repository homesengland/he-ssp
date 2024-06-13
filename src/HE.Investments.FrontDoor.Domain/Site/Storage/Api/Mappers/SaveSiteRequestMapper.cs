using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Extensions;
using HE.Investments.FrontDoor.Domain.Site.Storage.Api.Contract.Requests;

namespace HE.Investments.FrontDoor.Domain.Site.Storage.Api.Mappers;

internal static class SaveSiteRequestMapper
{
    public static SaveSiteRequest Map(FrontDoorProjectSiteDto site, string projectId)
    {
        return new SaveSiteRequest
        {
            ProjectRecordId = projectId.ToGuidAsString(),
            SiteId = site.SiteId,
            SiteName = site.SiteName,
            PlanningStatus = site.PlanningStatus,
            LocalAuthorityCode = site.LocalAuthorityCode ?? "6000002", // TODO: AB#98936 remove hardcoded value when API will support it
            NumberOfHomesEnabledBuilt = site.NumberofHomesEnabledBuilt,
        };
    }
}
