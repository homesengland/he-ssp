using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Cache;

namespace HE.Investment.AHP.Domain.Delivery.Crm;

internal sealed class RequestCacheDeliverPhaseCrmContextDecorator : IDeliveryPhaseCrmContext
{
    private readonly InMemoryCache<DeliveryPhaseDto, string> _cache = new();

    private readonly IDeliveryPhaseCrmContext _decorated;

    public RequestCacheDeliverPhaseCrmContextDecorator(IDeliveryPhaseCrmContext decorated)
    {
        _decorated = decorated;
    }

    public async Task<IList<DeliveryPhaseDto>> GetAllOrganisationDeliveryPhases(string applicationId, string organisationId, CancellationToken cancellationToken)
    {
        return await _decorated.GetAllOrganisationDeliveryPhases(applicationId, organisationId, cancellationToken);
    }

    public async Task<IList<DeliveryPhaseDto>> GetAllUserDeliveryPhases(string applicationId, string organisationId, string userId, CancellationToken cancellationToken)
    {
        return await _decorated.GetAllUserDeliveryPhases(applicationId, organisationId, userId, cancellationToken);
    }

    public async Task<DeliveryPhaseDto?> GetOrganisationDeliveryPhaseById(string applicationId, string deliveryPhaseId, string organisationId, CancellationToken cancellationToken)
    {
        return await _cache.GetFromCache(
            deliveryPhaseId.ToGuidAsString(),
            async () => await _decorated.GetOrganisationDeliveryPhaseById(applicationId, deliveryPhaseId, organisationId, cancellationToken));
    }

    public async Task<DeliveryPhaseDto?> GetUserDeliveryPhaseById(string applicationId, string deliveryPhaseId, string organisationId, string userId, CancellationToken cancellationToken)
    {
        return await _cache.GetFromCache(
            deliveryPhaseId.ToGuidAsString(),
            async () => await _decorated.GetUserDeliveryPhaseById(applicationId, deliveryPhaseId, organisationId, userId, cancellationToken));
    }

    public async Task Remove(string applicationId, string deliveryPhaseId, string organisationId, string userId, CancellationToken cancellationToken)
    {
        await _decorated.Remove(applicationId, deliveryPhaseId, organisationId, userId, cancellationToken);
        _cache.Delete(deliveryPhaseId);
    }

    public async Task<string> Save(DeliveryPhaseDto deliveryPhase, string organisationId, string userId, CancellationToken cancellationToken)
    {
        deliveryPhase.id = await _decorated.Save(deliveryPhase, organisationId, userId, cancellationToken);
        _cache.ReplaceCache(deliveryPhase.id, deliveryPhase);

        return deliveryPhase.id;
    }
}
