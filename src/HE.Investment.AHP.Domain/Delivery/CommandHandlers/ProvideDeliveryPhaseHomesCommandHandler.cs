using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Consortium.Shared.UserContext;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class ProvideDeliveryPhaseHomesCommandHandler : DeliveryCommandHandlerBase<ProvideDeliveryPhaseHomesCommand>
{
    public ProvideDeliveryPhaseHomesCommandHandler(
        IDeliveryPhaseRepository repository,
        IConsortiumUserContext accountUserContext,
        ILogger<ProvideDeliveryPhaseHomesCommandHandler> logger)
        : base(repository, accountUserContext, logger)
    {
    }

    protected override void Perform(DeliveryPhasesEntity deliveryPhases, ProvideDeliveryPhaseHomesCommand request)
    {
        var homesToDeliver = CreateHomesToDeliver(request.HomesToDeliver);
        deliveryPhases.ProvideHomesToBeDeliveredInPhase(request.DeliveryPhaseId, homesToDeliver);
    }

    [SuppressMessage("Performance", "CA1859", Justification = "Reviewed")]
    private static IReadOnlyCollection<HomesToDeliverInPhase> CreateHomesToDeliver(IDictionary<string, string?> homesToDeliver)
    {
        var operationResult = new OperationResult();
        var result = homesToDeliver
            .Select(x => operationResult.Aggregate(() => new HomesToDeliverInPhase(HomeTypeId.From(x.Key), x.Value)))
            .ToList();
        operationResult.CheckErrors();

        return result;
    }
}
