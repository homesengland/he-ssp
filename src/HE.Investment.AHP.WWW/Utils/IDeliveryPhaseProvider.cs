using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Queries;

namespace HE.Investment.AHP.WWW.Utils;

public interface IDeliveryPhaseProvider
{
    Task<DeliveryPhaseDetails> Get(GetDeliveryPhaseDetailsQuery query, CancellationToken cancellationToken);

    void Invalidate();
}
