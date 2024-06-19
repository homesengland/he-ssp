using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Consortium.Shared.UserContext;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class ProvideBuildActivityCommandHandler : UpdateDeliveryPhaseCommandHandler<ProvideBuildActivityCommand>
{
    public ProvideBuildActivityCommandHandler(IDeliveryPhaseRepository repository, IConsortiumUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override Task<OperationResult> Update(IDeliveryPhaseEntity entity, ProvideBuildActivityCommand request, CancellationToken cancellationToken)
    {
        var buildActivityType = request.BuildActivityType.IsProvided() ?
            new BuildActivity(entity.Tenure, entity.TypeOfHomes.GetValueOrFirstValue(), request.BuildActivityType!.Value)
            : new BuildActivity(entity.Tenure);

        entity.ProvideBuildActivity(buildActivityType);

        return Task.FromResult(OperationResult.Success());
    }
}
