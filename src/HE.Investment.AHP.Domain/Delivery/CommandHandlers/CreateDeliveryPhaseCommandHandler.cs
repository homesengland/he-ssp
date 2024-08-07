using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Delivery.Policies;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Consortium.Shared.UserContext;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class CreateDeliveryPhaseCommandHandler : DeliveryCommandHandlerBase, IRequestHandler<CreateDeliveryPhaseCommand, OperationResult<DeliveryPhaseId?>>
{
    private readonly IDeliveryPhaseRepository _deliveryPhaseRepository;

    private readonly IConsortiumUserContext _accountUserContext;

    private readonly IOnlyCompletionMilestonePolicy _onlyCompletionMilestonePolicy;

    public CreateDeliveryPhaseCommandHandler(
        IDeliveryPhaseRepository repository,
        IConsortiumUserContext accountUserContext,
        ILogger<CreateDeliveryPhaseCommandHandler> logger,
        IOnlyCompletionMilestonePolicy onlyCompletionMilestonePolicy)
        : base(logger)
    {
        _accountUserContext = accountUserContext;
        _deliveryPhaseRepository = repository;
        _onlyCompletionMilestonePolicy = onlyCompletionMilestonePolicy;
    }

    public async Task<OperationResult<DeliveryPhaseId?>> Handle(CreateDeliveryPhaseCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var deliveryPhases = await _deliveryPhaseRepository.GetByApplicationId(request.ApplicationId, account, cancellationToken);

        try
        {
            var deliveryPhase = deliveryPhases.CreateDeliveryPhase(
                new DeliveryPhaseName(request.DeliveryPhaseName),
                account.SelectedOrganisation(),
                _onlyCompletionMilestonePolicy);
            var deliveryPhaseId = await _deliveryPhaseRepository.Save(deliveryPhase, account, cancellationToken);

            return new OperationResult<DeliveryPhaseId?>(deliveryPhaseId);
        }
        catch (DomainValidationException domainValidationException)
        {
            return new OperationResult<DeliveryPhaseId?>(domainValidationException.OperationResult.Errors, null);
        }
    }
}
