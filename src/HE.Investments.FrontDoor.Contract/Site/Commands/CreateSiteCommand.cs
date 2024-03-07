using HE.Investments.Common.Contract.Validators;
using HE.Investments.FrontDoor.Contract.Project;
using MediatR;

namespace HE.Investments.FrontDoor.Contract.Site.Commands;

public record CreateSiteCommand(FrontDoorProjectId ProjectId, string? Name) : IRequest<OperationResult<FrontDoorSiteId>>;
