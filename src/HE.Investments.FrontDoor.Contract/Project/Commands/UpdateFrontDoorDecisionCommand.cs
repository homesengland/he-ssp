using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using MediatR;

namespace HE.Investments.FrontDoor.Contract.Project.Commands;

public record UpdateFrontDoorDecisionCommand(FrontDoorProjectId ProjectId, ApplicationType ApplicationType) : IProvideProjectDetailsCommand;
