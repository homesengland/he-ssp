using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Delivery.Repositories;

public class DeliveryPhaseRepository : IDeliveryPhaseRepository
{
    private readonly IApplicationRepository _applicationRepository;

    public DeliveryPhaseRepository(IApplicationRepository applicationRepository)
    {
        _applicationRepository = applicationRepository;
    }

    public async Task<DeliveryPhasesEntity> GetByApplicationId(ApplicationId applicationId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var application = await _applicationRepository.GetApplicationBasicInfo(applicationId, userAccount, cancellationToken);

        return new DeliveryPhasesEntity(
            application,
            new[]
            {
                new DeliveryPhaseEntity(application, "Phase 1", SectionStatus.InProgress, new DeliveryPhaseId("phase-1"), new DateTime(2023, 12, 12)),
            },
            SectionStatus.InProgress);
    }
}
