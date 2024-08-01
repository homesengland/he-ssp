using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Delivery.Policies;
using HE.Investment.AHP.Domain.Delivery.Tranches;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Utils;
using HE.Investments.Programme.Contract;
using DeliveryPhaseTranches = HE.Investment.AHP.Domain.Delivery.Tranches.DeliveryPhaseTranches;

namespace HE.Investment.AHP.Domain.Delivery.Entities;

public class DeliveryPhaseEntity : DomainEntity, IDeliveryPhaseEntity
{
    private readonly List<HomesToDeliverInPhase> _homesToDeliver;

    private readonly ModificationTracker _modificationTracker = new();

    private readonly IOnlyCompletionMilestonePolicy _onlyCompletionMilestonePolicy;

    public DeliveryPhaseEntity(
        ApplicationBasicInfo application,
        DeliveryPhaseName name,
        OrganisationBasicInfo organisation,
        SectionStatus status,
        MilestonesPercentageTranches milestonesPercentageTranches,
        MilestonesCalculatedTranches milestonesCalculatedTranches,
        bool milestoneTranchesAmendRequested,
        IOnlyCompletionMilestonePolicy onlyCompletionMilestonePolicy,
        TypeOfHomes? typeOfHomes = null,
        BuildActivity? buildActivity = null,
        bool? reconfiguringExisting = null,
        IEnumerable<HomesToDeliverInPhase>? homesToDeliver = null,
        AcquisitionMilestoneDetails? acquisitionMilestone = null,
        StartOnSiteMilestoneDetails? startOnSiteMilestone = null,
        CompletionMilestoneDetails? completionMilestone = null,
        DeliveryPhaseId? id = null,
        DateTime? createdOn = null,
        IsAdditionalPaymentRequested? isAdditionalPaymentRequested = null,
        bool? claimMilestone = null)
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
        _onlyCompletionMilestonePolicy = onlyCompletionMilestonePolicy;
        DeliveryPhaseMilestones = new DeliveryPhaseMilestones(IsOnlyCompletionMilestone, acquisitionMilestone, startOnSiteMilestone, completionMilestone);
        IsAdditionalPaymentRequested = isAdditionalPaymentRequested;
        _homesToDeliver = homesToDeliver?.ToList() ?? [];
        Tranches = new DeliveryPhaseTranches(
            Id,
            Application,
            milestonesPercentageTranches,
            milestonesCalculatedTranches,
            milestoneTranchesAmendRequested,
            claimMilestone,
            IsOnlyCompletionMilestone);

