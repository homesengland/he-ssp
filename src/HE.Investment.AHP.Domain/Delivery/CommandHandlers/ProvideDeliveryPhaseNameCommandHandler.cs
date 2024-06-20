using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Consortium.Shared.UserContext;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class ProvideDeliveryPhaseNameCommandHandler : DeliveryCommandHandlerBase<ProvideDeliveryPhaseNameCommand>
{
    public ProvideDeliveryPhaseNameCommandHandler(
        IDeliveryPhaseRepository repository,
        IConsortiumUserContext accountUserContext,
        ILogger<ProvideDeliveryPhaseNameCommandHandler> logger)
        : base(repository, accountUserContext, logger)
    {
    }

    protected override void Perform(DeliveryPhasesEntity deliveryPhases, ProvideDeliveryPhaseNameCommand request)
    {
        deliveryPhases.ProvideDeliveryPhaseName(request.DeliveryPhaseId, new DeliveryPhaseName(request.DeliveryPhaseName));
    }
}
