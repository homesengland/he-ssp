using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class SummaryOfDelivery : ValueObject
{
    public SummaryOfDelivery(
        decimal? grantApportioned,
        decimal? acquisitionMilestone,
        decimal? startOnSiteMilestone,
        decimal? completionMilestone)
    {
        GrantApportioned = grantApportioned;
        AcquisitionMilestone = acquisitionMilestone;
        StartOnSiteMilestone = startOnSiteMilestone;
        CompletionMilestone = completionMilestone;
        ValidateValues();
    }

    public static SummaryOfDelivery LackOfCalculation => new(null, null, null, null);

    public decimal? GrantApportioned { get; }

    public decimal? AcquisitionMilestone { get; }

    public decimal? StartOnSiteMilestone { get; }

    public decimal? CompletionMilestone { get; }

    public SummaryOfDeliveryPercentage SummaryOfDeliveryPercentage => new(
        AcquisitionMilestone / GrantApportioned,
        StartOnSiteMilestone / GrantApportioned,
        CompletionMilestone / GrantApportioned);

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return GrantApportioned;
        yield return AcquisitionMilestone;
        yield return StartOnSiteMilestone;
        yield return CompletionMilestone;
    }

    private void ValidateValues()
    {
        if (GrantApportioned.GetValueOrDefault() ==
            AcquisitionMilestone.GetValueOrDefault() + StartOnSiteMilestone.GetValueOrDefault() + CompletionMilestone.GetValueOrDefault())
        {
            return;
        }

        throw new DomainValidationException("The sum of all milestones should be equal to the grant apportioned.");
    }
}
