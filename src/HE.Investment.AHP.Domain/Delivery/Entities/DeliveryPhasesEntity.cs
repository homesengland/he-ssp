using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Delivery.Tranches;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Delivery.Entities;

public class DeliveryPhasesEntity : IHomeTypeConsumer
{
    private readonly List<DeliveryPhaseEntity> _deliveryPhases;

    private readonly List<DeliveryPhaseEntity> _toRemove = new();

    private readonly List<HomesToDeliver> _homesToDeliver;

    private readonly ModificationTracker _statusModificationTracker = new();

    public DeliveryPhasesEntity(
        ApplicationBasicInfo application,
        IEnumerable<DeliveryPhaseEntity> deliveryPhases,
        IEnumerable<HomesToDeliver> homesToDeliver,
        SectionStatus status)
    {
        Application = application;
        _deliveryPhases = deliveryPhases.ToList();
        _homesToDeliver = homesToDeliver.ToList();
        Status = status;
    }

    public ApplicationBasicInfo Application { get; }

    public IEnumerable<IDeliveryPhaseEntity> DeliveryPhases => _deliveryPhases;

    public SectionStatus Status { get; private set; }

    public bool IsStatusChanged => _statusModificationTracker.IsModified;

    public int UnusedHomeTypesCount => _homesToDeliver.Select(x => x.TotalHomes).Sum() -
                                       _homesToDeliver.Select(x => GetHomesToBeDeliveredInAllPhases(x.HomeTypeId)).Sum();

    public string HomeTypeConsumerName => "Delivery Phase";

    public IDeliveryPhaseEntity? PopRemovedDeliveryPhase()
    {
        if (_toRemove.Any())
        {
            var result = _toRemove[0];
            _toRemove.RemoveAt(0);
            return result;
        }

        return null;
    }

    public bool IsHomeTypeUsed(HomeTypeId homeTypeId) => _deliveryPhases.Exists(x => x.IsHomeTypeUsed(homeTypeId));

    public IEnumerable<(HomesToDeliver HomesToDeliver, int? ToDeliver)> GetHomesToDeliverInPhase(DeliveryPhaseId deliveryPhaseId)
    {
        var deliveryPhase = GetEntityById(deliveryPhaseId);

        foreach (var homesToDeliver in _homesToDeliver)
        {
            var toBeDeliveredInTotal = GetHomesToBeDeliveredInAllPhases(homesToDeliver.HomeTypeId);
            var toBeDeliveredInPhase = deliveryPhase.GetHomesToBeDeliveredForHomeType(homesToDeliver.HomeTypeId);
            if (toBeDeliveredInTotal < homesToDeliver.TotalHomes || toBeDeliveredInPhase.IsProvided())
            {
                yield return (homesToDeliver, toBeDeliveredInPhase);
            }
        }
    }

    public void ProvideHomesToBeDeliveredInPhase(DeliveryPhaseId deliveryPhaseId, IReadOnlyCollection<HomesToDeliverInPhase> homesToDeliver)
    {
        if (!_homesToDeliver.Any())
        {
            OperationResult.ThrowValidationError(nameof(HomesToDeliver), "You must add at least 1 home type in home types section");
        }

        if (homesToDeliver.All(x => x.Value == 0))
        {
            OperationResult.ThrowValidationError(nameof(HomesToDeliver), "You must add at least 1 home to a home type for this delivery phase");
        }

        GetEntityById(deliveryPhaseId).SetHomesToBeDeliveredInThisPhase(homesToDeliver);

        var errors = new List<ErrorItem>();
        foreach (var homeTypeId in homesToDeliver.Select(x => x.HomeTypeId))
        {
            var homeToDeliver = _homesToDeliver.SingleOrDefault(x => x.HomeTypeId == homeTypeId)
                                ?? throw new NotFoundException(nameof(HomesToDeliver), homeTypeId);
            if (GetHomesToBeDeliveredInAllPhases(homeTypeId) > homeToDeliver.TotalHomes)
            {
                errors.Add(new ErrorItem(HomesToDeliverInPhase.AffectedField(homeTypeId), "You have entered more homes to this home type than are in the application"));
            }
        }

        if (errors.Any())
        {
            OperationResult.New().AddValidationErrors(errors).CheckErrors();
        }

        MarkAsInProgress();
    }

