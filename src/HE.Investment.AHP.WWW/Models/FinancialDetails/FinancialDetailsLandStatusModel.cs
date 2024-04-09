namespace HE.Investment.AHP.WWW.Models.FinancialDetails;
public class FinancialDetailsLandStatusModel : FinancialDetailsBaseModel
{
    public FinancialDetailsLandStatusModel()
    {
    }

    public FinancialDetailsLandStatusModel(Guid applicationId, string applicationName, string? purchasePrice, bool isFullUnconditionalOption)
        : base(applicationId, applicationName)
    {
        PurchasePrice = purchasePrice;
        IsFullUnconditionalOption = isFullUnconditionalOption;
    }

    public string? PurchasePrice { get; set; }

    public bool IsFullUnconditionalOption { get; set; }
}
