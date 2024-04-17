using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Delivery.Tranches;

public class MilestonesTranches : ValueObject
{
    public MilestonesTranches(
        decimal? sumOfGrantApportioned,
        decimal? acquisitionMilestone,
        decimal? startOnSiteMilestone,
        decimal? completionMilestone)
    {
        SumOfGrantApportioned = sumOfGrantApportioned;
        AcquisitionMilestone = acquisitionMilestone;
        StartOnSiteMilestone = startOnSiteMilestone;
        CompletionMilestone = completionMilestone;
    }

    public static MilestonesTranches LackOfCalculation => new(null, null, null, null);

    public decimal? SumOfGrantApportioned { get; }

    public decimal? AcquisitionMilestone { get; }

    public decimal? StartOnSiteMilestone { get; }

    public decimal? CompletionMilestone { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return SumOfGrantApportioned;
        yield return AcquisitionMilestone;
        yield return StartOnSiteMilestone;
        yield return CompletionMilestone;
    }
}
