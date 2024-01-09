using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.FinancialDetails.Constants;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.Entities;

public class FinancialDetailsEntity : IQuestion
{
    public FinancialDetailsEntity(ApplicationBasicInfo applicationBasicInfo)
    {
        SectionStatus = SectionStatus.NotStarted;
        ApplicationBasicInfo = applicationBasicInfo;
        LandStatus = new LandStatus();
        LandValue = new LandValue();
        OtherApplicationCosts = new OtherApplicationCosts();
        ExpectedContributions = new ExpectedContributionsToScheme(applicationBasicInfo.Tenure);
        PublicGrants = new PublicGrants();
    }

    public FinancialDetailsEntity(
        ApplicationBasicInfo applicationBasicInfo,
        LandStatus landStatus,
        LandValue landValue,
        OtherApplicationCosts otherApplicationCosts,
        ExpectedContributionsToScheme expectedContributionsToScheme,
        PublicGrants publicGrants,
        SectionStatus sectionStatus)
    {
        ApplicationBasicInfo = applicationBasicInfo;
        LandStatus = landStatus;
        LandValue = landValue;
        OtherApplicationCosts = otherApplicationCosts;
        ExpectedContributions = expectedContributionsToScheme;
        PublicGrants = publicGrants;
        SectionStatus = sectionStatus;
    }

    public ApplicationBasicInfo ApplicationBasicInfo { get; }

    public LandStatus LandStatus { get; private set; }

    public LandValue LandValue { get; private set; }

    public OtherApplicationCosts OtherApplicationCosts { get; private set; }

    public ExpectedContributionsToScheme ExpectedContributions { get; private set; }

    public PublicGrants PublicGrants { get; private set; }

    public SectionStatus SectionStatus { get; private set; }

    public void ProvideLandStatus(LandStatus landStatus)
    {
        ChangeStatus(LandStatus != landStatus);
        LandStatus = landStatus;
    }

    public void ProvideLandValue(LandValue landValue)
    {
        ChangeStatus(LandValue != landValue);
        LandValue = landValue;
    }

    public void ProvideOtherApplicationCosts(OtherApplicationCosts otherApplicationCosts)
    {
        ChangeStatus(OtherApplicationCosts != otherApplicationCosts);
        OtherApplicationCosts = otherApplicationCosts;
    }

    public void ProvideExpectedContributions(ExpectedContributionsToScheme expectedContribution)
    {
        ChangeStatus(ExpectedContributions != expectedContribution);
        ExpectedContributions = expectedContribution;
    }

    public void ProvideGrants(PublicGrants publicGrants)
    {
        ChangeStatus(PublicGrants != publicGrants);
        PublicGrants = publicGrants;
    }

    public bool IsAnswered()
    {
        return LandStatus.IsAnswered() &&
               LandValue.IsAnswered() &&
               OtherApplicationCosts.IsAnswered() &&
               ExpectedContributions.IsAnswered() &&
               PublicGrants.IsAnswered();
    }

    public void CompleteFinancialDetails(IsSectionCompleted isSectionCompleted)
    {
        if (isSectionCompleted == IsSectionCompleted.Undefied)
        {
            OperationResult.New().AddValidationError(nameof(IsSectionCompleted), "Select whether you have completed this section").CheckErrors();
        }

        if (isSectionCompleted == IsSectionCompleted.No)
        {
            SectionStatus = SectionStatus.InProgress;
            return;
        }

        if (!IsAnswered())
        {
            OperationResult
                .New()
                .AddValidationError(nameof(IsSectionCompleted), ValidationErrorMessage.SectionIsNotCompleted).CheckErrors();
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

    public decimal ExpectedTotalCosts() => OtherApplicationCosts.ExpectedTotalCosts() + (LandValue.CurrentLandValue?.Value ?? 0);

    public decimal ExpectedTotalContributions() => ExpectedContributions.CalculateTotal() + PublicGrants.CalculateTotal();

    private void ChangeStatus(bool isChanged)
    {
        SectionStatus = isChanged ? SectionStatus.InProgress : SectionStatus;
    }
}
