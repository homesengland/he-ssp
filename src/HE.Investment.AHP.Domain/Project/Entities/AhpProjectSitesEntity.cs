using System.Collections.ObjectModel;
using HE.Investment.AHP.Contract.Project;
using HE.Investment.AHP.Domain.Project.ValueObjects;

namespace HE.Investment.AHP.Domain.Project.Entities;

public class AhpProjectSitesEntity
{
    public AhpProjectSitesEntity(AhpProjectId id, AhpProjectName name, IList<ProjectSite>? sites = null)
    {
        Id = id;
        Name = name;
        Sites = new ReadOnlyCollection<ProjectSite>(sites ?? []);
    }

    public AhpProjectId Id { get; }

    public AhpProjectName Name { get; }

    public IReadOnlyList<ProjectSite> Sites { get; }
}
