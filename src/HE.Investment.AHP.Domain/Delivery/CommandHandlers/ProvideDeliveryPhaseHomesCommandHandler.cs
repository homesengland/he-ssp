using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class ProvideDeliveryPhaseHomesCommandHandler : DeliveryCommandHandlerBase<ProvideDeliveryPhaseHomesCommand>, IRequestHandler<ProvideDeliveryPhaseHomesCommand, OperationResult>
{
    public ProvideDeliveryPhaseHomesCommandHandler(
        IDeliveryPhaseRepository repository,
        IAccountUserContext accountUserContext,
        ILogger<ProvideDeliveryPhaseHomesCommandHandler> logger)
        : base(repository, accountUserContext, logger)
    {
    }

    protected override IList<ErrorItem> Perform(DeliveryPhasesEntity deliveryPhases, ProvideDeliveryPhaseHomesCommand request)
    {
        // AB#66085 save Delivery Phase homes.
        return Array.Empty<ErrorItem>();
    }
}
