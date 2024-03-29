using System.Globalization;
using HE.Investments.Common.Extensions;
using HE.Investments.Loans.BusinessLogic.Projects.Enums;

namespace HE.Investments.Loans.BusinessLogic.Projects.Repositories.Mappers;
internal static class SourceOfValuationMapper
{
    public static SourceOfValuation? FromString(string valuationSource)
    {
        if (valuationSource.IsNotProvided())
        {
            return null;
        }

        return valuationSource.ToLower(CultureInfo.InvariantCulture) switch
        {
            "customerestimate" => SourceOfValuation.SelfEstimate,
            "estateagentestimate" => SourceOfValuation.EstateAgentEstimate,
            "ricsredbookvaluation" => SourceOfValuation.RicsRedBookValuation,
            _ => null,
        };
    }

    public static string ToString(SourceOfValuation valuationSource)
    {
        return valuationSource switch
        {
            SourceOfValuation.SelfEstimate => "customerEstimate",
            SourceOfValuation.EstateAgentEstimate => "estateAgentEstimate",
            SourceOfValuation.RicsRedBookValuation => "ricsRedBookValuation",
            _ => throw new ArgumentException($"Cannot convert {valuationSource} to SourceOfValuation."),
        };
    }

    public static string ToCrmString(SourceOfValuation valuationSource)
    {
        return valuationSource switch
        {
            SourceOfValuation.SelfEstimate => "customerestimate",
            SourceOfValuation.EstateAgentEstimate => "estateagentestimate",
            SourceOfValuation.RicsRedBookValuation => "ricsredbookvaluation",
            _ => throw new ArgumentException($"Cannot convert {valuationSource} to SourceOfValuation."),
        };
    }
}
