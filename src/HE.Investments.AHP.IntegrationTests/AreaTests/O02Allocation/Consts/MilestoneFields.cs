using HE.Investments.Common.Extensions;

namespace HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation.Consts;

public static class MilestoneFields
{
    public static string AmountOfGrantApportioned(string? prefix = null) => $"{GetPrefix(prefix)}Amount of grant apportioned to this milestone";

    public static string PercentageOfGrantApportioned(string? prefix = null) => $"{GetPrefix(prefix)}Percentage of grant apportioned to this milestone";

    public static string ForecastClaimDate(string? prefix = null) => $"{GetPrefix(prefix)}Forecast claim date";

    private static string GetPrefix(string? prefix)
    {
        return prefix.IsProvided() ? $"{prefix} - " : string.Empty;
    }
}
