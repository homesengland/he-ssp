using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.WWW.Models.FinancialDetails;
public class FinancialDetailsGrantsModel : FinancialDetailsBaseModel
{
    public FinancialDetailsGrantsModel()
        : base()
    {
    }

    public FinancialDetailsGrantsModel(
        Guid applicationId,
        string applicationName,
        string? countyCouncilGrants,
        string? dHSCExtraCreGrants,
        string? localAuthorityGrants,
        string? socialServicesGrants,
        string? healthRelatedGrants,
        string? lotteryGrants,
        string? otherPublicBodiesGrants)
        : base(applicationId, applicationName)
    {
        CountyCouncilGrants = countyCouncilGrants;
        DHSCExtraCareGrants = dHSCExtraCreGrants;
        LocalAuthorityGrants = localAuthorityGrants;
        SocialServicesGrants = socialServicesGrants;
        HealthRelatedGrants = healthRelatedGrants;
        LotteryGrants = lotteryGrants;
        OtherPublicBodiesGrants = otherPublicBodiesGrants;
    }

    public string? CountyCouncilGrants { get; set; }

    public string? DHSCExtraCareGrants { get; set; }

    public string? LocalAuthorityGrants { get; set; }

    public string? SocialServicesGrants { get; set; }

    public string? HealthRelatedGrants { get; set; }

    public string? LotteryGrants { get; set; }

    public string? OtherPublicBodiesGrants { get; set; }

    public string TotalGrants
    {
        get
        {
            decimal result = 0;
            result += CountyCouncilGrants.TryParseNullableDecimal() ?? 0;
            result += DHSCExtraCareGrants.TryParseNullableDecimal() ?? 0;
            result += LocalAuthorityGrants.TryParseNullableDecimal() ?? 0;
            result += SocialServicesGrants.TryParseNullableDecimal() ?? 0;
            result += HealthRelatedGrants.TryParseNullableDecimal() ?? 0;
            result += LotteryGrants.TryParseNullableDecimal() ?? 0;
            result += OtherPublicBodiesGrants.TryParseNullableDecimal() ?? 0;

            return ((decimal?)result).ToWholeNumberString() ?? "0";
        }
    }
}
