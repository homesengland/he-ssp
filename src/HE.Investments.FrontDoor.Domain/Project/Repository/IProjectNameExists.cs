using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;

namespace HE.Investments.FrontDoor.Domain.Project.Repository;

public interface IProjectNameExists
{
    Task<bool> DoesExist(ProjectName name, FrontDoorProjectId? exceptProjectId, CancellationToken cancellationToken);
}
