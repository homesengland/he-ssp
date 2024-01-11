using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class ProvideAdditionalPaymentRequestCommandHandler : UpdateDeliveryPhaseCommandHandler<ProvideAdditionalPaymentRequestCommand>
{
    public ProvideAdditionalPaymentRequestCommandHandler(IDeliveryPhaseRepository repository, IAccountUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override Task<OperationResult> Update(IDeliveryPhaseEntity entity, ProvideAdditionalPaymentRequestCommand request)
    {
        entity.ProvideAdditionalPaymentRequest(request.IsAdditionalPaymentRequested.HasValue ? new IsAdditionalPaymentRequested(request.IsAdditionalPaymentRequested.Value) : null);

        return Task.FromResult(OperationResult.Success());
    }
}