        Tranches.TranchesAmended += MarkAsNotCompleted;
    }

    public ApplicationBasicInfo Application { get; }

    public OrganisationBasicInfo Organisation { get; }

    public DeliveryPhaseId Id { get; set; }

    public DeliveryPhaseName Name { get; private set; }

    public TypeOfHomes? TypeOfHomes { get; private set; }

    public BuildActivity BuildActivity { get; private set; }

    public DeliveryPhaseTranches Tranches { get; private set; }

    public bool? ReconfiguringExisting { get; private set; }

    public DateTime? CreatedOn { get; }

    public SectionStatus Status { get; private set; }

    public bool IsNew => Id.IsNew;

    public bool IsModified => _modificationTracker.IsModified || Tranches.IsModified;

    public bool IsOnlyCompletionMilestone => _onlyCompletionMilestonePolicy.Validate(Organisation.IsUnregisteredBody, BuildActivity);

    public IEnumerable<HomesToDeliverInPhase> HomesToDeliver => _homesToDeliver;

    public int TotalHomesToBeDeliveredInThisPhase => _homesToDeliver.Select(x => x.Value).Sum();

    public DeliveryPhaseMilestones DeliveryPhaseMilestones { get; private set; }

    public IsAdditionalPaymentRequested? IsAdditionalPaymentRequested { get; private set; }

    public bool IsHomeTypeUsed(HomeTypeId homeTypeId)
    {
        return _homesToDeliver.Exists(x => x.HomeTypeId == homeTypeId && x.Value > 0);
    }

    public int? GetHomesToBeDeliveredForHomeType(HomeTypeId homeTypeId)
    {
        return _homesToDeliver.SingleOrDefault(x => x.HomeTypeId == homeTypeId)?.Value;
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
            Tranches.UnClaimMilestone();
        }
    }

    public void ProvideName(DeliveryPhaseName deliveryPhaseName)
    {
        Name = _modificationTracker.Change(Name, deliveryPhaseName, MarkAsNotCompleted);
    }

    public void ProvideDeliveryPhaseMilestones(DeliveryPhaseMilestones milestones)
    {
        DeliveryPhaseMilestones = _modificationTracker.Change(DeliveryPhaseMilestones, milestones, MarkAsNotCompleted);
    }

    public void ProvideTypeOfHomes(TypeOfHomes? typeOfHomes)
    {
        if (typeOfHomes.IsNotProvided())
        {
            OperationResult.ThrowValidationError("TypeOfHomes", "Select the type of homes you are delivering in this phase");
        }

        TypeOfHomes = _modificationTracker.Change(TypeOfHomes, typeOfHomes!.Value.NotDefault(), MarkAsNotCompleted, ResetTypeOfHomesDependencies);
    }

    public void ProvideAdditionalPaymentRequest(IsAdditionalPaymentRequested? isAdditionalPaymentRequested)
    {
        if (!Organisation.IsUnregisteredBody)
        {
            throw new DomainValidationException("Cannot provide Additional Payment Request for Registered Partner.");
        }

        IsAdditionalPaymentRequested = _modificationTracker.Change(IsAdditionalPaymentRequested, isAdditionalPaymentRequested, MarkAsNotCompleted);
    }

    public void Complete(Programme programme, IsSectionCompleted? isSectionCompleted, IDateTimeProvider dateTimeProvider)
    {
        if (isSectionCompleted == null)
        {
            OperationResult.ThrowValidationError("IsSectionCompleted", ValidationErrorMessage.NoCheckAnswers);
        }

        if (isSectionCompleted == IsSectionCompleted.No)
        {
            Status = _modificationTracker.Change(Status, SectionStatus.InProgress);
            return;
        }

        if (!IsAnswered())
        {
            OperationResult.ThrowValidationError("IsSectionCompleted", ValidationErrorMessage.SectionIsNotCompleted);
        }

        DeliveryPhaseMilestones.ValidateMilestoneDates(programme, dateTimeProvider);

        Status = _modificationTracker.Change(Status, SectionStatus.Completed);
    }

    public void ProvideBuildActivity(BuildActivity buildActivity)
    {
        if (buildActivity.IsNotAnswered())
        {
            OperationResult.ThrowValidationError("BuildActivityType", "Select the build activity type");
        }

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
        return TypeOfHomes is Contract.Delivery.Enums.TypeOfHomes.Rehab && BuildActivity.Type != BuildActivityType.ExistingSatisfactory;
    }

    private bool IsAnswered()
    {
        var reconfigureExistingValid = !IsReconfiguringExistingNeeded() || ReconfiguringExisting.HasValue;

        var isAnswered = Name.IsProvided() &&
                         TypeOfHomes.IsProvided() &&
                         BuildActivity.IsAnswered() &&
                         reconfigureExistingValid &&
                         Tranches.IsAnswered() &&
                         _homesToDeliver.Count != 0 &&
                         DeliveryPhaseMilestones.IsAnswered();

        if (Organisation.IsUnregisteredBody)
        {
            isAnswered = isAnswered && IsAdditionalPaymentRequested != null && IsAdditionalPaymentRequested.IsAnswered();
        }

        return isAnswered;
    }

    private void MarkAsNotCompleted()
    {
        Status = _modificationTracker.Change(Status, SectionStatus.InProgress);
    }

    private void ResetTypeOfHomesDependencies(TypeOfHomes? newTypeOfHomes)
    {
        BuildActivity = BuildActivity.WithClearedAnswer(newTypeOfHomes.GetValueOrFirstValue());
        ReconfiguringExisting = null;
    }

    private void ResetBuildActivityDependencies(BuildActivity newBuildActivity)
    {
        var milestones = new DeliveryPhaseMilestones(
            _onlyCompletionMilestonePolicy.Validate(Organisation.IsUnregisteredBody, newBuildActivity),
            DeliveryPhaseMilestones);
        DeliveryPhaseMilestones = _modificationTracker.Change(DeliveryPhaseMilestones, milestones, MarkAsNotCompleted);
    }
}
