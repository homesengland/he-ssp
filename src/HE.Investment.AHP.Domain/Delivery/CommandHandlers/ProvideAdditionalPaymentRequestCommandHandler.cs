using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Repositories;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Consortium.Shared.UserContext;

namespace HE.Investment.AHP.Domain.Delivery.CommandHandlers;

public class ProvideAdditionalPaymentRequestCommandHandler : UpdateDeliveryPhaseCommandHandler<ProvideAdditionalPaymentRequestCommand>
{
    public ProvideAdditionalPaymentRequestCommandHandler(IDeliveryPhaseRepository repository, IConsortiumUserContext accountUserContext)
        : base(repository, accountUserContext)
    {
    }

    protected override Task<OperationResult> Update(IDeliveryPhaseEntity entity, ProvideAdditionalPaymentRequestCommand request, CancellationToken cancellationToken)
    {
        entity.ProvideAdditionalPaymentRequest(request.IsAdditionalPaymentRequested.HasValue ? new IsAdditionalPaymentRequested(request.IsAdditionalPaymentRequested.Value) : null);

        return Task.FromResult(OperationResult.Success());
    }
}
