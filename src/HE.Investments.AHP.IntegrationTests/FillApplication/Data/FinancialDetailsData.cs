namespace HE.Investments.AHP.IntegrationTests.FillApplication.Data;

public class FinancialDetailsData
{
    public decimal LandStatus { get; private set; }

    public bool IsPublicLand { get; private set; }

    public decimal PublicLandValue { get; private set; }

    public decimal ExpectedWorksCosts { get; private set; }

    public decimal ExpectedOnCosts { get; private set; }

    public decimal ExpectedContributionsRentalIncomeBorrowing { get; private set; }

    public decimal ExpectedContributionsSaleOfHomesOnThisScheme { get; private set; }

    public decimal ExpectedContributionsSaleOfHomesOnOtherSchemes { get; private set; }

    public decimal ExpectedContributionsOwnResources { get; private set; }

    public decimal ExpectedContributionsRcgfContribution { get; private set; }

    public decimal ExpectedContributionsOtherCapitalSources { get; private set; }

    public decimal ExpectedContributionsHomesTransferValue { get; private set; }

    public decimal CountyCouncilGrants { get; private set; }

    public decimal DhscExtraCareGrants { get; private set; }

    public decimal LocalAuthorityGrants { get; private set; }

    public decimal SocialServicesGrants { get; private set; }

    public decimal HealthRelatedGrants { get; private set; }

    public decimal LotteryGrants { get; private set; }

    public decimal OtherPublicBodiesGrants { get; private set; }

    public decimal TotalExpectedContributions => ExpectedContributionsRentalIncomeBorrowing +
                                                  ExpectedContributionsSaleOfHomesOnThisScheme +
                                                  ExpectedContributionsSaleOfHomesOnOtherSchemes +
                                                  ExpectedContributionsOwnResources +
                                                  ExpectedContributionsRcgfContribution +
                                                  ExpectedContributionsOtherCapitalSources +
                                                  ExpectedContributionsHomesTransferValue;

    public decimal TotalGrants => CountyCouncilGrants +
                                   DhscExtraCareGrants +
                                   LocalAuthorityGrants +
                                   SocialServicesGrants +
                                   HealthRelatedGrants +
                                   LotteryGrants +
                                   OtherPublicBodiesGrants;

    public decimal TotalContributions => TotalGrants + TotalExpectedContributions;

    public void GenerateLandStatus()
    {
        LandStatus = DateTime.UtcNow.Millisecond;
    }

    public void GenerateLandValue()
    {
        IsPublicLand = true;
        PublicLandValue = DateTime.UtcNow.Millisecond;
    }

    public void GenerateOtherApplicationCosts()
    {
        ExpectedWorksCosts = DateTime.UtcNow.Millisecond;
        ExpectedOnCosts = DateTime.UtcNow.Second;
    }

    public void GenerateExpectedContributions()
    {
        ExpectedContributionsRentalIncomeBorrowing = DateTime.UtcNow.Millisecond;
        ExpectedContributionsSaleOfHomesOnThisScheme = ExpectedContributionsRentalIncomeBorrowing + 1;
        ExpectedContributionsSaleOfHomesOnOtherSchemes = ExpectedContributionsRentalIncomeBorrowing + 2;
        ExpectedContributionsOwnResources = ExpectedContributionsRentalIncomeBorrowing + 3;
        ExpectedContributionsRcgfContribution = ExpectedContributionsRentalIncomeBorrowing + 4;
        ExpectedContributionsOtherCapitalSources = ExpectedContributionsRentalIncomeBorrowing + 5;
        ExpectedContributionsHomesTransferValue = ExpectedContributionsRentalIncomeBorrowing + 6;
    }

    public void GenerateGrants()
    {
        CountyCouncilGrants = DateTime.UtcNow.Millisecond;
        DhscExtraCareGrants = CountyCouncilGrants + 1;
        LocalAuthorityGrants = CountyCouncilGrants + 2;
        SocialServicesGrants = CountyCouncilGrants + 3;
        HealthRelatedGrants = CountyCouncilGrants + 4;
        LotteryGrants = CountyCouncilGrants + 5;
        OtherPublicBodiesGrants = CountyCouncilGrants + 6;
    }
}
