using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class ProvideAcquisitionMilestoneDetailsCommandHandler : UpdateDeliveryPhaseCommandHandler<ProvideAcquisitionMilestoneDetailsCommand>
{
    public ProvideAcquisitionMilestoneDetailsCommandHandler(IDeliveryPhaseRepository repository, IAccountUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override Task<OperationResult> Update(IDeliveryPhaseEntity entity, ProvideAcquisitionMilestoneDetailsCommand request)
    {
        var operationResult = OperationResult.New();

        var acquisitionDate = operationResult.AggregateNullable(() =>
            AcquisitionDate.Create(request.AcquisitionDate.Day, request.AcquisitionDate.Month, request.AcquisitionDate.Year));
        var milestonePaymentDate = operationResult.AggregateNullable(() =>
            MilestonePaymentDate.Create(request.PaymentDate.Day, request.PaymentDate.Month, request.PaymentDate.Year));
        var details = operationResult.AggregateNullable(() => AcquisitionMilestoneDetails.Create(acquisitionDate, milestonePaymentDate));

        entity.ProvideAcquisitionMilestoneDetails(details);

        return Task.FromResult(operationResult);
    }
}
