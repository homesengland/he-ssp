using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investments.Account.Shared.User;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Delivery.Repositories;

public interface IDeliveryPhaseRepository
{
    Task<DeliveryPhasesEntity> GetByApplicationId(
        ApplicationId applicationId,
        UserAccount userAccount,
        CancellationToken cancellationToken);
}
