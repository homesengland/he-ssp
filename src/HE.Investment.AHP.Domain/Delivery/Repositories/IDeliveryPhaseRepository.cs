using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract;
using HE.Investments.Consortium.Shared.UserContext;

namespace HE.Investment.AHP.Domain.Delivery.Repositories;

public interface IDeliveryPhaseRepository
{
    Task<DeliveryPhasesEntity> GetByApplicationId(
        AhpApplicationId applicationId,
        ConsortiumUserAccount userAccount,
        CancellationToken cancellationToken);

    Task<IDeliveryPhaseEntity> GetById(
        AhpApplicationId applicationId,
        DeliveryPhaseId deliveryPhaseId,
        ConsortiumUserAccount userAccount,
        CancellationToken cancellationToken);

    Task<DeliveryPhaseId> Save(IDeliveryPhaseEntity deliveryPhase, UserAccount userAccount, CancellationToken cancellationToken);

    Task Save(DeliveryPhasesEntity deliveryPhases, ConsortiumUserAccount userAccount, CancellationToken cancellationToken);
}
