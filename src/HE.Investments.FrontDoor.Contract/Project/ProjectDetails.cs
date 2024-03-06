using HE.Investments.FrontDoor.Contract.Project.Enums;
using HE.Investments.FrontDoor.Contract.Site;

namespace HE.Investments.FrontDoor.Contract.Project;

public class ProjectDetails
{
    public FrontDoorProjectId Id { get; set; }

    public string Name { get; set; }

    public bool IsEnglandHousingDelivery { get; set; }

    public IList<SupportActivityType>? SupportActivityTypes { get; set; }

    public string? OrganisationHomesBuilt { get; set; }

    public bool? IsSiteIdentified { get; set; }

    public FrontDoorSiteId? LastSiteId { get; set; }

    public ProjectGeographicFocus GeographicFocus { get; set; }

    public bool? IsFundingRequired { get; set; }

    public AffordableHomesAmount AffordableHomesAmount { get; set; }
}
