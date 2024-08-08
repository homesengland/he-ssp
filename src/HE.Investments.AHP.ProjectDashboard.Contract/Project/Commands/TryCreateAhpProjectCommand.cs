using HE.Investments.FrontDoor.Shared.Project;
using MediatR;

namespace HE.Investments.AHP.ProjectDashboard.Contract.Project.Commands;

public record TryCreateAhpProjectCommand(FrontDoorProjectId FrontDoorProjectId) : IRequest<AhpProjectId>;
