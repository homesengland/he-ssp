using HE.Investments.Common.Contract.Enum;

namespace HE.Investments.FrontDoor.Contract.Site;

public class SiteDetails
{
    public FrontDoorSiteId Id { get; set; }

    public string Name { get; set; }

    public string ProjectName { get; set; }

    public string? HomesNumber { get; set; }

    public SitePlanningStatus? PlanningStatus { get; set; }

    public AddAnotherSite? AddAnotherSite { get; set; }
}
