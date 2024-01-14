using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Delivery.Policies;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Delivery.Entities;

public class DeliveryPhaseEntity : IDeliveryPhaseEntity
{
    private readonly IList<HomesToDeliverInPhase> _homesToDeliver;

    private readonly ModificationTracker _modificationTracker = new();

    public DeliveryPhaseEntity(
        ApplicationBasicInfo application,
        DeliveryPhaseName name,
        OrganisationBasicInfo organisation,
        TypeOfHomes? typeOfHomes,
        BuildActivityType buildActivityType,
        SectionStatus status,
        IEnumerable<HomesToDeliverInPhase> homesToDeliver,
        DeliveryPhaseMilestones milestones,
        DeliveryPhaseId? id = null,
        DateTime? createdOn = null,
        IsAdditionalPaymentRequested? isAdditionalPaymentRequested = null)
    {
        Application = application;
        Name = name;
        Organisation = organisation;
        TypeOfHomes = typeOfHomes;
        BuildActivityType = buildActivityType;
        Status = status;
        Id = id ?? DeliveryPhaseId.New();
        CreatedOn = createdOn;
        DeliveryPhaseMilestones = milestones;
        IsAdditionalPaymentRequested = isAdditionalPaymentRequested;
        _homesToDeliver = homesToDeliver.ToList();
    }

    public ApplicationBasicInfo Application { get; }

    public OrganisationBasicInfo Organisation { get; }

    public DeliveryPhaseId Id { get; set; }

    public DeliveryPhaseName Name { get; private set; }

    public TypeOfHomes? TypeOfHomes { get; private set; }

    public BuildActivityType BuildActivityType { get; private set; }

    public DateTime? CreatedOn { get; }

    public SectionStatus Status { get; private set; }

    public bool IsNew => Id.IsNew;

    public bool IsModified => _modificationTracker.IsModified;

    public IEnumerable<HomesToDeliverInPhase> HomesToDeliver => _homesToDeliver;

    public int TotalHomesToBeDeliveredInThisPhase => _homesToDeliver.Select(x => x.ToDeliver).Sum();

    public DeliveryPhaseMilestones DeliveryPhaseMilestones { get; private set; }

    public IsAdditionalPaymentRequested? IsAdditionalPaymentRequested { get; private set; }

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
            throw new DomainValidationException("Each Home Type can be selected only once.");
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

    public void ProvideName(DeliveryPhaseName deliveryPhaseName)
    {
        Name = _modificationTracker.Change(Name, deliveryPhaseName, MarkAsNotCompleted);
    }

    public async Task ProvideDeliveryPhaseMilestones(DeliveryPhaseMilestones milestones, IMilestoneDatesInProgrammeDateRangePolicy policy)
    {
        await policy.Validate(milestones);

        DeliveryPhaseMilestones = _modificationTracker.Change(DeliveryPhaseMilestones, milestones, MarkAsNotCompleted);
    }

    public void ProvideTypeOfHomes(TypeOfHomes typeOfHomes)
    {
        if (typeOfHomes != TypeOfHomes)
        {
            BuildActivityType.ClearAnswer();
        }

        TypeOfHomes = _modificationTracker.Change(TypeOfHomes, typeOfHomes.NotDefault(), MarkAsNotCompleted);
    }

    public void ProvideAdditionalPaymentRequest(IsAdditionalPaymentRequested? isAdditionalPaymentRequested)
    {
        if (!Organisation.IsUnregisteredBody)
        {
            throw new DomainValidationException("Cannot provide Additional Payment Request for Registered Partner.");
        }

        IsAdditionalPaymentRequested = _modificationTracker.Change(IsAdditionalPaymentRequested, isAdditionalPaymentRequested, MarkAsNotCompleted);
    }

    public void Complete()
    {
        if (!IsAnswered())
        {
#pragma warning disable S1135 // Track uses of "TODO" tags
            // TODO #67047: throw and handle exception
            throw new DomainValidationException("Cannot complete deliveryPhase.");
#pragma warning restore S1135 // Track uses of "TODO" tags
        }

        Status = _modificationTracker.Change(Status, SectionStatus.Completed);
    }

    public void ProvideBuildActivityType(BuildActivityType buildActivityType)
    {
        BuildActivityType = _modificationTracker.Change(BuildActivityType, buildActivityType, MarkAsNotCompleted);
    }

    private bool IsAnswered()
    {
        if (Organisation.IsUnregisteredBody)
        {
            return DeliveryPhaseMilestones.IsAnswered() &&
                   IsAdditionalPaymentRequested != null && IsAdditionalPaymentRequested.IsAnswered();
        }

        return DeliveryPhaseMilestones.IsAnswered();
    }

    private void MarkAsNotCompleted()
    {
        Status = _modificationTracker.Change(Status, SectionStatus.InProgress);
    }
}
