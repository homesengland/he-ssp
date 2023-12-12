using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Errors;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using HE.Investments.Loans.Common.Exceptions;

namespace HE.Investment.AHP.Domain.FinancialDetails.Entities;

public class FinancialDetailsEntity
{
    public FinancialDetailsEntity(ApplicationBasicInfo applicationBasicInfo)
    {
        SectionStatus = SectionStatus.NotStarted;
        ApplicationBasicInfo = applicationBasicInfo;
    }

    public FinancialDetailsEntity(
        ApplicationBasicInfo applicationBasicInfo,
        PurchasePrice? purchasePrice,
        ExpectedPurchasePrice? expectedPurchasePrice,
        CurrentLandValue? landValue,
        bool? isPublicLand,
        ExpectedWorksCosts? expectedWorksCosts,
        ExpectedOnCosts? expectedOnCosts,
        ExpectedContributionsToScheme expectedContributionsToScheme,
        PublicGrants publicGrants,
        SectionStatus sectionStatus)
    {
        ApplicationBasicInfo = applicationBasicInfo;
        PurchasePrice = purchasePrice;
        LandValue = landValue;
        IsPublicLand = isPublicLand;
        ExpectedWorksCosts = expectedWorksCosts;
        ExpectedOnCosts = expectedOnCosts;
        ExpectedPurchasePrice = expectedPurchasePrice;
        ExpectedContributions = expectedContributionsToScheme;
        PublicGrants = publicGrants;
        SectionStatus = sectionStatus;
    }

    public ApplicationBasicInfo ApplicationBasicInfo { get; }

    public PurchasePrice? PurchasePrice { get; private set; }

    public ExpectedPurchasePrice? ExpectedPurchasePrice { get; private set; }

    public CurrentLandValue? LandValue { get; private set; }

    public bool? IsPublicLand { get; private set; }

    public ExpectedWorksCosts? ExpectedWorksCosts { get; private set; }

    public ExpectedOnCosts? ExpectedOnCosts { get; private set; }

    public ExpectedContributionsToScheme ExpectedContributions { get; private set; }

    public PublicGrants PublicGrants { get; private set; }

    public SectionStatus SectionStatus { get; private set; }

    public void ProvideLandStatus(PurchasePrice? purchasePrice, ExpectedPurchasePrice? expectedPurchasePrice)
    {
        if (purchasePrice.IsProvided() && expectedPurchasePrice.IsProvided())
        {
            throw new DomainException(
                $"{PurchasePrice.Fields.DisplayName} cannot be provided together with {ExpectedPurchasePrice.Fields.DisplayName}",
                CommonErrorCodes.InvalidDomainOperation);
        }

        PurchasePrice = purchasePrice;
        ExpectedPurchasePrice = expectedPurchasePrice;
        SetSectionStatus(purchasePrice != null);
    }

    public void ProvideCurrentLandValue(CurrentLandValue? currentLandValue)
    {
        LandValue = currentLandValue;
        SetSectionStatus(currentLandValue.IsProvided());
    }

    public void ProvideIsPublicLand(bool? isPublicLand)
    {
        IsPublicLand = isPublicLand;
        SetSectionStatus(isPublicLand.HasValue);
    }

    public void ProvideOtherApplicationCosts(ExpectedWorksCosts? expectedWorksCosts, ExpectedOnCosts? expectedOnCosts)
    {
        ExpectedWorksCosts = expectedWorksCosts;
        ExpectedOnCosts = expectedOnCosts;
        SetSectionStatus(expectedWorksCosts == null || expectedOnCosts == null);
    }

    public void ProvideExpectedContributions(ExpectedContributionsToScheme expectedContribution)
    {
        ExpectedContributions = expectedContribution;
    }

    public void ProvideGrants(PublicGrants publicGrants)
    {
        PublicGrants = publicGrants;
    }

    public bool AreAllQuestionsAnswered()
    {
        return PurchasePrice.IsProvided() &&
               LandValue.IsProvided() &&
               IsPublicLand.HasValue &&
               ExpectedWorksCosts.IsProvided() &&
               ExpectedOnCosts.IsProvided() &&
               ExpectedContributions.AreAllQuestionsAnswered() &&
               PublicGrants.AreAllQuestionsAnswered();
    }

    public void CompleteFinancialDetails()
    {
        var result = OperationResult.New();

        // Fix this when all fields will be refactored #84615
        // result.Aggregate(() => LandValue.CheckErrors());
        // result.Aggregate(() => PurchasePrice.CheckErrors());
        // result.Aggregate(() => ExpectedCosts.CheckErrors());
        // result.Aggregate(() => Contributions.CheckErrors());
        // result.Aggregate(() => Grants.CheckErrors());
        // if (Contributions.TotalContributions + Grants.TotalGrants != ExpectedCosts.TotalCosts + (PurchasePrice.ActualPrice ?? 0))
        // {
        //     result.AddValidationError(FinancialDetailsValidationFieldNames.CostsAndFunding, FinancialDetailsValidationErrors.CostsAndFundingMismatch);
        // }
        result.CheckErrors();

        SectionStatus = SectionStatus.Completed;
    }

    public decimal ExpectedTotalCosts() => (ExpectedWorksCosts?.Value ?? 0) + (ExpectedOnCosts?.Value ?? 0);

    public decimal ExpectedTotalContributions() => ExpectedContributions.CalculateTotal() + PublicGrants.CalculateTotal();

    private void SetSectionStatus(bool isAnyValueSet)
    {
        if (isAnyValueSet)
        {
            SectionStatus = SectionStatus.InProgress;
        }
    }
}
