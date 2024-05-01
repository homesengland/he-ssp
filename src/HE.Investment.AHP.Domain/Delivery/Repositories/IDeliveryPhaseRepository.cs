using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Delivery.Repositories;

public interface IDeliveryPhaseRepository
{
    Task<DeliveryPhasesEntity> GetByApplicationId(
        AhpApplicationId applicationId,
        UserAccount userAccount,
        CancellationToken cancellationToken);

    Task<IDeliveryPhaseEntity> GetById(
        AhpApplicationId applicationId,
        DeliveryPhaseId deliveryPhaseId,
        UserAccount userAccount,
        CancellationToken cancellationToken);

    Task<DeliveryPhaseId> Save(IDeliveryPhaseEntity deliveryPhase, OrganisationId organisationId, CancellationToken cancellationToken);

    Task Save(DeliveryPhasesEntity deliveryPhases, OrganisationId organisationId, CancellationToken cancellationToken);
}
