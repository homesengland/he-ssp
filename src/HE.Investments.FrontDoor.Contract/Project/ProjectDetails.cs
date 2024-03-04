namespace HE.Investments.FrontDoor.Contract.Project;

public class ProjectDetails
{
    public FrontDoorProjectId Id { get; set; }

    public string Name { get; set; }

    public bool IsEnglandHousingDelivery { get; set; }

    public IList<ActivityType>? ActivityTypes { get; set; }
}
