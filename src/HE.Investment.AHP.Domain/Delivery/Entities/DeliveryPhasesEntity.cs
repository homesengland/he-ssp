using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Delivery.Entities;

public class DeliveryPhasesEntity : IHomeTypeConsumer
{
    private readonly IList<DeliveryPhaseEntity> _deliveryPhases;

    private readonly IList<DeliveryPhaseEntity> _toRemove = new List<DeliveryPhaseEntity>();

    private readonly ApplicationBasicInfo _application;

    private readonly IList<HomesToDeliver> _homesToDeliver;

    private readonly ModificationTracker _statusModificationTracker = new();

    public DeliveryPhasesEntity(
        ApplicationBasicInfo application,
        IEnumerable<DeliveryPhaseEntity> deliveryPhases,
        IEnumerable<HomesToDeliver> homesToDeliver,
        SectionStatus status)
    {
        _application = application;
        _deliveryPhases = deliveryPhases.ToList();
        _homesToDeliver = homesToDeliver.ToList();
        Status = status;
    }

    public AhpApplicationId ApplicationId => _application.Id;

    public ApplicationName ApplicationName => _application.Name;

    public IEnumerable<IDeliveryPhaseEntity> DeliveryPhases => _deliveryPhases;

    public IEnumerable<IDeliveryPhaseEntity> ToRemove => _toRemove;

    public SectionStatus Status { get; private set; }

    public bool IsStatusChanged => _statusModificationTracker.IsModified;

    public int UnusedHomeTypesCount => _homesToDeliver.Select(x => x.TotalHomes).Sum() -
                                       _homesToDeliver.Select(x => GetHomesToBeDeliveredInAllPhases(x.HomeTypeId)).Sum();

    public string HomeTypeConsumerName => "Delivery Phase";

    public bool IsHomeTypeUsed(HomeTypeId homeTypeId) => _deliveryPhases.Any(x => x.IsHomeTypeUsed(homeTypeId));

    public IEnumerable<(HomesToDeliver HomesToDeliver, int ToDeliver)> GetHomesToDeliverInPhase(DeliveryPhaseId deliveryPhaseId)
    {
        var deliveryPhase = GetEntityById(deliveryPhaseId);

        return _homesToDeliver.Select(x => (x, deliveryPhase.GetHomesToBeDeliveredForHomeType(x.HomeTypeId)));
    }

    public void SetHomesToBeDeliveredInPhase(DeliveryPhaseId deliveryPhaseId, IReadOnlyCollection<HomesToDeliverInPhase> homesToDeliver)
    {
        GetEntityById(deliveryPhaseId).SetHomesToBeDeliveredInThisPhase(homesToDeliver);

        var errors = new List<ErrorItem>();
        foreach (var (homeTypeId, _) in homesToDeliver)
        {
            var homeToDeliver = _homesToDeliver.SingleOrDefault(x => x.HomeTypeId == homeTypeId)
                                ?? throw new NotFoundException(nameof(HomesToDeliver), homeTypeId);
            if (GetHomesToBeDeliveredInAllPhases(homeTypeId) > homeToDeliver.TotalHomes)
            {
                errors.Add(new ErrorItem($"HomeType-{homeTypeId}", "You have entered more homes to this home type than are in the application"));
            }
        }

        if (errors.Any())
        {
            OperationResult.New().AddValidationErrors(errors).CheckErrors();
        }
    }

    public DeliveryPhaseEntity CreateDeliveryPhase(DeliveryPhaseName name)
    {
        // TODO: get proper organisation basic info
        var orgBasicInfo = new OrganisationBasicInfo(false);
        var deliveryPhaseNameAlreadyUsed = _deliveryPhases.Any(x => x.Name == name);
        if (deliveryPhaseNameAlreadyUsed)
        {
            OperationResult.New().AddValidationError(nameof(DeliveryPhaseName), "Provided delivery phase name is already in use. Delivery phase name should be unique.").CheckErrors();
        }

        var deliveryPhase = new DeliveryPhaseEntity(
            _application,
            name,
            orgBasicInfo,
            null,
            new BuildActivityType(),
            SectionStatus.InProgress,
            new List<HomesToDeliverInPhase>());

        _deliveryPhases.Add(deliveryPhase);
        return deliveryPhase;
    }

    public void Remove(DeliveryPhaseId deliveryPhaseId, RemoveDeliveryPhaseAnswer removeAnswer)
    {
        var deliveryPhase = GetEntityById(deliveryPhaseId);
        if (removeAnswer == RemoveDeliveryPhaseAnswer.Undefined)
        {
            OperationResult.New().AddValidationError(nameof(RemoveDeliveryPhaseAnswer), "Select whether you want to remove this delivery phase").CheckErrors();
        }

        if (removeAnswer == RemoveDeliveryPhaseAnswer.Yes)
        {
            _toRemove.Add(deliveryPhase);
            _deliveryPhases.Remove(deliveryPhase);
        }
    }

    public void CompleteSection(IsSectionCompleted isSectionCompleted)
    {
        if (isSectionCompleted == IsSectionCompleted.Undefied)
        {
            OperationResult.New().AddValidationError(nameof(IsSectionCompleted), "Select whether you have completed this section").CheckErrors();
        }

        if (isSectionCompleted == IsSectionCompleted.Yes)
        {
            if (!_deliveryPhases.Any())
            {
                throw new DomainValidationException(
                    new OperationResult().AddValidationErrors(new List<ErrorItem>
                    {
                        new("DeliveryPhases", "Delivery Section cannot be completed because at least one Delivery Phase needs to be added."),
                    }));
            }

            var notCompletedDeliveryPhases = _deliveryPhases.Where(x => x.Status != SectionStatus.Completed).ToList();
            if (notCompletedDeliveryPhases.Any())
            {
                throw new DomainValidationException(new OperationResult().AddValidationErrors(
                    notCompletedDeliveryPhases.Select(x => new ErrorItem($"DeliveryPhase-{x.Id}", $"Complete {x?.Name?.Value} to save and continue")).ToList()));
            }

            if (!AreAllHomeTypesUsed())
            {
                throw new DomainValidationException(
                    new OperationResult().AddValidationErrors(new List<ErrorItem>
                    {
                        new("DeliveryPhases", "Delivery Section cannot be completed because not all homes from Home Types are used."),
                    }));
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

    public void Add(DeliveryPhaseEntity deliveryPhase)
    {
        _deliveryPhases.Add(deliveryPhase);
    }

    public DeliveryPhaseEntity GetEntityById(DeliveryPhaseId deliveryPhaseId) => _deliveryPhases.SingleOrDefault(x => x.Id == deliveryPhaseId)
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
            .Select(x => x.ToDeliver)
            .Sum();
    }
}
