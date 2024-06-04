using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Utils;
using HE.Investments.Programme.Contract;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class DeliveryPhaseMilestones : ValueObject, IQuestion
{
    public DeliveryPhaseMilestones(
        bool isOnlyCompletionMilestone,
        AcquisitionMilestoneDetails? acquisitionMilestone = null,
        StartOnSiteMilestoneDetails? startOnSiteMilestone = null,
        CompletionMilestoneDetails? completionMilestone = null)
    {
        IsOnlyCompletionMilestone = isOnlyCompletionMilestone;
        AcquisitionMilestone = acquisitionMilestone;
        StartOnSiteMilestone = startOnSiteMilestone;
        CompletionMilestone = completionMilestone;

        ValidateDatesForOnlyCompletionMilestone(acquisitionMilestone, startOnSiteMilestone);
    }

    public DeliveryPhaseMilestones(
        bool isOnlyCompletionMilestone,
        DeliveryPhaseMilestones milestones)
    {
        if (!isOnlyCompletionMilestone)
        {
            AcquisitionMilestone = milestones.AcquisitionMilestone;
            StartOnSiteMilestone = milestones.StartOnSiteMilestone;
        }

        IsOnlyCompletionMilestone = isOnlyCompletionMilestone;
        CompletionMilestone = milestones.CompletionMilestone;

        ValidateDatesForOnlyCompletionMilestone(AcquisitionMilestone, StartOnSiteMilestone);
    }

    public AcquisitionMilestoneDetails? AcquisitionMilestone { get; }

    public StartOnSiteMilestoneDetails? StartOnSiteMilestone { get; }

    public CompletionMilestoneDetails? CompletionMilestone { get; }

    public bool IsOnlyCompletionMilestone { get; }

    public bool IsAnswered()
    {
        if (IsOnlyCompletionMilestone)
        {
            return CompletionMilestone != null && CompletionMilestone.IsAnswered();
        }

        return AcquisitionMilestone != null && AcquisitionMilestone.IsAnswered() &&
               StartOnSiteMilestone != null && StartOnSiteMilestone.IsAnswered() &&
               CompletionMilestone != null && CompletionMilestone.IsAnswered();
    }

    public void ValidateMilestoneDates(Programme programme, IDateTimeProvider dateTimeProvider)
    {
        MilestonesShouldFollowEachOther();
        MilestoneClaimsShouldFollowEachOther();
        MilestoneClaimsShouldBeInTheFuture(dateTimeProvider);
        MilestoneClaimsShouldBeWithinProgrammeFundingDates(programme);
        StartOnSiteMilestoneShouldBeWithinProgrammeStartOnSiteDates(programme);
        CompletionMilestoneShouldBeWithinProgrammeCompletionDates(programme);
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return IsOnlyCompletionMilestone;
        yield return AcquisitionMilestone;
        yield return StartOnSiteMilestone;
        yield return CompletionMilestone;
    }

    private void ValidateDatesForOnlyCompletionMilestone(
        AcquisitionMilestoneDetails? acquisitionMilestone,
        StartOnSiteMilestoneDetails? startOnSiteMilestone)
    {
        if (IsOnlyCompletionMilestone && acquisitionMilestone.IsProvided())
        {
            throw new DomainValidationException(
                "Cannot provide Acquisition Milestone details");
        }

        if (IsOnlyCompletionMilestone && startOnSiteMilestone.IsProvided())
        {
            throw new DomainValidationException(
                "Cannot provide Start On Site Milestone details");
        }
    }

    private void MilestonesShouldFollowEachOther()
    {
        if (StartOnSiteMilestone?.MilestoneDate?.IsBefore(AcquisitionMilestone?.MilestoneDate) == true
            || CompletionMilestone?.MilestoneDate?.IsBefore(StartOnSiteMilestone?.MilestoneDate) == true)
        {
            throw new DomainValidationException("Milestone dates should follow each other");
        }
    }

    private void MilestoneClaimsShouldFollowEachOther()
    {
        if (StartOnSiteMilestone?.PaymentDate?.IsBefore(AcquisitionMilestone?.PaymentDate) == true
            || CompletionMilestone?.PaymentDate?.IsBefore(StartOnSiteMilestone?.PaymentDate) == true)
        {
            throw new DomainValidationException("Milestone claim dates should follow each other");
        }
    }

    private void MilestoneClaimsShouldBeInTheFuture(IDateTimeProvider dateTimeProvider)
    {
        var today = dateTimeProvider.Now.Date;
        if (AcquisitionMilestone?.PaymentDate?.IsBefore(today) == true
            || StartOnSiteMilestone?.PaymentDate?.IsBefore(today) == true
            || CompletionMilestone?.PaymentDate?.IsBefore(today) == true)
        {
            throw new DomainValidationException("Milestone claim dates should be in the future");
        }
    }

    private void MilestoneClaimsShouldBeWithinProgrammeFundingDates(Programme programme)
    {
        if (AcquisitionMilestone?.PaymentDate?.IsBefore(programme.FundingDates.StartDate) == true
            || CompletionMilestone?.PaymentDate?.IsBefore(programme.FundingDates.StartDate) == true
            || CompletionMilestone?.PaymentDate?.IsAfter(programme.FundingDates.EndDate) == true)
        {
            throw new DomainValidationException("Milestone claim dates should be withing Programme Funding dates");
        }
    }

    private void StartOnSiteMilestoneShouldBeWithinProgrammeStartOnSiteDates(Programme programme)
    {
        if (StartOnSiteMilestone?.MilestoneDate?.IsBefore(programme.StartOnSiteDates.StartDate) == true
            || StartOnSiteMilestone?.MilestoneDate?.IsAfter(programme.StartOnSiteDates.EndDate) == true)
        {
            throw new DomainValidationException("Start on Site milestone dates should be within programme Start on Site dates");
        }
    }

    private void CompletionMilestoneShouldBeWithinProgrammeCompletionDates(Programme programme)
    {
        if (CompletionMilestone?.MilestoneDate?.IsBefore(programme.CompletionDates.StartDate) == true
            || CompletionMilestone?.MilestoneDate?.IsAfter(programme.CompletionDates.EndDate) == true)
        {
            throw new DomainValidationException("Completion milestone dates should be within programme Completion dates");
        }
    }
}
