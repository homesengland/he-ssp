using System.Collections.ObjectModel;
using HE.Investment.AHP.Contract.Project;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Project.ValueObjects;

public class AhpProjectSites : ValueObject
{
    public AhpProjectSites(AhpProjectId id, AhpProjectName name, IList<AhpProjectSite>? sites = null)
    {
        Id = id;
        Name = name;
        Sites = new ReadOnlyCollection<AhpProjectSite>(sites ?? []);
    }

    public AhpProjectId Id { get; }

    public AhpProjectName Name { get; }

    public IReadOnlyList<AhpProjectSite> Sites { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Id;
        yield return Name;
        yield return Sites;
    }
}
