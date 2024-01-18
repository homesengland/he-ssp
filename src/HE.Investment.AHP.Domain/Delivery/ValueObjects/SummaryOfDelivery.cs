using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class SummaryOfDelivery : ValueObject
{
    public SummaryOfDelivery(decimal? grantApportioned, decimal? acquisitionMilestone, decimal? starOnSiteMilestone, decimal? completionMilestone)
    {
        GrantApportioned = grantApportioned;
        AcquisitionMilestone = acquisitionMilestone;
        StarOnSiteMilestone = starOnSiteMilestone;
        CompletionMilestone = completionMilestone;
        ValidateValues();
    }

    public decimal? GrantApportioned { get; }

    public decimal? AcquisitionMilestone { get; }

    public decimal? StarOnSiteMilestone { get; }

    public decimal? CompletionMilestone { get; }

    public static SummaryOfDelivery LackOfCalculation => new(null, null, null, null);

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return GrantApportioned;
        yield return AcquisitionMilestone;
        yield return StarOnSiteMilestone;
        yield return CompletionMilestone;
    }

    private void ValidateValues()
    {
        if (GrantApportioned.GetValueOrDefault() == AcquisitionMilestone.GetValueOrDefault() + StarOnSiteMilestone.GetValueOrDefault() + CompletionMilestone.GetValueOrDefault())
        {
            return;
        }

        throw new DomainValidationException("The sum of all milestones should be equal to the grant apportioned.");
    }
}
