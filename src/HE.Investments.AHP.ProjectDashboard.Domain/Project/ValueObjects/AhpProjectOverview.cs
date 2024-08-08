using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.AHP.ProjectDashboard.Domain.Project.ValueObjects;

public class AhpProjectOverview : ValueObject
{
    public AhpProjectOverview(
        FrontDoorProjectId id,
        AhpProjectName name,
        IList<AhpProjectApplication>? applications = null,
        IList<AhpProjectAllocation>? allocations = null)
    {
        Id = id;
        Name = name;
        Applications.AddRange(applications);
        Allocations.AddRange(allocations);
    }

    public FrontDoorProjectId Id { get; }

    public AhpProjectName Name { get; }

    public IList<AhpProjectApplication> Applications { get; } = [];

    public IList<AhpProjectAllocation> Allocations { get; } = [];

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Id;
        yield return Name;
        yield return Applications;
    }
}
