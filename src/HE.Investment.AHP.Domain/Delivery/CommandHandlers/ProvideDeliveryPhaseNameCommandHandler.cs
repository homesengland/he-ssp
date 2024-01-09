using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class ProvideDeliveryPhaseNameCommandHandler : DeliveryCommandHandlerBase<ProvideDeliveryPhaseNameCommand>
{
    public ProvideDeliveryPhaseNameCommandHandler(
        IDeliveryPhaseRepository repository,
        IAccountUserContext accountUserContext,
        ILogger<ProvideDeliveryPhaseNameCommandHandler> logger)
        : base(repository, accountUserContext, logger)
    {
    }

    protected override IList<ErrorItem> Perform(DeliveryPhasesEntity deliveryPhases, ProvideDeliveryPhaseNameCommand request)
    {
        var deliveryPhase = deliveryPhases.GetById(new DeliveryPhaseId(request.DeliveryPhaseId));

        return PerformWithValidation(() => deliveryPhase.ProvideName(new DeliveryPhaseName(request.DeliveryPhaseName)));
    }
}
