using HE.Investments.FrontDoor.Shared.Project;
using MediatR;

namespace HE.Investment.AHP.Contract.Project.Commands;

public record CreateAhpProjectIfDoesNotExistCommand(FrontDoorProjectId FrontDoorProjectId) : IRequest<AhpProjectId>;
