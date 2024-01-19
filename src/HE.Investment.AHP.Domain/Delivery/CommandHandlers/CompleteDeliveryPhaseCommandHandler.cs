using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class CompleteDeliveryPhaseCommandHandler : UpdateDeliveryPhaseCommandHandler<CompleteDeliveryPhaseCommand>
{
    public CompleteDeliveryPhaseCommandHandler(
        IDeliveryPhaseRepository repository,
        IAccountUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override Task<OperationResult> Update(IDeliveryPhaseEntity entity, CompleteDeliveryPhaseCommand request, CancellationToken cancellationToken)
    {
        entity.Complete();

        return Task.FromResult(OperationResult.Success());
    }
}
