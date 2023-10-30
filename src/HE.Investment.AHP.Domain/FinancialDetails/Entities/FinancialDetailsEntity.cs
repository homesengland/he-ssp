using HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;

namespace HE.Investment.AHP.Domain.FinancialDetails.Entities;

public class FinancialDetailsEntity
{
    public FinancialDetailsEntity(FinancialSchemeId financialSchemeId, bool isPurchasePriceKnown)
    {
        FinancialDetailsId = new FinancialDetailsId(Guid.NewGuid());
        FinancialSchemeId = financialSchemeId;
        IsPurchasePriceKnown = isPurchasePriceKnown;
    }

    public FinancialSchemeId FinancialSchemeId { get; }

    public FinancialDetailsId FinancialDetailsId { get; }

    public PurchasePrice PurchasePrice { get; set; }

    public bool IsPurchasePriceKnown { get; set; }

    public void ProvidePurchasePrice(PurchasePrice price)
    {
        PurchasePrice = price;
    }
}
