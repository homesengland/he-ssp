using HE.Investments.Common.Contract;
using HE.Investments.FrontDoor.Contract.Site;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Contract;

namespace HE.Investments.FrontDoor.Contract.Project;

public class ProjectDetails
{
    public FrontDoorProjectId Id { get; set; }

    public string Name { get; set; }

    public bool IsEnglandHousingDelivery { get; set; }

    public IList<SupportActivityType>? SupportActivityTypes { get; set; }

    public string? OrganisationHomesBuilt { get; set; }

    public IList<InfrastructureType>? InfrastructureTypes { get; set; }

    public bool? IsSiteIdentified { get; set; }

    public FrontDoorSiteId? LastSiteId { get; set; }

    public ProjectGeographicFocus GeographicFocus { get; set; }

    public bool? IsFundingRequired { get; set; }

    public AffordableHomesAmount AffordableHomesAmount { get; set; }

    public IList<RegionType>? Regions { get; set; }

    public string? HomesNumber { get; set; }

    public RequiredFundingOption? RequiredFunding { get; set; }

    public bool? IsSupportRequired { get; set; }

    public bool? IsProfit { get; set; }

    public DateDetails? ExpectedStartDate { get; set; }
}
