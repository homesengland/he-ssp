using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Queries;
using MediatR;

namespace HE.Investment.AHP.WWW.Utils;

public class CachedDeliveryPhaseProvider : IDeliveryPhaseProvider
{
    private readonly IMediator _mediator;

    private DeliveryPhaseDetails? _data;

    private GetDeliveryPhaseDetailsQuery? _cachedQueryRequest;

    public CachedDeliveryPhaseProvider(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<DeliveryPhaseDetails> Get(GetDeliveryPhaseDetailsQuery query, CancellationToken cancellationToken)
    {
        if (_cachedQueryRequest is null || _cachedQueryRequest != query)
        {
            _data = await _mediator.Send(query, cancellationToken);
            _cachedQueryRequest = query;
        }

        return _data!;
    }

    public void Invalidate()
    {
        _cachedQueryRequest = null;
        _data = null;
    }
}
