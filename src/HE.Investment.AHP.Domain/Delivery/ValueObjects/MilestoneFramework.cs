using Dawn;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class MilestoneFramework : ValueObject
{
    public MilestoneFramework(decimal acquisitionPercentage, decimal startOnSitePercentage, decimal completionPercentage)
    {
        AcquisitionPercentage = Guard.Argument(acquisitionPercentage, nameof(acquisitionPercentage)).InRange(0, 1);
        StartOnSitePercentage = Guard.Argument(startOnSitePercentage, nameof(startOnSitePercentage)).InRange(0, 1);
        CompletionPercentage = Guard.Argument(completionPercentage, nameof(completionPercentage)).InRange(0, 1);
    }

    public static MilestoneFramework Default => new(0.5m, 0.4m, 0.1m);

    public decimal AcquisitionPercentage { get; }

    public decimal StartOnSitePercentage { get; }

    public decimal CompletionPercentage { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return StartOnSitePercentage;
        yield return AcquisitionPercentage;
        yield return CompletionPercentage;
    }
}
