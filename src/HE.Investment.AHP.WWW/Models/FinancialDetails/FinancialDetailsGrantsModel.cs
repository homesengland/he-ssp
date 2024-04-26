namespace HE.Investment.AHP.WWW.Models.FinancialDetails;

public class FinancialDetailsGrantsModel : FinancialDetailsBaseModel
{
    public FinancialDetailsGrantsModel()
    {
    }

    public FinancialDetailsGrantsModel(
        Guid applicationId,
        string applicationName,
        string? countyCouncilGrants,
        string? dhscExtraCareGrants,
        string? localAuthorityGrants,
        string? socialServicesGrants,
        string? healthRelatedGrants,
        string? lotteryGrants,
        string? otherPublicBodiesGrants,
        string? totalReceivedGrands)
        : base(applicationId, applicationName)
    {
        CountyCouncilGrants = countyCouncilGrants;
        DhscExtraCareGrants = dhscExtraCareGrants;
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
