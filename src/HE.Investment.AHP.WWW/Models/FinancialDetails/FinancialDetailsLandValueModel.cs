using HE.Investments.Common.Contract.Enum;

namespace HE.Investment.AHP.WWW.Models.FinancialDetails;
public class FinancialDetailsLandValueModel : FinancialDetailsBaseModel
{
    public FinancialDetailsLandValueModel()
    {
    }

    public FinancialDetailsLandValueModel(Guid applicationId, string applicationName, string? landValue, YesNoType isOnPublicLand)
        : base(applicationId, applicationName)
    {
        LandValue = landValue;
        IsOnPublicLand = isOnPublicLand;
    }

    public string? LandValue { get; set; }

    public YesNoType IsOnPublicLand { get; set; }
}
