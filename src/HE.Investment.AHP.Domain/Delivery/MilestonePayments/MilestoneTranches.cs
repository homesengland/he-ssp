using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Delivery.MilestonePayments;

public class MilestoneTranches : ValueObject
{
    public MilestoneTranches(decimal? acquisition, decimal? startOnSite, decimal? completion)
    {
        Acquisition = acquisition;
        StartOnSite = startOnSite;
        Completion = completion;
    }

    public static MilestoneTranches NotProvided => new(null, null, null);

    public decimal? Acquisition { get; }

    public decimal? StartOnSite { get; }

    public decimal? Completion { get; }

    public MilestoneTranches WithAcquisition(decimal? acquisition)
    {
        return new MilestoneTranches(acquisition, StartOnSite, Completion);
    }

    public MilestoneTranches WithCompletion(decimal? completion)
    {
        return new MilestoneTranches(Acquisition, StartOnSite, completion);
    }

    public MilestoneTranches WithStartOnSite(decimal? startOnSite)
    {
        return new MilestoneTranches(Acquisition, startOnSite, Completion);
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Acquisition;
        yield return StartOnSite;
        yield return Completion;
    }


}
