using System.Text.Json.Serialization;
using HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;

namespace HE.Investment.AHP.Domain.FinancialDetails.Entities;

public class FinancialDetailsEntity
{
    public FinancialDetailsEntity()
    {
        FinancialDetailsId = new FinancialDetailsId(Guid.NewGuid());
    }

    [JsonConstructor]
    public FinancialDetailsEntity(
        FinancialDetailsId financialDetailsId,
        bool isPurchasePriceKnown,
        PurchasePrice purchasePrice)
    {
        FinancialDetailsId = financialDetailsId;
        IsPurchasePriceKnown = isPurchasePriceKnown;
        PurchasePrice = purchasePrice;
    }

    public FinancialDetailsId FinancialDetailsId { get; private set; }

    public PurchasePrice PurchasePrice { get; private set; }

    public bool IsPurchasePriceKnown { get; private set; }

    public void ProvidePurchasePrice(PurchasePrice price)
    {
        PurchasePrice = price;
    }
}
