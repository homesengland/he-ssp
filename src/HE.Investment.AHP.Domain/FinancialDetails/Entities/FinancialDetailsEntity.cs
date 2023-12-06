using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
using ApplicationId = HE.Investment.AHP.Domain.FinancialDetails.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.FinancialDetails.Entities;

public class FinancialDetailsEntity
{
    public FinancialDetailsEntity(ApplicationId applicationId, string applicationName)
    {
        ApplicationId = applicationId;
        ApplicationName = applicationName;
        SectionStatus = SectionStatus.NotStarted;
    }

    public FinancialDetailsEntity(
        ApplicationId applicationId,
        string applicationName,
        PurchasePrice purchasePrice,
        CurrentLandValue? landValue,
        bool? isPublicLand,
        ExpectedCosts expectedCosts,
        Contributions contributions,
        Grants grants,
        SectionStatus sectionStatus)
    {
        ApplicationId = applicationId;
        ApplicationName = applicationName;
        PurchasePrice = purchasePrice;
        LandValue = landValue;
        IsPublicLand = isPublicLand;
        ExpectedCosts = expectedCosts;
        Contributions = contributions;
        Grants = grants;
        SectionStatus = sectionStatus;
    }

    public ApplicationId ApplicationId { get; private set; }

    public string ApplicationName { get; private set; }

    public PurchasePrice PurchasePrice { get; private set; }

    public CurrentLandValue? LandValue { get; private set; }

    public bool? IsPublicLand { get; private set; }

    public ExpectedCosts ExpectedCosts { get; private set; }

    public Contributions Contributions { get; private set; }

    public Grants Grants { get; private set; }

    public SectionStatus SectionStatus { get; private set; }

    public void ProvidePurchasePrice(PurchasePrice purchasePrice)
    {
        PurchasePrice = purchasePrice;
        SetSectionStatus(purchasePrice.IsAnyValueNotNull);
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

    public void ProvideExpectedCosts(ExpectedCosts expectedCosts)
    {
        ExpectedCosts = expectedCosts;
        SetSectionStatus(expectedCosts.IsAnyValueNotNull);
    }

    public void ProvideContributions(Contributions contributions)
    {
        Contributions = contributions;
        SetSectionStatus(Contributions.IsAnyValueNotNull);
    }

    public void ProvideGrants(Grants grants)
    {
        Grants = grants;
        SetSectionStatus(Grants.IsAnyValueNotNull);
    }

    public void CompleteFinancialDetails()
    {
        var result = OperationResult.New();
        result.Aggregate(() => PurchasePrice.CheckErrors());

        // Fix this
        // result.Aggregate(() => LandValue.CheckErrors());
        result.Aggregate(() => ExpectedCosts.CheckErrors());
        result.Aggregate(() => Contributions.CheckErrors());
        result.Aggregate(() => Grants.CheckErrors());
        if (Contributions.TotalContributions + Grants.TotalGrants != ExpectedCosts.TotalCosts + (PurchasePrice.ActualPrice ?? 0))
        {
            result.AddValidationError(FinancialDetailsValidationFieldNames.CostsAndFunding, FinancialDetailsValidationErrors.CostsAndFundingMismatch);
        }

        result.CheckErrors();

        SectionStatus = SectionStatus.Completed;
    }

    private void SetSectionStatus(bool isAnyValueSet)
    {
        if (isAnyValueSet)
        {
            SectionStatus = SectionStatus.InProgress;
        }
    }
}
