using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Consortium.Shared.UserContext;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class ProvideReconfiguringExistingCommandHandler : UpdateDeliveryPhaseCommandHandler<ProvideReconfiguringExistingCommand>
{
    public ProvideReconfiguringExistingCommandHandler(IDeliveryPhaseRepository repository, IConsortiumUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override Task<OperationResult> Update(IDeliveryPhaseEntity entity, ProvideReconfiguringExistingCommand request, CancellationToken cancellationToken)
    {
        entity.ProvideReconfiguringExisting(request.ReconfiguringExisting);
        return Task.FromResult(OperationResult.Success());
    }
}
