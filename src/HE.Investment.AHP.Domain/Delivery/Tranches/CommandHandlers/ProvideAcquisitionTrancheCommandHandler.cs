using HE.Investment.AHP.Contract.Delivery.MilestonePayments.Commands;
using HE.Investment.AHP.Domain.Delivery.CommandHandlers;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Delivery.Tranches.CommandHandlers;

public class ProvideAcquisitionTrancheCommandHandler : UpdateDeliveryPhaseCommandHandler<ProvideAcquisitionTrancheCommand>
{
    public ProvideAcquisitionTrancheCommandHandler(IDeliveryPhaseRepository repository, IAccountUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override Task<OperationResult> Update(IDeliveryPhaseEntity entity, ProvideAcquisitionTrancheCommand request, CancellationToken cancellationToken)
    {
        var deliveryPhaseTranches = entity.Tranches;
        deliveryPhaseTranches.ProvideAcquisitionTranche(request.AcquisitionTranche);
        return Task.FromResult(OperationResult.Success());
    }
}
