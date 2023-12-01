namespace HE.Investment.AHP.WWW.Models.FinancialDetails;
public class FinancialDetailsLandStatusModel : FinancialDetailsBaseModel
{
    public FinancialDetailsLandStatusModel()
    {
    }

    public FinancialDetailsLandStatusModel(Guid applicationId, string applicationName, string purchasePrice, bool isFinal)
        : base(applicationId, applicationName)
    {
        PurchasePrice = purchasePrice;
        IsFinal = isFinal;
    }

    public string PurchasePrice { get; set; }

    public bool IsFinal { get; set; }
}
