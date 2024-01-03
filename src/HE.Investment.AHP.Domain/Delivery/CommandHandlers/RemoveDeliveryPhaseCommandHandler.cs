using HE.Investment.AHP.Domain.Delivery.Commands;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class RemoveDeliveryPhaseCommandHandler : DeliveryCommandHandlerBase, IRequestHandler<RemoveDeliveryPhaseCommand, OperationResult>
{
    private readonly IDeliveryPhaseRepository _repository;

    private readonly IAccountUserContext _accountUserContext;

    public RemoveDeliveryPhaseCommandHandler(
        IDeliveryPhaseRepository repository,
        IAccountUserContext accountUserContext,
        ILogger<RemoveDeliveryPhaseCommandHandler> logger)
        : base(logger)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult> Handle(RemoveDeliveryPhaseCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var deliveryPhases = await _repository.GetByApplicationId(new ApplicationId(request.ApplicationId), account, cancellationToken);
        var validationErrors = PerformWithValidation(
            () => deliveryPhases.Remove(new DeliveryPhaseId(request.DeliveryPhaseId), request.RemoveDeliveryPhaseAnswer));
        if (validationErrors.Any())
        {
            return new OperationResult(validationErrors);
        }

        await _repository.Save(deliveryPhases, account.SelectedOrganisationId(), cancellationToken);
        return OperationResult.Success();
    }
}
