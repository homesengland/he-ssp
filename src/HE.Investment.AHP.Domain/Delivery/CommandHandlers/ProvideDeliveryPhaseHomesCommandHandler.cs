using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class ProvideDeliveryPhaseHomesCommandHandler : DeliveryCommandHandlerBase<ProvideDeliveryPhaseHomesCommand>
{
    public ProvideDeliveryPhaseHomesCommandHandler(
        IDeliveryPhaseRepository repository,
        IAccountUserContext accountUserContext,
        ILogger<ProvideDeliveryPhaseHomesCommandHandler> logger)
        : base(repository, accountUserContext, logger)
    {
    }

    protected override void Perform(DeliveryPhasesEntity deliveryPhases, ProvideDeliveryPhaseHomesCommand request)
    {
        var homesToDeliver = CreateHomesToDeliver(request.HomesToDeliver);
        deliveryPhases.ProvideHomesToBeDeliveredInPhase(request.DeliveryPhaseId, homesToDeliver);
    }

    private static IReadOnlyCollection<HomesToDeliverInPhase> CreateHomesToDeliver(IDictionary<string, string?> homesToDeliver)
    {
        var operationResult = new OperationResult();
        var result = homesToDeliver
            .Select(x => operationResult.Aggregate(() => HomesToDeliverInPhase.Create(HomeTypeId.From(x.Key), x.Value)))
            .ToList();
        operationResult.CheckErrors();

        return result;
    }
}
