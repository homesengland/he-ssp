using System.Text.Json.Serialization;
using HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Contract.Application.Enums;
using ApplicationId = HE.Investment.AHP.Contract.FinancialDetails.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.FinancialDetails.Entities;

public class FinancialDetailsEntity
{
    public FinancialDetailsEntity(ApplicationId applicationId, string applicationName, bool isPurchasePriceKnown)
    {
        ApplicationId = applicationId;
        ApplicationName = applicationName;
        IsPurchasePriceKnown = isPurchasePriceKnown;
    }

    [JsonConstructor]
    public FinancialDetailsEntity(
        bool isPurchasePriceKnown,
        PurchasePrice? purchasePrice,
        LandOwnership? landOwnership)
    {
        IsPurchasePriceKnown = isPurchasePriceKnown;
        PurchasePrice = purchasePrice;
        LandOwnership = landOwnership;
    }

    public ApplicationId ApplicationId { get; private set; }

    public string ApplicationName { get; private set; }

    public PurchasePrice? PurchasePrice { get; private set; }

    public bool? IsPurchasePriceKnown { get; private set; }

    public LandOwnership? LandOwnership { get; private set; }

    public LandValue? LandValue { get; private set; }

    public void ProvidePurchasePrice(PurchasePrice price)
    {
        PurchasePrice = price;
    }

    public void ProvideLandOwnershipAndValue(LandOwnership landOwnership, LandValue landValue)
    {
        LandOwnership = landOwnership;
        LandValue = landValue;
    }
}
