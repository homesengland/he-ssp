using HE.Investments.Common.Contract.Infrastructure.Events;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.FrontDoor.Contract.Project.Events;

public record FrontDoorProjectHasBeenCreatedEvent(FrontDoorProjectId ProjectId, string ProjectName) : IDomainEvent;
