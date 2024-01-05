namespace HE.Investments.AHP.IntegrationTests.FillApplication.Data;

public class FinancialDetailsData
{
    public decimal? LandStatus { get; private set; }

    public bool IsPublicLand { get; private set; }

    public decimal? PublicLandValue { get; private set; }

    public decimal? ExpectedWorksCosts { get; private set; }

    public decimal? ExpectedOnCosts { get; private set; }

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
}
