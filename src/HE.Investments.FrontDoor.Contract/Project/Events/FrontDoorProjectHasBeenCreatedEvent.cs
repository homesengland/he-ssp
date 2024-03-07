using HE.Investments.Common.Contract.Infrastructure.Events;

namespace HE.Investments.FrontDoor.Contract.Project.Events;

public record FrontDoorProjectHasBeenCreatedEvent(FrontDoorProjectId ProjectId, string ProjectName) : IDomainEvent;
