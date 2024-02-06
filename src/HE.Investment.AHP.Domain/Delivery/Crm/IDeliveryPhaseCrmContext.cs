using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investment.AHP.Domain.Delivery.Crm;

public interface IDeliveryPhaseCrmContext
{
    Task<int?> GetDeliveryStatus(string applicationId, Guid organisationId, CancellationToken cancellationToken);

    Task SaveDeliveryStatus(string applicationId, Guid organisationId, int deliveryStatus, CancellationToken cancellationToken);

    Task<IList<DeliveryPhaseDto>> GetAllOrganisationDeliveryPhases(
        string applicationId,
        Guid organisationId,
        IEnumerable<string> fieldsToRetrieve,
        CancellationToken cancellationToken);

    Task<IList<DeliveryPhaseDto>> GetAllUserDeliveryPhases(
        string applicationId,
        Guid organisationId,
        IEnumerable<string> fieldsToRetrieve,
        CancellationToken cancellationToken);

    Task<DeliveryPhaseDto?> GetOrganisationDeliveryPhaseById(
        string applicationId,
        string deliveryPhaseId,
        Guid organisationId,
        IEnumerable<string> fieldsToRetrieve,
        CancellationToken cancellationToken);

    Task<DeliveryPhaseDto?> GetUserDeliveryPhaseById(
        string applicationId,
        string deliveryPhaseId,
        Guid organisationId,
        IEnumerable<string> fieldsToRetrieve,
        CancellationToken cancellationToken);

    Task Remove(string applicationId, string deliveryPhaseId, Guid organisationId, CancellationToken cancellationToken);

    Task<string> Save(DeliveryPhaseDto deliveryPhase, Guid organisationId, IEnumerable<string> fieldsToSave, CancellationToken cancellationToken);
}
