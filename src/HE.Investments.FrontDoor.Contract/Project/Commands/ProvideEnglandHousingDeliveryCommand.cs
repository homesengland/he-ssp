using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.FrontDoor.Contract.Project.Commands;

public record ProvideEnglandHousingDeliveryCommand(FrontDoorProjectId? ProjectId, bool? IsEnglandHousingDelivery) : IRequest<OperationResult>;
