using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

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

    public bool IsAnswered()
    {
        return SumOfGrantApportioned.IsProvided()
               && AcquisitionMilestone.IsProvided()
               && StartOnSiteMilestone.IsProvided()
               && CompletionMilestone.IsProvided();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return SumOfGrantApportioned;
        yield return AcquisitionMilestone;
        yield return StartOnSiteMilestone;
        yield return CompletionMilestone;
    }
}
