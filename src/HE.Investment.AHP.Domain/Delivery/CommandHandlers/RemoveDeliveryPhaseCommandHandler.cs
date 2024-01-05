using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Validators;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class RemoveDeliveryPhaseCommandHandler : DeliveryCommandHandlerBase<RemoveDeliveryPhaseCommand>
{
    public RemoveDeliveryPhaseCommandHandler(
        IDeliveryPhaseRepository repository,
        IAccountUserContext accountUserContext,
        ILogger<RemoveDeliveryPhaseCommandHandler> logger)
        : base(repository, accountUserContext, logger)
    {
    }

    protected override IList<ErrorItem> Perform(DeliveryPhasesEntity deliveryPhases, RemoveDeliveryPhaseCommand request)
    {
        return PerformWithValidation(() => deliveryPhases.Remove(new DeliveryPhaseId(request.DeliveryPhaseId), request.RemoveDeliveryPhaseAnswer));
    }
}
