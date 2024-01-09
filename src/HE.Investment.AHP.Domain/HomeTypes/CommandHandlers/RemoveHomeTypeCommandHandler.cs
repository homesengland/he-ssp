using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

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
        var homeTypes = await _repository.GetByApplicationId(new ApplicationId(request.ApplicationId), account, HomeTypeSegmentTypes.None, cancellationToken);
        var deliveryPhases = await _deliveryRepository.GetByApplicationId(new ApplicationId(request.ApplicationId), account, cancellationToken);
        var validationErrors = PerformWithValidation(() => homeTypes.Remove(new HomeTypeId(request.HomeTypeId), request.RemoveHomeTypeAnswer, deliveryPhases));
        if (validationErrors.Any())
        {
            return new OperationResult(validationErrors);
        }

        await _repository.Save(homeTypes, account.SelectedOrganisationId(), cancellationToken);
        return OperationResult.Success();
    }
}
