using HE.Investments.Common.Contract;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Delivery.Commands;

public record ProvideStartOnSiteMilestoneDetailsCommand(string ApplicationId, string DeliveryPhaseId, DateDetails StartOnSiteDate, DateDetails PaymentDate) : IRequest<OperationResult>, IUpdateDeliveryPhaseCommand;
