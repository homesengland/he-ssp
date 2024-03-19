using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Contract;

namespace HE.Investments.Loans.IntegrationTests.Config;

public class FrontDoorProjectData
{
    public FrontDoorProjectData(string name, IReadOnlyCollection<SupportActivityType> supportActivities)
    {
        Name = name;
        SupportActivities = supportActivities;
    }

    public FrontDoorProjectId Id { get; private set; }

    public string Name { get; private set; }

    public bool IsEnglandHousingDelivery => true;

    public IReadOnlyCollection<SupportActivityType> SupportActivities { get; private set; }

    public void SetProjectId(FrontDoorProjectId projectId)
    {
        Id = projectId;
    }
}
