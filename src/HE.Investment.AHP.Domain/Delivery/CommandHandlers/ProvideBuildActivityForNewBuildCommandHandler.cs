using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class ProvideBuildActivityForNewBuildCommandHandler : UpdateDeliveryPhaseCommandHandler<ProvideBuildActivityForNewBuildCommand>
{
    public ProvideBuildActivityForNewBuildCommandHandler(IDeliveryPhaseRepository repository, IAccountUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override Task<OperationResult> Update(IDeliveryPhaseEntity entity, ProvideBuildActivityForNewBuildCommand request)
    {
        var buildActivityType = request.BuildActivityType.IsProvided() ?
            new BuildActivityType(entity.TypeOfHomes.GetValueOfFirstValue(), request.BuildActivityType!.Value)
            : new BuildActivityType();

        entity.ProvideBuildActivityType(buildActivityType);

        return Task.FromResult(OperationResult.Success());
    }
}
