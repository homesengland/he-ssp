using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
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
        return PerformWithValidation(() => deliveryPhases.Remove(request.DeliveryPhaseId, request.RemoveDeliveryPhaseAnswer));
    }
}
