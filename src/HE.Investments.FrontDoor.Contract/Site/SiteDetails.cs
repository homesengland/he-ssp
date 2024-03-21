using HE.Investments.Common.Contract.Enum;
using HE.Investments.FrontDoor.Contract.Site.Enums;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.FrontDoor.Contract.Site;

public class SiteDetails
{
    public FrontDoorSiteId Id { get; set; }

    public string Name { get; set; }

    public string ProjectName { get; set; }

    public string? HomesNumber { get; set; }

    public string? LocalAuthorityCode { get; set; }

    public SitePlanningStatus? PlanningStatus { get; set; }

    public AddAnotherSite? AddAnotherSite { get; set; }

    public RemoveSiteAnswer? RemoveSiteAnswer { get; set; }
}
