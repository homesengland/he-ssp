using System.Text.Json.Serialization;
using HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Contract.Application.Enums;

namespace HE.Investment.AHP.Domain.FinancialDetails.Entities;

public class FinancialDetailsEntity
{
    public FinancialDetailsEntity(ApplicationID applicationId, string applicationName, FinancialDetailsId? financialDetailsId, bool isPurchasePriceKnown)
    {
        ApplicationId = applicationId;
        ApplicationName = applicationName;
        IsPurchasePriceKnown = isPurchasePriceKnown;
        FinancialDetailsId = FinancialDetailsId.From(financialDetailsId?.Value ?? Guid.NewGuid());
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

    public ApplicationID ApplicationId { get; private set; }

    public string ApplicationName { get; private set; }

    public FinancialDetailsId FinancialDetailsId { get; private set; }

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
