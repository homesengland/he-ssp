using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class UnCompleteDeliveryPhaseCommandHandler : UpdateDeliveryPhaseCommandHandler<UnCompleteDeliveryPhaseCommand>
{
    public UnCompleteDeliveryPhaseCommandHandler(
        IDeliveryPhaseRepository repository,
        IAccountUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override Task<OperationResult> Update(IDeliveryPhaseEntity entity, UnCompleteDeliveryPhaseCommand request)
    {
        entity.UnComplete();

        return Task.FromResult(OperationResult.Success());
    }
}
