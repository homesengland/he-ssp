namespace HE.Investment.AHP.WWW.Models.FinancialDetails;

public class FinancialDetailsLandStatusModel : FinancialDetailsBaseModel
{
    public FinancialDetailsLandStatusModel()
    {
    }

    public FinancialDetailsLandStatusModel(string applicationId, string applicationName, string? purchasePrice, string? landAcquisitionStatus, bool hasFullLandOwnership)
        : base(applicationId, applicationName)
    {
        PurchasePrice = purchasePrice;
        LandAcquisitionStatus = landAcquisitionStatus;
        HasFullLandOwnership = hasFullLandOwnership;
    }

    public string? PurchasePrice { get; set; }

    public bool HasFullLandOwnership { get; set; }

    public string? LandAcquisitionStatus { get; set; }
}
