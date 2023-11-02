using System.Text.Json.Serialization;
using HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Contract.Application.Enums;

namespace HE.Investment.AHP.Domain.FinancialDetails.Entities;

public class FinancialDetailsEntity
{
    public FinancialDetailsEntity()
    {
        FinancialDetailsId = new FinancialDetailsId(Guid.NewGuid());
    }

    public FinancialDetailsEntity(bool isPurchasePriceKnown)
    {
        FinancialDetailsId = new FinancialDetailsId(Guid.NewGuid());
        IsPurchasePriceKnown = isPurchasePriceKnown;
    }

    [JsonConstructor]
    public FinancialDetailsEntity(
        FinancialDetailsId financialDetailsId,
        bool isPurchasePriceKnown,
        PurchasePrice? purchasePrice,
        LandOwnership? landOwnership)
    {
        FinancialDetailsId = financialDetailsId;
        IsPurchasePriceKnown = isPurchasePriceKnown;
        PurchasePrice = purchasePrice;
        LandOwnership = landOwnership;
    }

    public FinancialDetailsId FinancialDetailsId { get; private set; }

    public PurchasePrice? PurchasePrice { get; private set; }

    public bool? IsPurchasePriceKnown { get; private set; }

    public LandOwnership? LandOwnership { get; private set; }

    public void ProvidePurchasePrice(PurchasePrice price)
    {
        PurchasePrice = price;
    }

    public void ProvideLandOwnership(LandOwnership landOwnership)
    {
        LandOwnership = landOwnership;
    }
}
