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

    public RentalIncomeBorrowing? RentalIncomeBorrowing { get; private set; }

    public SalesOfHomesOnThisScheme? SalesOfHomesOnThisScheme { get; private set; }

    public SalesOfHomesOnOtherSchemes? SalesOfHomesOnOtherSchemes { get; private set; }

    public OwnResources? OwnResources { get; private set; }

    public RCGFContribution? RCGFContribution { get; private set; }

    public OtherCapitalSources? OtherCapitalSources { get; private set; }

    public SharedOwnershipSales? SharedOwnershipSales { get; private set; }

    public HomesTransferValue? HomesTransferValue { get; private set; }

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

    public void ProvideExpectedCosts(ExpectedWorksCosts? expectedWorksCosts, ExpectedOnCosts? expectedOnCosts)
    {
        ExpectedWorksCosts = expectedWorksCosts;
        ExpectedOnCosts = expectedOnCosts;
    }

    public void ProvideContributions(
        RentalIncomeBorrowing? rentalIncomeBorrowing,
        SalesOfHomesOnThisScheme? salesOfHomesOnThisScheme,
        SalesOfHomesOnOtherSchemes? salesOfHomesOnOtherSchemes,
        OwnResources? ownResources,
        RCGFContribution? rCGFContribution,
        OtherCapitalSources? otherCapitalSources,
        SharedOwnershipSales? sharedOwnershipSales,
        HomesTransferValue? homesTransferValue)
    {
        RentalIncomeBorrowing = rentalIncomeBorrowing;
        SalesOfHomesOnThisScheme = salesOfHomesOnThisScheme;
        SalesOfHomesOnOtherSchemes = salesOfHomesOnOtherSchemes;
        OwnResources = ownResources;
        RCGFContribution = rCGFContribution;
        OtherCapitalSources = otherCapitalSources;
        SharedOwnershipSales = sharedOwnershipSales;
        HomesTransferValue = homesTransferValue;
    }
}
