namespace HE.Investment.AHP.Domain.Delivery.Tranches;

public sealed record MilestonesCalculatedTranches(
    decimal? SumOfGrantApportioned,
    decimal? AcquisitionMilestone,
    decimal? StartOnSiteMilestone,
    decimal? CompletionMilestone)
{
    public static MilestonesCalculatedTranches NotCalculated => new(null, null, null, null);
}
