using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

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

    public void CheckComplete()
    {
        ValidatePaymentDates(AcquisitionMilestone, StartOnSiteMilestone, CompletionMilestone);
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return IsOnlyCompletionMilestone;
        yield return AcquisitionMilestone;
        yield return StartOnSiteMilestone;
        yield return CompletionMilestone;
    }

    private void ValidatePaymentDates(
        AcquisitionMilestoneDetails? acquisitionMilestoneDetails,
        StartOnSiteMilestoneDetails? startOnSiteMilestoneDetails,
        CompletionMilestoneDetails? completionMilestoneDetails)
    {
        if (!IsOnlyCompletionMilestone)
        {
            ValidateMilestonePaymentDate(
                acquisitionMilestoneDetails?.PaymentDate,
                startOnSiteMilestoneDetails?.PaymentDate,
                "The start on site milestone claim date cannot be before the acquisition milestone claim date");

            ValidateMilestonePaymentDate(
                startOnSiteMilestoneDetails?.PaymentDate,
                completionMilestoneDetails?.PaymentDate,
                "The completion milestone claim date cannot be before the start on site milestone claim date");

            ValidateMilestonePaymentDate(
                acquisitionMilestoneDetails?.PaymentDate,
                completionMilestoneDetails?.PaymentDate,
                "The completion milestone claim date cannot be before the acquisition milestone claim date");
        }
    }

    private void ValidateMilestonePaymentDate(
        MilestonePaymentDate? firstMilestonePaymentDate,
        MilestonePaymentDate? secondMilestonePaymentDate,
        string errorMessage)
    {
        if (firstMilestonePaymentDate != null &&
            secondMilestonePaymentDate != null &&
            firstMilestonePaymentDate.Value > secondMilestonePaymentDate.Value)
        {
            throw new DomainValidationException("ClaimMilestonePaymentAt", errorMessage);
        }
    }

    private void ValidateDatesForOnlyCompletionMilestone(
        AcquisitionMilestoneDetails? acquisitionMilestone,
        StartOnSiteMilestoneDetails? startOnSiteMilestone)
    {
        if (IsOnlyCompletionMilestone && acquisitionMilestone.IsProvided())
        {
            throw new DomainValidationException(
                "Cannot provide Acquisition Milestone details.");
        }

        if (IsOnlyCompletionMilestone && startOnSiteMilestone.IsProvided())
        {
            throw new DomainValidationException(
                "Cannot provide Start On Site Milestone details.");
        }
    }
}
