using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class DeliveryPhaseMilestones : ValueObject, IQuestion
{
    private readonly OrganisationBasicInfo _organisation;
    private readonly BuildActivity _buildActivity;

    public DeliveryPhaseMilestones(
        OrganisationBasicInfo organisation,
        BuildActivity buildActivity,
        AcquisitionMilestoneDetails? acquisitionMilestone = null,
        StartOnSiteMilestoneDetails? startOnSiteMilestone = null,
        CompletionMilestoneDetails? completionMilestone = null)
    {
        _organisation = organisation;
        _buildActivity = buildActivity;

        ValidatePaymentDates(acquisitionMilestone, startOnSiteMilestone, completionMilestone);
        ValidateDatesForUnregisteredBody(acquisitionMilestone, startOnSiteMilestone);

        AcquisitionMilestone = acquisitionMilestone;
        StartOnSiteMilestone = startOnSiteMilestone;
        CompletionMilestone = completionMilestone;
    }

    public AcquisitionMilestoneDetails? AcquisitionMilestone { get; }

    public StartOnSiteMilestoneDetails? StartOnSiteMilestone { get; }

    public CompletionMilestoneDetails? CompletionMilestone { get; }

    public bool IsOnlyCompletionMilestone => _buildActivity.IsOffTheShelfOrExistingSatisfactory;

    public bool IsAnswered()
    {
        if (_organisation.IsUnregisteredBody || _buildActivity.IsOffTheShelfOrExistingSatisfactory)
        {
            return CompletionMilestone != null && CompletionMilestone.IsAnswered();
        }

        return AcquisitionMilestone != null && AcquisitionMilestone.IsAnswered() &&
               StartOnSiteMilestone != null && StartOnSiteMilestone.IsAnswered() &&
               CompletionMilestone != null && CompletionMilestone.IsAnswered();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return _organisation;
        yield return _buildActivity;
        yield return AcquisitionMilestone;
        yield return StartOnSiteMilestone;
        yield return CompletionMilestone;
    }

    private void ValidatePaymentDates(
        AcquisitionMilestoneDetails? acquisitionMilestoneDetails,
        StartOnSiteMilestoneDetails? startOnSiteMilestoneDetails,
        CompletionMilestoneDetails? completionMilestoneDetails)
    {
        if (!_organisation.IsUnregisteredBody && !_buildActivity.IsOffTheShelfOrExistingSatisfactory)
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
        if ((_organisation.IsUnregisteredBody || _buildActivity.IsOffTheShelfOrExistingSatisfactory) && acquisitionMilestone.IsProvided())
        {
            throw new DomainValidationException(
                "Cannot provide Acquisition Milestone details.");
        }

        if ((_organisation.IsUnregisteredBody || _buildActivity.IsOffTheShelfOrExistingSatisfactory) && startOnSiteMilestone.IsProvided())
        {
            throw new DomainValidationException(
                "Cannot provide Start On Site Milestone details.");
        }
    }
}
