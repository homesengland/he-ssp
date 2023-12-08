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
        string? otherPublicBodiesGrants,
        string totalReceivedGrands)
        : base(applicationId, applicationName)
    {
        CountyCouncilGrants = countyCouncilGrants;
        DhscExtraCareGrants = dHSCExtraCreGrants;
        LocalAuthorityGrants = localAuthorityGrants;
        SocialServicesGrants = socialServicesGrants;
        HealthRelatedGrants = healthRelatedGrants;
        LotteryGrants = lotteryGrants;
        OtherPublicBodiesGrants = otherPublicBodiesGrants;
        TotalGrants = totalReceivedGrands;
    }

    public string? CountyCouncilGrants { get; set; }

    public string? DhscExtraCareGrants { get; set; }

    public string? LocalAuthorityGrants { get; set; }

    public string? SocialServicesGrants { get; set; }

    public string? HealthRelatedGrants { get; set; }

    public string? LotteryGrants { get; set; }

    public string? OtherPublicBodiesGrants { get; set; }

    public string? TotalGrants { get; set; }
}
