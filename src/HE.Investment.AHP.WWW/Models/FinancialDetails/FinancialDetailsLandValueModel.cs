namespace HE.Investment.AHP.WWW.Models.FinancialDetails;

public class FinancialDetailsLandValueModel : FinancialDetailsBaseModel
{
    public FinancialDetailsLandValueModel()
    {
    }

    public FinancialDetailsLandValueModel(string applicationId, string applicationName, string? landValue, bool? isOnPublicLand)
        : base(applicationId, applicationName)
    {
        LandValue = landValue;
        IsOnPublicLand = isOnPublicLand;
    }

    public string? LandValue { get; set; }

    public bool? IsOnPublicLand { get; set; }
}
