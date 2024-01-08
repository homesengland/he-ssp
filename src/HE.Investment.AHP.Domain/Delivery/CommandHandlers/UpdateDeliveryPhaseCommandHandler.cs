using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Validators;
using MediatR;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public abstract class UpdateDeliveryPhaseCommandHandler<TCommand> : IRequestHandler<TCommand, OperationResult>
    where TCommand : IUpdateDeliveryPhaseCommand, IRequest<OperationResult>
{
    private readonly IDeliveryPhaseRepository _repository;
    private readonly IAccountUserContext _accountUserContext;

    protected UpdateDeliveryPhaseCommandHandler(IDeliveryPhaseRepository repository, IAccountUserContext accountUserContext)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult> Handle(TCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var deliveryPhase = await _repository.GetById(new ApplicationId(request.ApplicationId), new DeliveryPhaseId(request.DeliveryPhaseId), account, cancellationToken);

        var result = await Update(deliveryPhase, request);

        if (result.IsValid)
        {
            await _repository.Save(deliveryPhase, account, cancellationToken);
        }

        return result;
    }

    protected abstract Task<OperationResult> Update(IDeliveryPhaseEntity entity, TCommand request);
}
