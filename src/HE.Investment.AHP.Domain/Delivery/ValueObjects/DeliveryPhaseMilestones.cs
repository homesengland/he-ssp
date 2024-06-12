using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
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
        var result = OperationResult.New();
        result.Aggregate(() => MilestonesShouldFollowEachOther());
        result.Aggregate(() => MilestoneClaimsShouldBeWithinProgrammeFundingDates(programme));
        result.Aggregate(() => MilestoneClaimsShouldFollowEachOther());
        result.Aggregate(() => MilestonesShouldBeBeforeMilestoneClaims());
        result.Aggregate(() => MilestoneClaimsShouldBeInTheFuture(dateTimeProvider));
        result.Aggregate(() => StartOnSiteAndCompletionMilestonesShouldBeWithinProgrammeDates(programme));
        result.CheckErrors();
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
            throw new DomainValidationException("Cannot provide Acquisition Milestone details");
        }

        if (IsOnlyCompletionMilestone && startOnSiteMilestone.IsProvided())
        {
            throw new DomainValidationException("Cannot provide Start On Site Milestone details");
        }
    }

    private void MilestonesShouldFollowEachOther()
    {
        if (StartOnSiteMilestone?.MilestoneDate?.IsBefore(AcquisitionMilestone?.MilestoneDate) == true)
        {
            throw new DomainValidationException("Milestones", "The acquisition date must be before, or the same as, the start on site date");
        }

        if (CompletionMilestone?.MilestoneDate?.IsBefore(StartOnSiteMilestone?.MilestoneDate) == true)
        {
            throw new DomainValidationException("Milestones", "The start on site date must be before, or the same as, the completion date");
        }
    }

    private void MilestoneClaimsShouldFollowEachOther()
    {
        if (StartOnSiteMilestone?.PaymentDate?.IsBefore(AcquisitionMilestone?.PaymentDate) == true)
        {
            throw new DomainValidationException("Milestones", "The forecast acquisition claim date must be before, or the same as, the forecast start on site claim date");
        }

        if (CompletionMilestone?.PaymentDate?.IsBefore(StartOnSiteMilestone?.PaymentDate) == true)
        {
            throw new DomainValidationException("Milestones", "The forecast start on site claim date must be before, or the same as, the forecast completion claim date");
        }
    }

    private void MilestonesShouldBeBeforeMilestoneClaims()
    {
        if (AcquisitionMilestone?.PaymentDate?.IsBefore(AcquisitionMilestone?.MilestoneDate) == true)
        {
            throw new DomainValidationException("Milestones", "The acquisition date must be before, or the same as, the forecast acquisition claim date");
        }

        if (StartOnSiteMilestone?.PaymentDate?.IsBefore(StartOnSiteMilestone?.MilestoneDate) == true)
        {
            throw new DomainValidationException("Milestones", "The start on site date must be before, or the same as, the forecast start on site claim date");
        }

        if (CompletionMilestone?.PaymentDate?.IsBefore(CompletionMilestone?.MilestoneDate) == true)
        {
            throw new DomainValidationException("Milestones", "The completion date must be before, or the same as, the forecast completion claim date");
        }
    }

    private void MilestoneClaimsShouldBeInTheFuture(IDateTimeProvider dateTimeProvider)
    {
        var today = dateTimeProvider.Now.Date;
        if (AcquisitionMilestone?.PaymentDate?.IsBefore(today) == true)
        {
            throw new DomainValidationException("Milestones", "The forecast acquisition claim date must be today or in the future");
        }

        if (StartOnSiteMilestone?.PaymentDate?.IsBefore(today) == true)
        {
            throw new DomainValidationException("Milestones", "The forecast start on site claim date must be today or in the future");
        }

        if (CompletionMilestone?.PaymentDate?.IsBefore(today) == true)
        {
            throw new DomainValidationException("Milestones", "The forecast completion claim date must be today or in the future");
        }
    }

    private void MilestoneClaimsShouldBeWithinProgrammeFundingDates(Programme programme)
    {
        if (AcquisitionMilestone?.PaymentDate?.IsBefore(programme.FundingDates.StartDate) == true
            || StartOnSiteMilestone?.PaymentDate?.IsBefore(programme.FundingDates.StartDate) == true
            || CompletionMilestone?.PaymentDate?.IsBefore(programme.FundingDates.StartDate) == true
            || AcquisitionMilestone?.PaymentDate?.IsAfter(programme.FundingDates.EndDate) == true
            || StartOnSiteMilestone?.PaymentDate?.IsAfter(programme.FundingDates.EndDate) == true
            || CompletionMilestone?.PaymentDate?.IsAfter(programme.FundingDates.EndDate) == true)
        {
            throw new DomainValidationException("Milestones", "Dates fall outside of the programme requirements. Check your dates against the published funding requirements");
        }
    }

    private void StartOnSiteAndCompletionMilestonesShouldBeWithinProgrammeDates(Programme programme)
    {
        if (StartOnSiteMilestone?.MilestoneDate?.IsBefore(programme.StartOnSiteDates.StartDate) == true
            || StartOnSiteMilestone?.MilestoneDate?.IsAfter(programme.StartOnSiteDates.EndDate) == true
            || CompletionMilestone?.MilestoneDate?.IsBefore(programme.CompletionDates.StartDate) == true
            || CompletionMilestone?.MilestoneDate?.IsAfter(programme.CompletionDates.EndDate) == true)
        {
            throw new DomainValidationException("Milestones", "Dates fall outside of the programme requirements. Check your dates against the published funding requirements");
        }
    }
}
