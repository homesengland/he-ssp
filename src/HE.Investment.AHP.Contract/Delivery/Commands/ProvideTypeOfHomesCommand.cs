using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Delivery.Commands;

public record ProvideTypeOfHomesCommand(string ApplicationId, string DeliveryPhaseId, TypeOfHomes? TypeOfHomes) : IRequest<OperationResult>, IUpdateDeliveryPhaseCommand;
