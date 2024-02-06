using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Delivery.Tranches;

public class SummaryOfDelivery : ValueObject
{
    public SummaryOfDelivery(
        decimal? grantApportioned,
        decimal? acquisitionMilestone,
        decimal? startOnSiteMilestone,
        decimal? completionMilestone,
        bool validateValues = true)
    {
        GrantApportioned = grantApportioned;
        AcquisitionMilestone = acquisitionMilestone;
        StartOnSiteMilestone = startOnSiteMilestone;
        CompletionMilestone = completionMilestone;
        if (validateValues)
        {
            ValidateValues();
        }
    }

    public static SummaryOfDelivery LackOfCalculation => new(null, null, null, null);

    public decimal? GrantApportioned { get; }

    public decimal? AcquisitionMilestone { get; }

    public decimal? StartOnSiteMilestone { get; }

    public decimal? CompletionMilestone { get; }

    public bool IsSumUpTo => GrantApportioned.GetValueOrDefault() ==
                             AcquisitionMilestone.GetValueOrDefault() + StartOnSiteMilestone.GetValueOrDefault() + CompletionMilestone.GetValueOrDefault();

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
        if (IsSumUpTo)
        {
            return;
        }

        throw new DomainValidationException("The sum of all milestones should be equal to the grant apportioned.");
    }
}
