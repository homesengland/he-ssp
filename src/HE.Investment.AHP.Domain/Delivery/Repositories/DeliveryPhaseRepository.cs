using System.Collections.Concurrent;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Exceptions;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Delivery.Repositories;

public class DeliveryPhaseRepository : IDeliveryPhaseRepository
{
    private static readonly IDictionary<ApplicationId, DeliveryPhasesEntity> DeliveryPhases = new ConcurrentDictionary<ApplicationId, DeliveryPhasesEntity>();

    private readonly IApplicationRepository _applicationRepository;

    public DeliveryPhaseRepository(IApplicationRepository applicationRepository)
    {
        _applicationRepository = applicationRepository;
    }

    public async Task<DeliveryPhasesEntity> GetByApplicationId(ApplicationId applicationId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        if (!DeliveryPhases.TryGetValue(applicationId, out var deliveryPhases))
        {
            var application = await _applicationRepository.GetApplicationBasicInfo(applicationId, userAccount, cancellationToken);
            deliveryPhases = new DeliveryPhasesEntity(
                application,
                new[]
                {
                    new DeliveryPhaseEntity(application, "Phase 1", SectionStatus.InProgress, new DeliveryPhaseId("phase-1"), new DateTime(2023, 12, 12)),
                },
                SectionStatus.InProgress);

            DeliveryPhases.Add(applicationId, deliveryPhases);
        }

        return deliveryPhases;
    }

    public Task<IDeliveryPhaseEntity> GetById(ApplicationId applicationId, DeliveryPhaseId deliveryPhaseId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        if (!DeliveryPhases.TryGetValue(applicationId, out var deliveryPhases))
        {
            throw new NotFoundException(nameof(DeliveryPhaseEntity), deliveryPhaseId);
        }

        return Task.FromResult(deliveryPhases.GetById(deliveryPhaseId));
    }

    public Task Save(DeliveryPhasesEntity deliveryPhases, OrganisationId organisationId, CancellationToken cancellationToken)
    {
        // TODO: CRM integration
        return Task.CompletedTask;
    }
}
