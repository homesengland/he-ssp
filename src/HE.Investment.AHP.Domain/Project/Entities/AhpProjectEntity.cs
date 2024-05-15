using HE.Investment.AHP.Contract.Project;
using HE.Investment.AHP.Domain.Project.ValueObjects;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Project.Entities;

public class AhpProjectEntity
{
    public AhpProjectEntity(AhpProjectId id, AhpProjectName name, IList<AhpProjectApplication>? applications = null)
    {
        Id = id;
        Name = name;
        Applications.AddRange(applications);
    }

    public AhpProjectId Id { get; }

    public AhpProjectName Name { get; }

    public IList<AhpProjectApplication> Applications { get; } = new List<AhpProjectApplication>();
}
