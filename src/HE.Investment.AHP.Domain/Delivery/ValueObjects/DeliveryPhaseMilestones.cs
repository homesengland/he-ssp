using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class DeliveryPhaseMilestones : ValueObject, IQuestion
{
    public DeliveryPhaseMilestones(
        OrganisationBasicInfo organisation,
        AcquisitionMilestoneDetails? acquisitionMilestone = null,
        StartOnSiteMilestoneDetails? startOnSiteMilestone = null,
        CompletionMilestoneDetails? completionMilestone = null)
    {
        Organisation = organisation;

        ValidatePaymentDates(acquisitionMilestone, startOnSiteMilestone, completionMilestone);
        ValidateDatesForUnregisteredBody(acquisitionMilestone, startOnSiteMilestone);

        AcquisitionMilestone = acquisitionMilestone;
        StartOnSiteMilestone = startOnSiteMilestone;
        CompletionMilestone = completionMilestone;
    }

    public OrganisationBasicInfo Organisation { get; }

    public AcquisitionMilestoneDetails? AcquisitionMilestone { get; }

    public StartOnSiteMilestoneDetails? StartOnSiteMilestone { get; }

    public CompletionMilestoneDetails? CompletionMilestone { get; }

    public bool IsAnswered()
    {
        if (Organisation.IsUnregisteredBody)
        {
            return CompletionMilestone != null && CompletionMilestone.IsAnswered();
        }

        return AcquisitionMilestone != null && AcquisitionMilestone.IsAnswered() &&
               StartOnSiteMilestone != null && StartOnSiteMilestone.IsAnswered() &&
               CompletionMilestone != null && CompletionMilestone.IsAnswered();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return AcquisitionMilestone;
        yield return StartOnSiteMilestone;
        yield return CompletionMilestone;
    }

    private void ValidatePaymentDates(
        AcquisitionMilestoneDetails? acquisitionMilestoneDetails,
        StartOnSiteMilestoneDetails? startOnSiteMilestoneDetails,
        CompletionMilestoneDetails? completionMilestoneDetails)
    {
        if (!Organisation.IsUnregisteredBody)
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

    private void ValidateDatesForUnregisteredBody(
        AcquisitionMilestoneDetails? acquisitionMilestone,
        StartOnSiteMilestoneDetails? startOnSiteMilestone)
    {
        if (Organisation.IsUnregisteredBody && acquisitionMilestone.IsProvided())
        {
            throw new DomainValidationException(
                "Cannot provide Acquisition Milestone details for Unregistered Body Partner.");
        }

        if (Organisation.IsUnregisteredBody && startOnSiteMilestone.IsProvided())
        {
            throw new DomainValidationException(
                "Cannot provide Start On Site Milestone details for Unregistered Body Partner.");
        }
    }
}
