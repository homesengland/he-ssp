using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Validators;
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
        LandValue landValue,
        ExpectedCosts expectedCosts,
        Contributions contributions,
        Grants grants,
        SectionStatus sectionStatus)
    {
        ApplicationId = applicationId;
        ApplicationName = applicationName;
        PurchasePrice = purchasePrice;
        LandValue = landValue;
        ExpectedCosts = expectedCosts;
        Contributions = contributions;
        Grants = grants;
        SectionStatus = sectionStatus;
    }

    public ApplicationId ApplicationId { get; private set; }

    public string ApplicationName { get; private set; }

    public PurchasePrice PurchasePrice { get; private set; }

    public LandValue LandValue { get; private set; }

    public ExpectedCosts ExpectedCosts { get; private set; }

    public Contributions Contributions { get; private set; }

    public Grants Grants { get; private set; }

    public SectionStatus SectionStatus { get; private set; }

    public void ProvidePurchasePrice(PurchasePrice purchasePrice)
    {
        PurchasePrice = purchasePrice;
        SectionStatus = SectionStatus.InProgress;
    }

    public void ProvideLandValue(LandValue landValue)
    {
        LandValue = landValue;
        if (landValue.Value.HasValue || landValue.IsLandPublic.HasValue)
        {
            SectionStatus = SectionStatus.InProgress;
        }
    }

    public void ProvideExpectedCosts(ExpectedCosts expectedCosts)
    {
        ExpectedCosts = expectedCosts;
        if (expectedCosts.WorksCosts.HasValue || expectedCosts.OnCosts.HasValue)
        {
            SectionStatus = SectionStatus.InProgress;
        }
    }

    public void ProvideContributions(Contributions contributions)
    {
        Contributions = contributions;
        if (Contributions.RentalIncomeBorrowing != null
            || Contributions.SalesOfHomesOnThisScheme != null
            || Contributions.SalesOfHomesOnOtherSchemes != null
            || Contributions.OwnResources != null
            || Contributions.RCGFContributions != null
            || Contributions.OtherCapitalSources != null
            || Contributions.SharedOwnershipSales != null
            || Contributions.HomesTransferValue != null)
        {
            SectionStatus = SectionStatus.InProgress;
        }
    }

    public void ProvideGrants(Grants grants)
    {
        Grants = grants;

        if (Grants.CountyCouncil != null
            || Grants.DHSCExtraCare != null
            || Grants.LocalAuthority != null
            || Grants.SocialServices != null
            || Grants.HealthRelated != null
            || Grants.Lottery != null
            || Grants.OtherPublicBodies != null)
        {
            SectionStatus = SectionStatus.InProgress;
        }
    }

    public void CompleteFinancialDetails()
    {
        var result = OperationResult.New();
        result.Aggregate(() => PurchasePrice.CheckErrors());
        result.Aggregate(() => LandValue.CheckErrors());
        result.Aggregate(() => ExpectedCosts.CheckErrors());
        result.Aggregate(() => Contributions.CheckErrors());
        result.Aggregate(() => Grants.CheckErrors());
        result.CheckErrors();

        SectionStatus = SectionStatus.Completed;
    }
}
