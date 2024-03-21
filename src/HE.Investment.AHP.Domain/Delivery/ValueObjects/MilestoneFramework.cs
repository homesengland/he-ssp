using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class MilestoneFramework : ValueObject
{
    public MilestoneFramework(decimal acquisitionPercentage, decimal startOnSitePercentage, decimal completionPercentage)
    {
        AcquisitionPercentage = ValidatePercentage(acquisitionPercentage, nameof(acquisitionPercentage));
        StartOnSitePercentage = ValidatePercentage(startOnSitePercentage, nameof(startOnSitePercentage));
        CompletionPercentage = ValidatePercentage(completionPercentage, nameof(completionPercentage));
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

    private static decimal ValidatePercentage(decimal value, string parameterName)
    {
        if (value is < 0 or > 1)
        {
            throw new ArgumentException("Percentage value must be between 0 and 1.", parameterName);
        }

        return value;
    }
}
