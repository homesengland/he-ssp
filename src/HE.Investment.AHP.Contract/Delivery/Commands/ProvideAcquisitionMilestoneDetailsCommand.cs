using HE.Investments.Common.Contract;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Delivery.Commands;

public record ProvideAcquisitionMilestoneDetailsCommand(string ApplicationId, string DeliveryPhaseId, DateDetails AcquisitionDate, DateDetails PaymentDate) : IRequest<OperationResult>, IUpdateDeliveryPhaseCommand;
