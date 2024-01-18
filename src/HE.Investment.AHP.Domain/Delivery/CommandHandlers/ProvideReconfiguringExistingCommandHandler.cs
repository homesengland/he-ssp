using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class ProvideReconfiguringExistingCommandHandler : UpdateDeliveryPhaseCommandHandler<ProvideReconfiguringExistingCommand>
{
    public ProvideReconfiguringExistingCommandHandler(IDeliveryPhaseRepository repository, IAccountUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override Task<OperationResult> Update(IDeliveryPhaseEntity entity, ProvideReconfiguringExistingCommand request, CancellationToken cancellationToken)
    {
        entity.ProvideReconfiguringExisting(request.ReconfiguringExisting);
        return Task.FromResult(OperationResult.Success());
    }
}
