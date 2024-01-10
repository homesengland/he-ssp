using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class ProvideBuildActivityForNewBuildCommandHandler : UpdateDeliveryPhaseCommandHandler<ProvideBuildActivityForNewBuildCommand>
{
    public ProvideBuildActivityForNewBuildCommandHandler(IDeliveryPhaseRepository repository, IAccountUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override Task<OperationResult> Update(IDeliveryPhaseEntity entity, ProvideBuildActivityForNewBuildCommand request)
    {
        if (request.BuildActivityType.IsProvided())
        {
            entity.ProvideBuildActivityType(request.BuildActivityType!.Value);
        }

        return Task.FromResult(OperationResult.Success());
    }
}
