using HE.Investments.Common.Domain;

namespace HE.Investments.FrontDoor.Domain.Project;

public class ProjectEntity : DomainEntity
{
    public string Id { get; set; }

    public string Name { get; private set; }

    public ProjectEntity(string id, string name)
    {
        Id = id;
        Name = name;
    }
}
