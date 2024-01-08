using Dawn;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Delivery.Entities;

public class DeliveryPhaseEntity : IDeliveryPhaseEntity
{
    private readonly IList<HomesToDeliverInPhase> _homesToDeliver;

    private readonly ModificationTracker _modificationTracker = new();

    public DeliveryPhaseEntity(
        ApplicationBasicInfo application,
        string? name,
        SectionStatus status,
        IEnumerable<HomesToDeliverInPhase> homesToDeliver,
        DeliveryPhaseId? id = null,
        DateTime? createdOn = null,
        AcquisitionMilestoneDetails? acquisitionMilestone = null,
        StartOnSiteMilestoneDetails? startOnSiteMilestone = null,
        CompletionMilestoneDetails? completionMilestone = null)
    {
        Application = application;
        Name = new DeliveryPhaseName(name);
        Status = status;
        Id = id ?? DeliveryPhaseId.New();
        CreatedOn = createdOn;
        AcquisitionMilestone = acquisitionMilestone;
        StartOnSiteMilestone = startOnSiteMilestone;
        CompletionMilestone = completionMilestone;
        _homesToDeliver = homesToDeliver.ToList();
    }

    public ApplicationBasicInfo Application { get; }

    public DeliveryPhaseId Id { get; set; }

    public DeliveryPhaseName Name { get; private set; }

    public TypeOfHomes TypeOfHomes { get; private set; }

    public DateTime? CreatedOn { get; }

    public SectionStatus Status { get; private set; }

    public bool IsNew => Id.IsNew;

    public bool IsModified => _modificationTracker.IsModified;

    public IEnumerable<HomesToDeliverInPhase> HomesToDeliver => _homesToDeliver;

    public int TotalHomesToBeDeliveredInThisPhase => _homesToDeliver.Select(x => x.ToDeliver).Sum();

    public AcquisitionMilestoneDetails? AcquisitionMilestone { get; private set; }

    public StartOnSiteMilestoneDetails? StartOnSiteMilestone { get; private set; }

    public CompletionMilestoneDetails? CompletionMilestone { get; private set; }

    public bool IsHomeTypeUsed(HomeTypeId homeTypeId)
    {
        return _homesToDeliver.Any(x => x.HomeTypeId == homeTypeId);
    }

    public int GetHomesToBeDeliveredForHomeType(HomeTypeId homeTypeId)
    {
        return _homesToDeliver
            .Where(x => x.HomeTypeId == homeTypeId)
            .Select(x => x.ToDeliver)
            .Sum();
    }

    public void SetHomesToBeDeliveredInThisPhase(IReadOnlyCollection<HomesToDeliverInPhase> homesToDeliver)
    {
        if (homesToDeliver.DistinctBy(x => x.HomeTypeId).Count() != homesToDeliver.Count)
        {
            throw new InvalidOperationException("Each Home Type can be selected only once.");
        }

        var uniqueHomes = homesToDeliver.OrderBy(x => x.HomeTypeId.Value).ToList();
        if (!_homesToDeliver.SequenceEqual(uniqueHomes))
        {
            _homesToDeliver.Clear();
            _homesToDeliver.AddRange(uniqueHomes);
            _modificationTracker.MarkAsModified();
            MarkAsNotCompleted();
        }
    }

    public void ProvideAcquisitionMilestoneDetails(AcquisitionMilestoneDetails? details)
    {
        AcquisitionMilestone = _modificationTracker.Change(AcquisitionMilestone, details, MarkAsNotCompleted);
    }

    public void ProvideStartOnSiteMilestoneDetails(StartOnSiteMilestoneDetails? details)
    {
        StartOnSiteMilestone = _modificationTracker.Change(StartOnSiteMilestone, details, MarkAsNotCompleted);
    }

    public void ProvideCompletionMilestoneDetails(CompletionMilestoneDetails? details)
    {
        CompletionMilestone = _modificationTracker.Change(CompletionMilestone, details, MarkAsNotCompleted);
    }

    public void ProvideTypeOfHomes(TypeOfHomes typeOfHomes)
    {
        TypeOfHomes = _modificationTracker.Change(TypeOfHomes, typeOfHomes.NotDefault(), MarkAsNotCompleted);
    }

    private void MarkAsNotCompleted()
    {
        Status = _modificationTracker.Change(Status, SectionStatus.InProgress);
    }
}
