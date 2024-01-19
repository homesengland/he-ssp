using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investments.Account.Shared;
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

    protected override void Perform(DeliveryPhasesEntity deliveryPhases, RemoveDeliveryPhaseCommand request)
    {
        deliveryPhases.Remove(request.DeliveryPhaseId, request.RemoveDeliveryPhaseAnswer);
    }
}
