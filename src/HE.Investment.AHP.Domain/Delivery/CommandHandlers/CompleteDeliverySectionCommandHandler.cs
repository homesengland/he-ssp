using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class CompleteDeliverySectionCommandHandler : DeliveryCommandHandlerBase, IRequestHandler<CompleteDeliverySectionCommand, OperationResult>
{
    private readonly IDeliveryPhaseRepository _repository;

    private readonly IAccountUserContext _accountUserContext;

    public CompleteDeliverySectionCommandHandler(
        IDeliveryPhaseRepository repository,
        IAccountUserContext accountUserContext,
        ILogger<CompleteDeliverySectionCommandHandler> logger)
        : base(logger)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult> Handle(CompleteDeliverySectionCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var deliveryPhases = await _repository.GetByApplicationId(request.ApplicationId, account, cancellationToken);
        var validationErrors = PerformWithValidation(() => deliveryPhases.CompleteSection(request.IsDeliveryCompleted));
        if (validationErrors.Any())
        {
            return new OperationResult(validationErrors);
        }

        if (!request.IsCheckOnly)
        {
            await _repository.Save(deliveryPhases, account.SelectedOrganisationId(), cancellationToken);
        }

        return OperationResult.Success();
    }
}
