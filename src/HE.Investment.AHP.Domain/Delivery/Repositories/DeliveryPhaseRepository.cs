using System.Collections.Concurrent;
using HE.Investment.AHP.Contract.Delivery.Events;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Infrastructure.Events;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Delivery.Repositories;

public class DeliveryPhaseRepository : IDeliveryPhaseRepository
{
    private static readonly IDictionary<ApplicationId, DeliveryPhasesEntity> DeliveryPhases = new ConcurrentDictionary<ApplicationId, DeliveryPhasesEntity>();

    private readonly IApplicationRepository _applicationRepository;

    private readonly IEventDispatcher _eventDispatcher;

    public DeliveryPhaseRepository(IApplicationRepository applicationRepository, IEventDispatcher eventDispatcher)
    {
        _applicationRepository = applicationRepository;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<DeliveryPhasesEntity> GetByApplicationId(ApplicationId applicationId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        if (!DeliveryPhases.Any())
        {
            await InitMockedData(applicationId, userAccount, cancellationToken);
        }

        if (!DeliveryPhases.TryGetValue(applicationId, out var deliveryPhases))
        {
            throw new NotFoundException(nameof(DeliveryPhasesEntity), applicationId);
        }

        return deliveryPhases;
    }

    public async Task<DeliveryPhaseId> Save(IDeliveryPhaseEntity deliveryPhase, OrganisationId organisationId, CancellationToken cancellationToken)
    {
        var entity = (DeliveryPhaseEntity)deliveryPhase;
        if (entity.IsNew)
        {
            // TODO: Save Delivery Phase in CRM
            entity.Id = new DeliveryPhaseId(Guid.NewGuid().ToString());
            await _eventDispatcher.Publish(
                new DeliveryPhaseHasBeenCreatedEvent(entity.Application.Id.Value),
                cancellationToken);
        }
        else if (entity.IsModified)
        {
            // TODO: Save Delivery Phase in CRM
            await _eventDispatcher.Publish(new DeliveryPhaseHasBeenUpdatedEvent(entity.Application.Id.Value), cancellationToken);
        }

        return entity.Id;
    }

    public async Task<IDeliveryPhaseEntity> GetById(ApplicationId applicationId, DeliveryPhaseId deliveryPhaseId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        if (!DeliveryPhases.Any())
        {
            await InitMockedData(applicationId, userAccount, cancellationToken);
        }

        if (!DeliveryPhases.TryGetValue(applicationId, out var deliveryPhases))
        {
            throw new NotFoundException(nameof(DeliveryPhaseEntity), deliveryPhaseId);
        }

        return deliveryPhases.GetById(deliveryPhaseId);
    }

    public async Task Save(DeliveryPhasesEntity deliveryPhases, OrganisationId organisationId, CancellationToken cancellationToken)
    {
        if (deliveryPhases.IsStatusChanged)
        {
            // TODO: Update Delivery section status to In Progress in CRM
        }

        foreach (var deliveryPhaseToRemove in deliveryPhases.ToRemove)
        {
            // TODO: remove delivery Phase in CRM
            await _eventDispatcher.Publish(
                new DeliveryPhaseHasBeenRemovedEvent(deliveryPhaseToRemove.Application.Id.Value),
                cancellationToken);
        }
    }

    private async Task InitMockedData(ApplicationId applicationId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var application =
            await _applicationRepository.GetApplicationBasicInfo(applicationId, userAccount, cancellationToken);
        var homesToDeliver = new[]
        {
            new HomesToDeliver(new HomeTypeId("ht-1"), new HomeTypeName("1 bed flat"), 3),
            new HomesToDeliver(new HomeTypeId("ht-2"), new HomeTypeName("2 bed flat"), 2),
            new HomesToDeliver(new HomeTypeId("ht-3"), new HomeTypeName("3 bed flat"), 1),
            new HomesToDeliver(new HomeTypeId("ht-4"), new HomeTypeName("2 bed house"), 4),
        };
        var deliveryPhases = new DeliveryPhasesEntity(
            application,
            new[]
            {
                new DeliveryPhaseEntity(
                    application,
                    "Phase 1",
                    SectionStatus.InProgress,
                    new[] { new HomesToDeliverInPhase(new HomeTypeId("ht-1"), 3) },
                    new DeliveryPhaseId("phase-1"),
                    new DateTime(2023, 12, 12),
                    new AcquisitionMilestoneDetails(new AcquisitionDate("1", "2", "2023"), null)),
            },
            homesToDeliver,
            SectionStatus.InProgress);

        DeliveryPhases.Add(applicationId, deliveryPhases);
    }
}
