using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.FrontDoor.Contract.Project.Commands;

public record CreateProjectCommand(string? Name) : IRequest<OperationResult<FrontDoorProjectId>>;
