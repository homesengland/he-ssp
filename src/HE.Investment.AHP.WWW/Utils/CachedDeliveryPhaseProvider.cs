using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Queries;
using MediatR;
using NuGet.Common;

namespace HE.Investment.AHP.WWW.Utils;

public class CachedDeliveryPhaseProvider : IDeliveryPhaseProvider
{
    private readonly IMediator _mediator;
    private AsyncLazy<DeliveryPhaseDetails>? _data;

    public CachedDeliveryPhaseProvider(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<DeliveryPhaseDetails> Get(GetDeliveryPhaseDetailsQuery query, CancellationToken cancellationToken)
    {
        _data ??= new AsyncLazy<DeliveryPhaseDetails>(async () => await _mediator.Send(query, cancellationToken));

        return await _data;
    }
}
