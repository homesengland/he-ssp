using System.Text.Json.Serialization;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Contract.Application.Enums;
using ApplicationId = HE.Investment.AHP.Domain.FinancialDetails.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.FinancialDetails.Entities;

public class FinancialDetailsEntity
{
    public FinancialDetailsEntity(ApplicationId applicationId, string applicationName, bool isPurchasePriceKnown)
    {
        ApplicationId = applicationId;
        ApplicationName = applicationName;
        IsPurchasePriceKnown = isPurchasePriceKnown;
    }

    public FinancialDetailsEntity(
        bool isPurchasePriceKnown,
        PurchasePrice? purchasePrice,
        LandOwnership? landOwnership,
        ExpectedWorksCosts? expectedWorksCosts,
        ExpectedOnCosts? expectedOnCosts)
    {
        IsPurchasePriceKnown = isPurchasePriceKnown;
        PurchasePrice = purchasePrice;
        LandOwnership = landOwnership;
        ExpectedWorksCosts = expectedWorksCosts;
        ExpectedOnCosts = expectedOnCosts;
    }

    public ApplicationId ApplicationId { get; private set; }

    public string ApplicationName { get; private set; }

    public PurchasePrice? PurchasePrice { get; private set; }

    public bool? IsPurchasePriceKnown { get; private set; }

    public LandOwnership? LandOwnership { get; private set; }

    public LandValue? LandValue { get; private set; }

    public ExpectedWorksCosts? ExpectedWorksCosts { get; private set; }

    public ExpectedOnCosts? ExpectedOnCosts { get; private set; }

    public void ProvidePurchasePrice(PurchasePrice price)
    {
        PurchasePrice = price;
    }

    public void ProvideLandOwnership(LandOwnership landOwnership)
    {
        LandOwnership = landOwnership;
    }

    public void ProvideLandValue(LandValue landValue)
    {
        LandValue = landValue;
    }

    public void ProvideExpectedWorksCosts(ExpectedWorksCosts expectedWorksCosts)
    {
        ExpectedWorksCosts = expectedWorksCosts;
    }

    public void ProvideExpectedOnCosts(ExpectedOnCosts expectedOnCosts)
    {
        ExpectedOnCosts = expectedOnCosts;
    }
}
