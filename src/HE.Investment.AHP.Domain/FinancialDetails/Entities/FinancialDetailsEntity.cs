using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Errors;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
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
        OtherApplicationCosts otherApplicationCosts,
        ExpectedContributionsToScheme expectedContributionsToScheme,
        PublicGrants publicGrants,
        SectionStatus sectionStatus)
    {
        ApplicationBasicInfo = applicationBasicInfo;
        PurchasePrice = purchasePrice;
        LandValue = landValue;
        IsPublicLand = isPublicLand;
        OtherApplicationCosts = otherApplicationCosts;
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

    public OtherApplicationCosts OtherApplicationCosts { get; private set; }

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
    }

    public void ProvideCurrentLandValue(CurrentLandValue? currentLandValue)
    {
        LandValue = currentLandValue;
    }

    public void ProvideIsPublicLand(bool? isPublicLand)
    {
        IsPublicLand = isPublicLand;
    }

    public void ProvideOtherApplicationCosts(OtherApplicationCosts otherApplicationCosts)
    {
        OtherApplicationCosts = otherApplicationCosts;
    }

    public void ProvideExpectedContributions(ExpectedContributionsToScheme expectedContribution)
    {
        ExpectedContributions = expectedContribution;
    }

    public void ProvideGrants(PublicGrants publicGrants)
    {
        PublicGrants = publicGrants;
    }

    public bool IsAnswered()
    {
        return PurchasePrice.IsProvided() &&
               LandValue.IsProvided() &&
               IsPublicLand.HasValue &&
               OtherApplicationCosts.IsAnswered() &&
               ExpectedContributions.IsAnswered() &&
               PublicGrants.IsAnswered();
    }

    public void CompleteFinancialDetails()
    {
        if (!IsAnswered())
        {
            OperationResult
                .New()
                .AddValidationError("IsSectionCompleted", ValidationErrorMessage.SectionIsNotCompleted).CheckErrors();
        }

        if (ExpectedTotalCosts() != ExpectedTotalContributions())
        {
            OperationResult
                .New()
                .AddValidationError(FinancialDetailsValidationFieldNames.CostsAndFunding, FinancialDetailsValidationErrors.CostsAndFundingMismatch)
                .CheckErrors();
        }

        SectionStatus = SectionStatus.Completed;
    }

    public decimal ExpectedTotalCosts() => OtherApplicationCosts.ExpectedTotalCosts();

    public decimal ExpectedTotalContributions() => ExpectedContributions.CalculateTotal() + PublicGrants.CalculateTotal();
}
