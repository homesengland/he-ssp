using System.Collections.Concurrent;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Contract.Delivery.Events;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Infrastructure.Events;

namespace HE.Investment.AHP.Domain.Delivery.Repositories;

public class DeliveryPhaseRepository : IDeliveryPhaseRepository
{
    private static readonly IDictionary<AhpApplicationId, DeliveryPhasesEntity> DeliveryPhases = new ConcurrentDictionary<AhpApplicationId, DeliveryPhasesEntity>();

    private readonly IApplicationRepository _applicationRepository;

    private readonly IHomeTypeRepository _homeTypeRepository;

    private readonly IEventDispatcher _eventDispatcher;

    public DeliveryPhaseRepository(IApplicationRepository applicationRepository, IHomeTypeRepository homeTypeRepository, IEventDispatcher eventDispatcher)
    {
        _applicationRepository = applicationRepository;
        _homeTypeRepository = homeTypeRepository;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<DeliveryPhasesEntity> GetByApplicationId(AhpApplicationId applicationId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        await InitMockedData(applicationId, userAccount, cancellationToken);

        if (!DeliveryPhases.TryGetValue(applicationId, out var deliveryPhases))
        {
            throw new NotFoundException(nameof(DeliveryPhasesEntity), applicationId);
        }

        return deliveryPhases;
    }

    public async Task<IDeliveryPhaseEntity> GetById(
    AhpApplicationId applicationId,
    DeliveryPhaseId deliveryPhaseId,
    UserAccount userAccount,
    CancellationToken cancellationToken)
    {
        await InitMockedData(applicationId, userAccount, cancellationToken);
        if (!DeliveryPhases.TryGetValue(applicationId, out var deliveryPhases))
        {
            throw new NotFoundException(nameof(DeliveryPhaseEntity), deliveryPhaseId);
        }

        return deliveryPhases.GetById(deliveryPhaseId);
    }

    public async Task<DeliveryPhaseId> Save(IDeliveryPhaseEntity deliveryPhase, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var entity = (DeliveryPhaseEntity)deliveryPhase;
        await InitMockedData(entity.Application.Id, userAccount, cancellationToken);
        if (entity.IsNew)
        {
            entity.Id = new DeliveryPhaseId(Guid.NewGuid().ToString());
            await _eventDispatcher.Publish(
                new DeliveryPhaseHasBeenCreatedEvent(entity.Application.Id, entity.Name.Value),
                cancellationToken);
        }
        else if (entity.IsModified)
        {
            await _eventDispatcher.Publish(new DeliveryPhaseHasBeenUpdatedEvent(entity.Application.Id), cancellationToken);
        }

        return entity.Id;
    }

    public async Task Save(DeliveryPhasesEntity deliveryPhases, OrganisationId organisationId, CancellationToken cancellationToken)
    {
        if (deliveryPhases.IsStatusChanged)
        {
#pragma warning disable S1135 // Track uses of "TODO" tags
            //// TODO: AB#66083 Update Delivery section status to In Progress in CRM
#pragma warning restore S1135 // Track uses of "TODO" tags

        }

        var deliveryPhaseToRemove = deliveryPhases.PopRemovedDeliveryPhase();
        while (deliveryPhaseToRemove != null)
        {
#pragma warning disable S1135 // Track uses of "TODO" tags
            // TODO: AB#66083 remove delivery Phase in CRM
#pragma warning restore S1135 // Track uses of "TODO" tags

            await _eventDispatcher.Publish(
                new DeliveryPhaseHasBeenRemovedEvent(deliveryPhaseToRemove.Application.Id),
                cancellationToken);

            deliveryPhaseToRemove = deliveryPhases.PopRemovedDeliveryPhase();
        }
    }

    private async Task InitMockedData(AhpApplicationId applicationId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        if (DeliveryPhases.ContainsKey(applicationId))
        {
            return;
        }

        var application = await _applicationRepository.GetApplicationBasicInfo(applicationId, userAccount, cancellationToken);
        var homeTypes = await _homeTypeRepository.GetByApplicationId(
            applicationId,
            userAccount,
            new[] { HomeTypeSegmentType.HomeInformation },
            cancellationToken);

        var homesToDeliver = homeTypes.HomeTypes.Select(x => new HomesToDeliver(x.Id, x.Name, x.HomeInformation.NumberOfHomes?.Value ?? 0)).ToList();
        var deliveryPhases = new DeliveryPhasesEntity(
            application,
            new[]
            {
                new DeliveryPhaseEntity(
                    application,
                    new DeliveryPhaseName("Phase 1"),
                    userAccount.SelectedOrganisation(),
                    SectionStatus.InProgress,
                    null,
                    new BuildActivity(application.Tenure),
                    null,
                    homesToDeliver.Any() ? new[] { new HomesToDeliverInPhase(homesToDeliver[0].HomeTypeId, homesToDeliver[0].TotalHomes) } : new List<HomesToDeliverInPhase>(),
                    new DeliveryPhaseMilestones(userAccount.SelectedOrganisation(), completionMilestone: new CompletionMilestoneDetails(new CompletionDate("1", "2", "2023"), null)),
                    new DeliveryPhaseId("phase-1"),
                    new DateTime(2023, 12, 12, 0, 0, 0, DateTimeKind.Unspecified)),
                new DeliveryPhaseEntity(
                    application,
                    new DeliveryPhaseName("Almost completed rehab"),
                    userAccount.SelectedOrganisation(),
                    SectionStatus.InProgress,
                    TypeOfHomes.Rehab,
                    new BuildActivity(application.Tenure, TypeOfHomes.Rehab, BuildActivityType.RegenerationRehab),
                    true,
                    new List<HomesToDeliverInPhase>(),
                    new DeliveryPhaseMilestones(userAccount.SelectedOrganisation(), completionMilestone: new CompletionMilestoneDetails(new CompletionDate("1", "2", "2023"), null)),
                    new DeliveryPhaseId("phase-2"),
                    new DateTime(2023, 12, 12, 0, 0, 0, DateTimeKind.Unspecified)),
            },
            homesToDeliver,
            SectionStatus.InProgress);

        DeliveryPhases.Add(applicationId, deliveryPhases);
    }
}
