using HE.Investment.AHP.Contract.HomeTypes.Commands;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class RemoveHomeTypeCommandHandler : HomeTypeCommandHandlerBase, IRequestHandler<RemoveHomeTypeCommand, OperationResult>
{
    private readonly IHomeTypeRepository _repository;

    private readonly IDeliveryPhaseRepository _deliveryRepository;

    private readonly IAccountUserContext _accountUserContext;

    public RemoveHomeTypeCommandHandler(
        IHomeTypeRepository repository,
        IDeliveryPhaseRepository deliveryRepository,
        IAccountUserContext accountUserContext,
        ILogger<RemoveHomeTypeCommandHandler> logger)
        : base(logger)
    {
        _repository = repository;
        _deliveryRepository = deliveryRepository;
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult> Handle(RemoveHomeTypeCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var homeTypes = await _repository.GetByApplicationId(request.ApplicationId, account, cancellationToken);
        var deliveryPhases = await _deliveryRepository.GetByApplicationId(request.ApplicationId, account, cancellationToken);
        var validationErrors = PerformWithValidation(() => homeTypes.Remove(request.HomeTypeId, request.RemoveHomeTypeAnswer, deliveryPhases));
        if (validationErrors.Any())
        {
            return new OperationResult(validationErrors);
        }

        await _repository.Save(homeTypes, account.SelectedOrganisationId(), cancellationToken);
        return OperationResult.Success();
    }
}
