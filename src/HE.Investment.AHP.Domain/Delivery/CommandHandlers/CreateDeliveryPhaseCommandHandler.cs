using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class CreateDeliveryPhaseCommandHandler : DeliveryCommandHandlerBase, IRequestHandler<CreateDeliveryPhaseCommand, OperationResult<DeliveryPhaseId?>>
{
    private readonly IDeliveryPhaseRepository _deliveryPhaseRepository;

    private readonly IAccountUserContext _accountUserContext;

    public CreateDeliveryPhaseCommandHandler(
        IDeliveryPhaseRepository repository,
        IAccountUserContext accountUserContext,
        ILogger<CreateDeliveryPhaseCommandHandler> logger)
        : base(logger)
    {
        _accountUserContext = accountUserContext;
        _deliveryPhaseRepository = repository;
    }

    public async Task<OperationResult<DeliveryPhaseId?>> Handle(CreateDeliveryPhaseCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var deliveryPhases = await _deliveryPhaseRepository.GetByApplicationId(request.ApplicationId, account, cancellationToken);

        try
        {
            var deliveryPhase = deliveryPhases.CreateDeliveryPhase(new DeliveryPhaseName(request.DeliveryPhaseName), account.SelectedOrganisation());
            var deliveryPhaseId = await _deliveryPhaseRepository.Save(deliveryPhase, account.SelectedOrganisationId(), cancellationToken);

            return new OperationResult<DeliveryPhaseId?>(deliveryPhaseId);
        }
        catch (DomainValidationException domainValidationException)
        {
            return new OperationResult<DeliveryPhaseId?>(domainValidationException.OperationResult.Errors, null);
        }
    }
}
