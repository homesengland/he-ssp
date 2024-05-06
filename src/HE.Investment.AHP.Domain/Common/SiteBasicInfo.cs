using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investment.AHP.Domain.Common;

public record SiteBasicInfo(
    SiteId Id,
    SiteName Name,
    FrontDoorProjectId? FrontDoorProjectId,
    FrontDoorSiteId? FrontDoorSiteId,
    LandAcquisitionStatus LandAcquisitionStatus,
    SiteUsingModernMethodsOfConstruction SiteUsingModernMethodsOfConstruction);
