using HE.Investment.AHP.Contract.Project;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Project.ValueObjects;

public class AhpProjectApplications : ValueObject
{
    public AhpProjectApplications(AhpProjectId id, AhpProjectName name, IList<AhpProjectApplication>? applications = null)
    {
        Id = id;
        Name = name;
        Applications.AddRange(applications);
    }

    public AhpProjectId Id { get; }

    public AhpProjectName Name { get; }

    public IList<AhpProjectApplication> Applications { get; } = [];

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Id;
        yield return Name;
        yield return Applications;
    }
}
