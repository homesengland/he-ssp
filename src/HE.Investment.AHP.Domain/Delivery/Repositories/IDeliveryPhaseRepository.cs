using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.ValueObjects;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Delivery.Repositories;

public interface IDeliveryPhaseRepository
{
    Task<DeliveryPhasesEntity> GetByApplicationId(
        ApplicationId applicationId,
        UserAccount userAccount,
        CancellationToken cancellationToken);

    Task<IDeliveryPhaseEntity> GetById(
        ApplicationId applicationId,
        DeliveryPhaseId deliveryPhaseId,
        UserAccount userAccount,
        CancellationToken cancellationToken);

    Task Save(DeliveryPhasesEntity deliveryPhases, OrganisationId organisationId, CancellationToken cancellationToken);
}
