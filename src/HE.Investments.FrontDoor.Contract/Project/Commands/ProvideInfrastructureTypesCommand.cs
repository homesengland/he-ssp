using HE.Investments.Common.Contract.Validators;
using HE.Investments.FrontDoor.Contract.Project.Enums;
using MediatR;

namespace HE.Investments.FrontDoor.Contract.Project.Commands;

public record ProvideInfrastructureTypesCommand(FrontDoorProjectId ProjectId, IList<InfrastructureType> InfrastructureTypes) : IProvideProjectDetailsCommand;
