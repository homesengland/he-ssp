using System.Globalization;
using System.Text.Json.Serialization;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using ApplicationId = HE.Investment.AHP.Domain.FinancialDetails.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.FinancialDetails.Entities;

public class FinancialDetailsEntity
{
    public FinancialDetailsEntity(ApplicationId applicationId, string applicationName)
    {
        ApplicationId = applicationId;
        ApplicationName = applicationName;
    }

    public FinancialDetailsEntity(
        ApplicationId applicationId,
        string applicationName,
        ActualPurchasePrice? actualPurchasePrice,
        ExpectedPurchasePrice? expectedPurchasePrice,
        LandOwnership? landOwnership,
        LandValue? landValue,
        ExpectedWorksCosts? expectedWorksCosts,
        ExpectedOnCosts? expectedOnCosts,
        RentalIncomeBorrowing? rentalIncomeBorrowing,
        SalesOfHomesOnThisScheme? salesOfHomesOnThisScheme,
        SalesOfHomesOnOtherSchemes? salesOfHomesOnOtherSchemes,
        OwnResources? ownResources,
        RCGFContribution? rCGFContribution,
        OtherCapitalSources? otherCapitalSources,
        SharedOwnershipSales? sharedOwnershipSales,
        HomesTransferValue? homesTransferValue)
    {
        ApplicationId = applicationId;
        ApplicationName = applicationName;
        ActualPurchasePrice = actualPurchasePrice;
        ExpectedPurchasePrice = expectedPurchasePrice;
        LandOwnership = landOwnership;
        LandValue = landValue;
        ExpectedWorksCosts = expectedWorksCosts;
        ExpectedOnCosts = expectedOnCosts;
        RentalIncomeBorrowing = rentalIncomeBorrowing;
        SalesOfHomesOnThisScheme = salesOfHomesOnThisScheme;
        SalesOfHomesOnOtherSchemes = salesOfHomesOnOtherSchemes;
        OwnResources = ownResources;
        RCGFContribution = rCGFContribution;
        OtherCapitalSources = otherCapitalSources;
        SharedOwnershipSales = sharedOwnershipSales;
        HomesTransferValue = homesTransferValue;
    }

    public ApplicationId ApplicationId { get; private set; }

    public string ApplicationName { get; private set; }

    public ActualPurchasePrice? ActualPurchasePrice { get; private set; }

    public ExpectedPurchasePrice? ExpectedPurchasePrice { get; private set; }

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

    public void ProvidePurchasePrice(ActualPurchasePrice? actualPurchasePrice, ExpectedPurchasePrice? expectedPurchasePrice)
    {
        ActualPurchasePrice = actualPurchasePrice;
        ExpectedPurchasePrice = expectedPurchasePrice;
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
