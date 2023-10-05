using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.Projects.Enums;
using HE.InvestmentLoans.Common.Extensions;

namespace HE.InvestmentLoans.BusinessLogic.Projects.Repositories.Mappers;
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
            "selfestimate" => SourceOfValuation.SelfEstimate,
            "estateagentestimate" => SourceOfValuation.EstateAgentEstimate,
            "ricsredbookvaluation" => SourceOfValuation.RicsRedBookValuation,
            _ => null,
        };
    }

    public static string ToString(SourceOfValuation valuationSource)
    {
        return valuationSource switch
        {
            SourceOfValuation.SelfEstimate => "selfEstimate",
            SourceOfValuation.EstateAgentEstimate => "estateAgentEstimate",
            SourceOfValuation.RicsRedBookValuation => "ricsRedBookValuation",
            _ => throw new ArgumentException($"Cannot convert {valuationSource} to SourceOfValuation."),
        };
    }

    public static string ToCrmString(SourceOfValuation valuationSource)
    {
        return valuationSource switch
        {
            SourceOfValuation.SelfEstimate => "selfestimate",
            SourceOfValuation.EstateAgentEstimate => "estateagentestimate",
            SourceOfValuation.RicsRedBookValuation => "ricsredbookvaluation",
            _ => throw new ArgumentException($"Cannot convert {valuationSource} to SourceOfValuation."),
        };
    }
}
