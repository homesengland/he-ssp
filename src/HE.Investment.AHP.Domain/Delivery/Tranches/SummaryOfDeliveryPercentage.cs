using Dawn;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Delivery.Tranches;

public class SummaryOfDeliveryPercentage : ValueObject
{
    public SummaryOfDeliveryPercentage(decimal? acquisitionPercentage, decimal? startOnSitePercentage, decimal? completionPercentage)
    {
        AcquisitionPercentage = acquisitionPercentage;
        StartOnSitePercentage = startOnSitePercentage;
        CompletionPercentage = completionPercentage;
    }

    public static SummaryOfDeliveryPercentage LackOfCalculation => new(null, null, null);

    public decimal? AcquisitionPercentage { get; }

    public decimal? StartOnSitePercentage { get; }

    public decimal? CompletionPercentage { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return StartOnSitePercentage;
        yield return AcquisitionPercentage;
        yield return CompletionPercentage;
    }
}