    public DeliveryPhaseEntity CreateDeliveryPhase(DeliveryPhaseName name, OrganisationBasicInfo organisationBasicInfo)
    {
        var deliveryPhase = new DeliveryPhaseEntity(
            Application,
            ValidateNameUniqueness(name),
            organisationBasicInfo,
            SectionStatus.InProgress,
            MilestonesPercentageTranches.NotProvided,
            MilestonesTranches.LackOfCalculation,
            false,
            new SchemeFunding((int?)null, null));

        _deliveryPhases.Add(deliveryPhase);
        return deliveryPhase;
    }

    public void ProvideDeliveryPhaseName(DeliveryPhaseId deliveryPhaseId, DeliveryPhaseName name)
    {
        var entity = GetEntityById(deliveryPhaseId);
        entity.ProvideName(ValidateNameUniqueness(name, entity));
    }

    public void Remove(DeliveryPhaseId deliveryPhaseId, RemoveDeliveryPhaseAnswer removeAnswer)
    {
        var deliveryPhase = GetEntityById(deliveryPhaseId);
        if (removeAnswer == RemoveDeliveryPhaseAnswer.Undefined)
        {
            OperationResult.ThrowValidationError(nameof(RemoveDeliveryPhaseAnswer), "Select whether you want to remove this delivery phase");
        }

        if (removeAnswer == RemoveDeliveryPhaseAnswer.Yes)
        {
            _toRemove.Add(deliveryPhase);
            _deliveryPhases.Remove(deliveryPhase);
        }
    }

    public void CompleteSection(IsDeliveryCompleted isDeliveryCompleted)
    {
        if (isDeliveryCompleted == IsDeliveryCompleted.Undefied)
        {
            OperationResult.ThrowValidationError(nameof(IsDeliveryCompleted), "Select whether you have completed this section");
        }

        if (isDeliveryCompleted == IsDeliveryCompleted.Yes)
        {
            if (!_deliveryPhases.Any())
            {
                OperationResult.ThrowValidationError(
                    "DeliveryPhases",
                    "Delivery Section cannot be completed because at least one Delivery Phase needs to be added.");
            }

            var notCompletedDeliveryPhases = _deliveryPhases.Where(x => x.Status != SectionStatus.Completed).ToList();
            if (notCompletedDeliveryPhases.Any())
            {
                OperationResult.New()
                    .AddValidationErrors(notCompletedDeliveryPhases
                        .Select(x => new ErrorItem($"DeliveryPhase-{x.Id}", $"Complete {x.Name.Value} to save and continue"))
                        .ToList())
                    .CheckErrors();
            }

            if (!AreAllHomeTypesUsed())
            {
                OperationResult.ThrowValidationError("DeliveryPhases", "The number of homes assigned to delivery phases does not match the number of homes added in scheme information.");
            }

            Status = _statusModificationTracker.Change(Status, SectionStatus.Completed);
        }
        else
        {
            MarkAsInProgress();
        }
    }

    public IDeliveryPhaseEntity GetById(DeliveryPhaseId deliveryPhaseId) => GetEntityById(deliveryPhaseId);

    public void MarkAsInProgress()
    {
        Status = _statusModificationTracker.Change(Status, SectionStatus.InProgress);
    }

    private DeliveryPhaseName ValidateNameUniqueness(DeliveryPhaseName name, DeliveryPhaseEntity? entity = null)
    {
        if ((entity == null && _deliveryPhases.Exists(x => x.Name == name))
            || (entity != null && _deliveryPhases.Except(new[] { entity }).Any(x => x.Name == name)))
        {
            OperationResult.ThrowValidationError(nameof(DeliveryPhaseName), "Provided delivery phase name is already in use. Delivery phase name should be unique.");
        }

        return name;
    }

    private DeliveryPhaseEntity GetEntityById(DeliveryPhaseId deliveryPhaseId) => _deliveryPhases.SingleOrDefault(x => x.Id == deliveryPhaseId)
                                                                                  ?? throw new NotFoundException(nameof(DeliveryPhaseEntity), deliveryPhaseId);

    private bool AreAllHomeTypesUsed()
    {
        foreach (var (homeTypeId, _, totalToDeliver) in _homesToDeliver)
        {
            var toBeDelivered = GetHomesToBeDeliveredInAllPhases(homeTypeId);
            if (toBeDelivered != totalToDeliver)
            {
                return false;
            }
        }

        return true;
    }

    private int GetHomesToBeDeliveredInAllPhases(HomeTypeId homeTypeId)
    {
        return _deliveryPhases.SelectMany(x => x.HomesToDeliver)
            .Where(x => x.HomeTypeId == homeTypeId)
            .Select(x => x.Value)
            .Sum();
    }
}
