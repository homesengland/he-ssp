using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investment.AHP.Domain.Delivery.Crm;

public interface IDeliveryPhaseCrmContext
{
    Task<IList<DeliveryPhaseDto>> GetAllOrganisationDeliveryPhases(string applicationId, string organisationId, CancellationToken cancellationToken);

    Task<IList<DeliveryPhaseDto>> GetAllUserDeliveryPhases(string applicationId, string organisationId, CancellationToken cancellationToken);

    Task<DeliveryPhaseDto?> GetOrganisationDeliveryPhaseById(
        string applicationId,
        string deliveryPhaseId,
        string organisationId,
        CancellationToken cancellationToken);

    Task<DeliveryPhaseDto?> GetUserDeliveryPhaseById(
        string applicationId,
        string deliveryPhaseId,
        string organisationId,
        CancellationToken cancellationToken);

    Task Remove(string applicationId, string deliveryPhaseId, string organisationId, CancellationToken cancellationToken);

    Task<string> Save(DeliveryPhaseDto deliveryPhase, string organisationId, CancellationToken cancellationToken);
}
