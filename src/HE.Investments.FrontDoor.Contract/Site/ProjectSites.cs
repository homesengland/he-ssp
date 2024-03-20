using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.FrontDoor.Contract.Site;

public class ProjectSites
{
    public FrontDoorProjectId ProjectId { get; set; }

    public IList<SiteDetails> Sites { get; set; }
}
