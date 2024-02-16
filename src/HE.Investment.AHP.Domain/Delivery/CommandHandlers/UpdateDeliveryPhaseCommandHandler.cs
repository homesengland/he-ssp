using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public abstract class UpdateDeliveryPhaseCommandHandler<TCommand> : IRequestHandler<TCommand, OperationResult>
    where TCommand : IUpdateDeliveryPhaseCommand, IRequest<OperationResult>
{
    private readonly IDeliveryPhaseRepository _repository;

    protected UpdateDeliveryPhaseCommandHandler(IDeliveryPhaseRepository repository, IAccountUserContext accountUserContext)
    {
        _repository = repository;
        AccountUserContext = accountUserContext;
    }

    protected IAccountUserContext AccountUserContext { get; }

    public async Task<OperationResult> Handle(TCommand request, CancellationToken cancellationToken)
    {
        var account = await AccountUserContext.GetSelectedAccount();
        var deliveryPhase = await _repository.GetById(request.ApplicationId, request.DeliveryPhaseId, account, cancellationToken);

        var result = await Update(deliveryPhase, request, cancellationToken);

        if (result.IsValid)
        {
            await _repository.Save(deliveryPhase, account.SelectedOrganisationId(), cancellationToken);
        }

        return result;
    }

    protected abstract Task<OperationResult> Update(IDeliveryPhaseEntity entity, TCommand request, CancellationToken cancellationToken);
}
