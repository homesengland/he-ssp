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
using HE.Investments.Common.Messages;
using SummaryOfDelivery = HE.Investment.AHP.Domain.Delivery.ValueObjects.SummaryOfDelivery;

namespace HE.Investment.AHP.Domain.Delivery.Entities;

public class DeliveryPhaseEntity : DomainEntity, IDeliveryPhaseEntity
{
    private readonly IList<HomesToDeliverInPhase> _homesToDeliver;

    private readonly ModificationTracker _modificationTracker = new();

    public DeliveryPhaseEntity(
        ApplicationBasicInfo application,
        DeliveryPhaseName name,
        OrganisationBasicInfo organisation,
        SectionStatus status,
        TypeOfHomes? typeOfHomes = null,
        BuildActivity? buildActivity = null,
        bool? reconfiguringExisting = null,
        IEnumerable<HomesToDeliverInPhase>? homesToDeliver = null,
        DeliveryPhaseMilestones? milestones = null,
        DeliveryPhaseId? id = null,
        DateTime? createdOn = null,
        IsAdditionalPaymentRequested? isAdditionalPaymentRequested = null)
    {
        Id = id ?? DeliveryPhaseId.New();
        Application = application;
        Name = name;
        Organisation = organisation;
        TypeOfHomes = typeOfHomes;
        BuildActivity = buildActivity ?? new BuildActivity(application.Tenure);
        ReconfiguringExisting = reconfiguringExisting;
        Status = status;
        CreatedOn = createdOn;
        DeliveryPhaseMilestones = milestones ?? new DeliveryPhaseMilestones(organisation, BuildActivity);
        IsAdditionalPaymentRequested = isAdditionalPaymentRequested;
        _homesToDeliver = homesToDeliver?.ToList() ?? new List<HomesToDeliverInPhase>();
    }

    public ApplicationBasicInfo Application { get; }

    public OrganisationBasicInfo Organisation { get; }

    public DeliveryPhaseId Id { get; set; }

    public DeliveryPhaseName Name { get; private set; }

    public TypeOfHomes? TypeOfHomes { get; private set; }

    public BuildActivity BuildActivity { get; private set; }

    public bool? ReconfiguringExisting { get; private set; }

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

    public int? GetHomesToBeDeliveredForHomeType(HomeTypeId homeTypeId)
    {
        return _homesToDeliver.SingleOrDefault(x => x.HomeTypeId == homeTypeId)?.ToDeliver;
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

    public async Task ProvideDeliveryPhaseMilestones(
        DeliveryPhaseMilestones milestones,
        IMilestoneDatesInProgrammeDateRangePolicy policy,
        CancellationToken cancellationToken)
    {
        await policy.Validate(milestones, cancellationToken);

        DeliveryPhaseMilestones = _modificationTracker.Change(DeliveryPhaseMilestones, milestones, MarkAsNotCompleted);
    }

    public void ProvideTypeOfHomes(TypeOfHomes typeOfHomes)
    {
        TypeOfHomes = _modificationTracker.Change(TypeOfHomes, typeOfHomes.NotDefault(), MarkAsNotCompleted, ResetTypeOfHomesDependencies);
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
            throw new DomainValidationException(ValidationErrorMessage.SectionIsNotCompleted);
        }

        Status = _modificationTracker.Change(Status, SectionStatus.Completed);
    }

    public void UnComplete()
    {
        Status = _modificationTracker.Change(Status, SectionStatus.InProgress);
    }

    public SummaryOfDelivery CalculateSummary(decimal requiredFunding, int totalHousesToDeliver, MilestoneFramework milestoneFramework)
    {
        if (requiredFunding <= 0 || totalHousesToDeliver <= 0 || TotalHomesToBeDeliveredInThisPhase <= 0)
        {
            return SummaryOfDelivery.LackOfCalculation;
        }

        var grantApportioned = requiredFunding * TotalHomesToBeDeliveredInThisPhase / totalHousesToDeliver;

        if (Organisation.IsUnregisteredBody || BuildActivity.IsOffTheShelfOrExistingSatisfactory)
        {
            return new SummaryOfDelivery(grantApportioned, null, null, grantApportioned);
        }

        var acquisitionMilestone = (grantApportioned * milestoneFramework.AcquisitionPercentage).ToWholeNumberRoundFloor();
        var startOnSiteMilestone = (grantApportioned * milestoneFramework.StartOnSitePercentage).ToWholeNumberRoundFloor();
        var completionMilestone = grantApportioned - acquisitionMilestone - startOnSiteMilestone;

        return new SummaryOfDelivery(grantApportioned, acquisitionMilestone, startOnSiteMilestone, completionMilestone);
    }

    public void ProvideBuildActivity(BuildActivity buildActivity)
    {
        BuildActivity = _modificationTracker.Change(BuildActivity, buildActivity, MarkAsNotCompleted, ResetBuildActivityDependencies);
    }

    public void ProvideReconfiguringExisting(bool? reconfiguringExisting)
    {
        if (!IsReconfiguringExistingNeeded())
        {
            throw new DomainValidationException("Reconfiguring Existing is not needed.");
        }

        ReconfiguringExisting = _modificationTracker.Change(ReconfiguringExisting, reconfiguringExisting, MarkAsNotCompleted);
    }

    public bool IsReconfiguringExistingNeeded()
    {
        return TypeOfHomes == Contract.Delivery.Enums.TypeOfHomes.Rehab;
    }

    private bool IsAnswered()
    {
        var reconfigureExistingValid = !IsReconfiguringExistingNeeded() || ReconfiguringExisting.HasValue;

        var isAnswered = Name.IsProvided() &&
                         TypeOfHomes.IsProvided() &&
                         BuildActivity.IsAnswered() &&
                         reconfigureExistingValid &&
                         _homesToDeliver.Any() &&
                         DeliveryPhaseMilestones.IsAnswered();

        if (Organisation.IsUnregisteredBody)
        {
            return DeliveryPhaseMilestones.IsAnswered() &&
                   IsAdditionalPaymentRequested != null && IsAdditionalPaymentRequested.IsAnswered();
        }

        return isAnswered;
    }

    private void MarkAsNotCompleted()
    {
        Status = _modificationTracker.Change(Status, SectionStatus.InProgress);
    }

    private void ResetTypeOfHomesDependencies(TypeOfHomes? newTypeOfHomes)
    {
        ProvideBuildActivity(BuildActivity.WithClearedAnswer(newTypeOfHomes.GetValueOrFirstValue()));
        ReconfiguringExisting = null;
    }

    private void ResetBuildActivityDependencies(BuildActivity newBuildActivity)
    {
        var milestones = new DeliveryPhaseMilestones(Organisation, newBuildActivity, DeliveryPhaseMilestones);
        DeliveryPhaseMilestones = _modificationTracker.Change(DeliveryPhaseMilestones, milestones, MarkAsNotCompleted);
    }
}
