using HE.Investments.Common.Contract.Validators;
using HE.Investments.FrontDoor.Shared.Project;
using HE.UtilsService.BannerNotification.Shared;
using MediatR;

namespace HE.Investments.FrontDoor.Contract.Project.Queries;

public record DetermineProjectDecisionCommand(FrontDoorProjectId ProjectId) : IRequest<(OperationResult OperationResult, ApplicationType ApplicationType)>;
