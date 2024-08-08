using HE.Investments.Common.Contract.Infrastructure.Events;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.AHP.ProjectDashboard.Contract.Project.Events;

public record AhpProjectHasBeenCreatedEvent(FrontDoorProjectId FrontDoorProjectId) : IDomainEvent;
